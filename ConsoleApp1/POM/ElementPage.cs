using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace ConsoleApp1.POM
{
    public class ElementPage
    {

        #region Variable initializations. 

        private IWebDriver Driver;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor

        public ElementPage(IWebDriver Driver)
        {
            this.Driver = Driver;
            PageFactory.InitElements(Driver, this);
        }

        #endregion



        #region Elemennts

        [FindsBy(How = How.XPath, Using = "//*[@id='withOptGroup']")]
        private IWebElement DDLSelectValue { get; set; }

        By UploadedFilePath = By.XPath("//*[@id='uploadedFilePath']");

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

        By BtnChooseFile = By.XPath("//*[@id='uploadFile']");


        By GridRecords = By.XPath("//button[@id = 'addNewRecordButton']/../../following-sibling::div//div[@class='rt-td']");

        By RecordToDelete(string RecordToDelete)
        {
            return By.XPath("//button[@id = 'addNewRecordButton']/../../following-sibling::div//div[@class='rt-td' and text() ='" + RecordToDelete + "']/parent::div//span[@title='Delete']");
        }

        #endregion

        public void ClickOnLeftPaneElement(IWebDriver Driver, string groupheader, string ElementName)
        {
            _logger.Trace("Attempting to interact elements in the Left Pane. Element Name = " + ElementName);
            WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));

            if (BrowserWait.Until(ExpectedConditions.ElementExists(DivGroupHeader(groupheader))).GetAttribute("class").Equals("element-list collapse"))
            {
                TestUtility.UtilityClass.ScrollToElement(Driver.FindElement(GrouHeaderToClick(groupheader)));
                BrowserWait.Until(ExpectedConditions.ElementToBeClickable(GrouHeaderToClick(groupheader))).Click();
            }

            TestUtility.UtilityClass.ScrollToElement(Driver.FindElement(LeftPaneElement(groupheader, ElementName)));
            BrowserWait.Until(ExpectedConditions.ElementToBeClickable(LeftPaneElement(groupheader, ElementName))).Click();
            _logger.Info("Successfully Clicked on " + ElementName + " in left pane.");
        }

        public void SelectValueFromDroDown()
        {
            TestUtility.UtilityClass.SelectValueFromResponsiveDDL(DDLSelectValue, DDLSelectValueEntry, "Group 1, option 2");

            _logger.Info("Value selected from the drop down successfully.");
        }

        public void UploadFile(string FilePathFromToUpload)
        {
            _logger.Trace("Attempting to upload file.");
            WebDriverWait _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement EleBrowseButton = _wait.Until(ExpectedConditions.ElementIsVisible(BtnChooseFile));

            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", EleBrowseButton);
            Thread.Sleep(2000);
            TestUtility.UtilityClass.FileUploader(FilePathFromToUpload);
            _logger.Info("Files uploaded successfully.");
        }

        public string MethodUploadedFilePath()
        {
            WebDriverWait _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement EleUploadedFilePath = _wait.Until(ExpectedConditions.ElementIsVisible(UploadedFilePath));
            return EleUploadedFilePath.Text.ToString();
        }

        public void DeleteRecordFromGrid(string NameReocrdsToDelete)
        {

            WebDriverWait _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(GridRecords));

            _wait.Until(ExpectedConditions.ElementToBeClickable(RecordToDelete(NameReocrdsToDelete))).Click();

        }

        public bool VerifyDeletedRecordsInGrid(string DeletedRecords)
        {
            bool IsDeletedRecordExist = false;

            WebDriverWait _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));

            _wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(GridRecords));

            IList<IWebElement> GridCollection = Driver.FindElements(GridRecords);

            foreach (WebElement Record in GridCollection)
            {string abc = Record.Text;


                if (Record.Text.Equals(DeletedRecords))
                {
                    IsDeletedRecordExist = true;
                    break;
                }
            }

             return IsDeletedRecordExist;
        }

    }
}
