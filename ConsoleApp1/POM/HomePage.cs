using NLog;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System.ComponentModel;

namespace ConsoleApp1.POM
{
    public class HomePage
    {

        #region Webdriver 

        private readonly IWebDriver Driver;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor

        public HomePage(IWebDriver Driver)
        {
            this.Driver = Driver;
            PageFactory.InitElements(Driver, this);
        }

        #endregion

        #region Enum

        public enum EnumLeftPaneGroupHeader
        {
            [Description("Elements")]
            Elements,

            [Description("Forms")]
            Forms,

            [Description("Alerts, Frame & Windows")]
            AlertFrameWindow,

            [Description("Widgets")]
            Widgets,

            [Description("Interactions")]
            Interactions

        }

        public enum EnumLeftPaneElementList
        {
            #region Elements           

            [Description("Text Box")]
            TextBox,

            [Description("Check Box")]
            CheckBox,

            [Description("Radio Button")]
            RadioButton,

            [Description("Web Tables")]
            WebTables,

            [Description("Buttons")]
            Buttons,

            [Description("Upload and Download")]
            UploadAndDownload,

            #endregion

            #region Widgets

            [Description("Select Menu")]
            SelectMenu,

            #endregion


            #region Forms

            [Description("Practice Form")]
            PracticeForm


            #endregion

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
