using MercatorTest_PhilJarvis.Web.ClientPortal.Global;
using MercatorTest_PhilJarvis.Web.Shared;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using System;
using System.Threading;

namespace MercatorTest_PhilJarvis.Web.ClientPortal
{
    public class Entrance : AbstractPage
    {
        public Entrance(IWebDriver driver) 
            : base(driver, true)
        {
        }

        [FindsBy(How = How.CssSelector, Using = "#block_top_menu > ul > li:nth-child(2) > a")]
        protected IWebElement DressesMenu { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#center_column > ul > li:nth-child(2) > div > div.left-block > div > a.product_img_link > img")]
        protected IWebElement HighestPriceItem { get; set; }

        [FindsBy(How = How.XPath, Using = "/html/body/div[2]/div/div/div/div/iframe")]
        protected IWebElement PopUpFrame { get; set; }

        [FindsBy(How = How.Name, Using ="Submit")]
        protected IWebElement AddToCart {get; set; }

        public Header Login(Uri siteAddress)
        {
            var header = new Header(Driver);
            Navigate(siteAddress);

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(60));

            if (IsReady())
            {
                Thread.Sleep(5000);
                OpenDressesMenu();
                Thread.Sleep(5000);
                SelectTheHighestPriceItem();
                Thread.Sleep(5000);
                SwitchFrame();
                Thread.Sleep(5000);
                AddToShoppingCart();
                Thread.Sleep(5000);
            }

            return header;
        }

        public void OpenDressesMenu()
        {
            Click(DressesMenu, "Clicking on the dresses menu");
        }

        public void SelectTheHighestPriceItem()
        {
            Click(HighestPriceItem, "Clicking on the Highest Price Dress");
        }

        public void SwitchFrame()
        {
            Driver.SwitchTo().Frame(PopUpFrame);
        }

        public void AddToShoppingCart()
        {

            Click(AddToCart, "Clicking to add to cart");
        }
    }
}
