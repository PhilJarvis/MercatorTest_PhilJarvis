using MercatorTest_PhilJarvis.Web.ClientPortal.Global;
using MercatorTest_PhilJarvis.Web.Shared;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;

namespace MercatorTest_PhilJarvis.Web.ClientPortal
{
    public class Entrance : AbstractPage
    {
        public Entrance(IWebDriver driver) 
            : base(driver, true)
        {
        }

        public Header Login(Uri siteAddress)
        {
            var header = new Header(Driver);
            Navigate(siteAddress);

            return header;
        }

        [FindsBy(How = How.CssSelector, Using = "#stores_block_left")]
        protected IWebElement DressesMenu { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#center_column > ul > li:nth-child(2) > div > div.right-block > div.content_price > span")]
        protected IWebElement HighestPriceItem { get; set; }

        [FindsBy(How = How.Id, Using ="add_to_cart")]
        protected IWebElement SendToCart {get; set; }

        public Header OpenDressesMenu()
        {
            return Click(DressesMenu, new Header(Driver), "Clicking on the dresses menu");
        }

        public Header SelectTheHighestPriceItem()
        {
            return Click(HighestPriceItem, new Header(Driver), "Clicking on the Highest Price Dress");
        }

        public Header SendToShoppingCart()
        {
            return Click(SendToCart, new Header(Driver), "Clicking to send to cart");
        }
    }
}
