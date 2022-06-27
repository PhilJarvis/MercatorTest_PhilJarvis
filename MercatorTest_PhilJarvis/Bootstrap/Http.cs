using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Caching;
using System.Xml.Linq;

namespace MercatorTest_PhilJarvis.Bootstrap
{
    public class AuthWrapper
    {
        private const int CacheCredentialsDurationMinutes = 60;
        private readonly MemoryCache cache = MemoryCache.Default;

        protected readonly Uri webUrl;
        protected readonly string username;
        protected readonly string password;
        protected readonly bool useCache;

        public AuthWrapper(Uri webUrl, string username, string password, bool useCache)
        {
            this.webUrl = webUrl ?? throw new ArgumentNullException(nameof(webUrl));
            this.username = username ?? throw new ArgumentNullException(nameof(username));
            this.password = password ?? throw new ArgumentNullException(nameof(password));
            this.useCache = useCache;
        }

        public CookieContainer GetCookieContainer()
        {
            var cacheKey = string.Concat(webUrl.ToString().ToLower(), username);

            if (useCache && cache.Contains(cacheKey))
            {
                return cache.Get(cacheKey) as CookieContainer;
            }

            var cookies = GetUserCredentials(webUrl, username, password);
            var credentials = cookies.Cast<Cookie>().Where(cookie => cookie.Name.StartsWith("fedAuth") || cookie.Name.StartsWith(".AspNet.Federation"));

            var cookieContainer = new CookieContainer();

            foreach(var cookie in credentials)
            {
                cookieContainer.Add(cookie);
            }

            // see if we can insert into the cache
            if (useCache && !cache.Contains(cacheKey))
            {
                cache.Set(cacheKey, cookieContainer, DateTime.UtcNow.AddMinutes(CacheCredentialsDurationMinutes));
            }

            return cookieContainer;

        }

        private CookieCollection GetUserCredentials(Uri appUri, string username, string password)
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler { CookieContainer = cookieContainer };

            // disable redirects purely for debugging purposes
            handler.AllowAutoRedirect = false;

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("text/html", 0.9));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xhtml+xml", 0.9));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml", 0.9));
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*", 0.8));

                ServerCertificationValidation();

                var location = GetLoginLocation(client, appUri);

                // if we now have an HTTP 302 Found with SSO Location, do a GET to get the login page
                string LogInPageHtml;
                using (var logInResponseRequest = client.GetAsync(location).Result)
                {
                    LogInPageHtml = logInResponseRequest.Content.ReadAsStringAsync().Result;
                }

                // work out from the form where we need to post back to
                var signInTarget = location.GetLeftPart(UriPartial.Authority) + ParseFormActionAttribute(LogInPageHtml);

                // build the login details
                var credentials = new Dictionary<string, string>()
                {
                    {"UserName", username },
                    {"Password", password },
                    {"AuthMethod", "FormsAuthentication" },
                };

                Uri authPostRedirectLocation;
                using (var content = new FormUrlEncodedContent(credentials))
                {
                    // post the login details back to the server - this will return us a 302 Found WITH THE saml TOKEN LOCATION
                    var loginResponse = client.PostAsync(signInTarget, content).Result;

                    if (loginResponse.StatusCode != HttpStatusCode.Found)
                    {
                        throw new InvalidOperationException(string.Format("Posting login credetials to {0} returned status code {1} (expected 302 Found)", signInTarget, loginResponse.StatusCode));

                    }

                    authPostRedirectLocation = loginResponse.Headers.Location;
                }

                Uri authFormAction;
                var authFormElements = new Dictionary<string, string>();
                using(var authResponse = client.GetAsync(authPostRedirectLocation).Result)
                {
                    // Extract the SAML token
                    var responseContentString = authResponse.Content.ReadAsStringAsync().Result.Replace("&&", "&amp;");
                    var responseContent = XDocument.Parse(responseContentString);
                    var form = responseContent.Document.Descendants("form").Single();

                    // get the Form Elements that the App requires
                    foreach(var inputElement in form.Elements("input").Where(e => e.HasAttributes && e.Attribute("type").Value == "hidden"))
                    {
                        authFormElements.Add(inputElement.Attribute("name").Value, inputElement.Attribute("value").Value);
                    }

                    authFormAction = new Uri(form.Attribute("action").Value);
                }

                // now post our credentials to the app server to get the import FedAuth cookie
                using(var formParms = new FormUrlEncodedContent(authFormElements))
                {
                    using(var samlPostResult = client.PostAsync(authFormAction,formParms).Result)
                    {
                        if(samlPostResult.StatusCode != HttpStatusCode.Found)
                        {
                            throw new InvalidOperationException("Could not post the SAML token to the applicatiopn server");
                        }

                        GetHeadersCookies(samlPostResult, ref cookieContainer);
                    }
                }

                var cookies = GetAllCookies(cookieContainer);
                return cookies;
            }
        }

        protected virtual CookieCollection GetAllCookies(CookieContainer container)
        {
            var allCookies = new CookieCollection();
            var domainTableField = container.GetType().GetRuntimeFields().FirstOrDefault(x => x.Name == "m_domainTable");
            var domains = (IDictionary)domainTableField.GetValue(container);

            foreach (var val in domains.Values)
            {
                var type = val.GetType().GetRuntimeFields().First(x => x.Name == "m_list");
                var values = (IDictionary)type.GetValue(val);
                foreach(CookieCollection cookies in values.Values)
                {
                    allCookies.Add(cookies);
                }
            }

            return allCookies;
        }

        private void GetHeadersCookies(HttpResponseMessage response, ref CookieContainer cookieContainer)
        {
            IEnumerable<string> missedCookies = response.Headers.GetValues("Set-Cookie");
            foreach (var c in missedCookies)
            {
                var headerCookie = c.Split(';').Select(hc => hc.Split('=')).ToList();
                Cookie cookie = new Cookie { Name = headerCookie[0][0], Value = headerCookie[0][1] };
                CookieCollection cookieCollection = cookieContainer.GetCookies(response.RequestMessage.RequestUri);

                if(cookieCollection[cookie.Name] == null)
                {
                    cookieContainer.Add(webUrl, cookie);
                }
            }
        }

        private Uri GetLoginLocation(HttpClient client, Uri targetSite)
        {
            targetSite = new Uri(webUrl + ConfigurationData.LocalConfig.Instance.WebUrlQueryString);
            using (var result = client.GetAsync(targetSite).Result)
            {
                if (result.StatusCode != HttpStatusCode.Found)
                {
                    throw new InvalidOperationException(string.Format("Requesting URI {0} did not respond with HTTP302 Found", targetSite));
                }

                var location = result.Headers.Location;
                if (location.OriginalString.ToLower().Contains("/adfs/"))
                {
                    return GetLoginLocation(client, location);
                }

                return location;
            }
        }

        private void ServerCertificationValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, errors) => true;
        }

        protected virtual string ParseFormActionAttribute(string html)
        {

            var formElementStart = html.IndexOf("<form");
            var formElementActionStart = html.IndexOf("action=", formElementStart) + "action=\"".Length;
            var forElementActionEnd = html.IndexOf("\"", formElementActionStart);

            return html.Substring(formElementActionStart, forElementActionEnd - formElementActionStart);
        }
    }

    public class CookieAwareHttpClient : HttpClient
    {
        private CookieContainer container;

        public CookieAwareHttpClient(CookieContainer container) : base(new HttpClientHandler { CookieContainer = container, ClientCertificateOptions = ClientCertificateOption.Automatic})
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void SetHeaders(string appCode, Uri uri)
        {
            DefaultRequestHeaders.Add("ApplicationId", string.Format("{0}/{1}", uri.Host, appCode));
            DefaultRequestHeaders.Add("ApplicationSessionId",new Guid().ToString());
            DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }

    public class CookieAwareWebClient : WebClient
    {
        private readonly CookieContainer container;

        public CookieAwareWebClient(CookieContainer container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            var webRequest = request as HttpWebRequest;

            if (webRequest != null)
            {
                webRequest.CookieContainer = container;
            }
            return request;
        }
    }
}
