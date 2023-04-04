using ConsoleApp1.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1.POM
{
    public class ElementPage
    {

        #region Webdriver 

        private IWebDriver Driver;

        #endregion

        #region Constructor

        public ElementPage(IWebDriver Driver)
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
            SelectMenu

            #endregion

        }

        #endregion

        #region Elemennts

        [FindsBy(How = How.XPath, Using = "//*[@id='withOptGroup']")]
        private IWebElement DDLSelectValue { get; set; }

        By DDLSelectValueEntry = By.XPath("//*[@id='withOptGroup']//div[contains(@class, 'option')]");


        By GrouHeaderToClick(string groupheader)
        {
            return By.XPath("//*[@class='element-group']//*[text() ='" + groupheader + "']");
        }
        
        By DivGroupHeader(string groupheader)
        {
            return By.XPath("//*[@class='element-group']//*[text() ='" + groupheader + "']/../../../div[contains(@class, 'element-list')]");
        }

        By LeftPaneElement(string groupheader, string ElementName)
        {
            return By.XPath("//*[@class='element-group']//*[text() ='" + groupheader + "']/../../..//span[text() = '" + ElementName + "']");
        }


        [FindsBy(How = How.XPath, Using = "//*[@id='uploadFile']")]
        private IWebElement BtnChooseFile { get; set; }


        #endregion

        public void ClickOnLeftPaneElement(IWebDriver Driver, string groupheader, string ElementName)
        {
            WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));


            if (BrowserWait.Until(ExpectedConditions.ElementExists(DivGroupHeader(groupheader))).GetAttribute("class").Equals("element-list collapse"))
            {
                TestUtility.UtilityClass.ScrollToElement(Driver.FindElement(GrouHeaderToClick(groupheader)));
                BrowserWait.Until(ExpectedConditions.ElementToBeClickable(GrouHeaderToClick(groupheader))).Click();
            }

            TestUtility.UtilityClass.ScrollToElement(Driver.FindElement(LeftPaneElement(groupheader, ElementName)));
            BrowserWait.Until(ExpectedConditions.ElementToBeClickable(LeftPaneElement(groupheader, ElementName))).Click();

        }

        public void SelectValueFromDroDown()
        {
            TestUtility.UtilityClass.SelectValueFromResponsiveDDL(DDLSelectValue, DDLSelectValueEntry, "Group 1, option 2");
        }

        public void UploadFile(string FilePathFromToUpload)
        {
            TestUtility.UtilityClass.WaitForElementToBeClickable(BtnChooseFile);
            IWebElement abc = Driver.FindElement(By.Id("uploadFile"));

            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", abc);
            BtnChooseFile.Click();
            Thread.Sleep(1000);
            TestUtility.UtilityClass.FileUploader(FilePathFromToUpload);
        }

    }
}
