using ConsoleApp1.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1.POM
{


    public class HomePage
    {

        #region Webdriver 

        private IWebDriver Driver;

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

        By Element1 = By.XPath("//*[@id='app']/div/div/div[2]/div/div[1]/div/div[2]");

        #endregion

        #region Methods

        public void ClickOnElements(IWebDriver Driver)
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));

            wait.Until(ExpectedConditions.ElementToBeClickable(elements));

            IJavaScriptExecutor JS = (IJavaScriptExecutor)Driver;

            JS.ExecuteScript("arguments[0].scrollIntoView(true);", elements);


            elements.Click();
        }

        #endregion

    }
}
