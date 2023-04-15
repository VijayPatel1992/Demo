using NLog;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;


namespace ConsoleApp1.POM
{
    public class HomePage
    {

        #region Webdriver 

        private IWebDriver Driver;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor

        public HomePage(IWebDriver Driver)
        {
            this.Driver = Driver;
            PageFactory.InitElements(Driver, this);
        }

        #endregion

        #region Elemennts

        [FindsBy(How = How.XPath, Using = "//*[@id='app']/div/div/div[2]/div/div[1]/div/div[2]")]
        private IWebElement elements { get; set; }

        #endregion

        #region Methods

        public void ClickOnElements(IWebDriver Driver)
        {
            _logger.Trace("Attempting to Click on Elements.");

            TestUtility.UtilityClass.WaitForElementToBeClickable(elements);

            TestUtility.UtilityClass.ScrollToElement(elements);

            elements.Click();

            _logger.Info("Successfully clicked on the elements.");
        }


        #endregion

    }
}
