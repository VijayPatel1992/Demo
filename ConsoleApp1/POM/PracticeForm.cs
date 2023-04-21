using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;

namespace ConsoleApp1.POM
{
    public class PracticeForm
    {
        #region Webdriver 

        private IWebDriver Driver;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor
        public PracticeForm(IWebDriver Driver)
        {
            this.Driver = Driver;
            PageFactory.InitElements(Driver, this);
        }

        #endregion

        #region Elements

        By FirstName = By.Id("firstName");

        [FindsBy(How = How.Id, Using = "lastName")]
        private IWebElement TxtBoxLastName { get; set; }

        [FindsBy(How = How.Id, Using = "userEmail")]
        private IWebElement TxtBoxUserEmail { get; set; }

        By RadioBtnGender(string Gender)
        {
            return By.XPath("//*[@id='genterWrapper']//input/../label[text() = '" + Gender + "']");
        }

        [FindsBy(How = How.Id, Using = "userNumber")]
        private IWebElement TxtBoxMobileNumber { get; set; }

        [FindsBy(How = How.Id, Using = "dateOfBirthInput")]
        private IWebElement DatePickerDateOfBirth { get; set; }

        [FindsBy(How = How.Id, Using = "subjectsInput")]
        private IWebElement TxtBoxSubject { get; set; }

        string JSPathHobbiesCheckbox(int Index)
        {
            return "return document.querySelector('#hobbiesWrapper>div div:nth-child(" + Index + ") input')"
;
        }

        [FindsBy(How = How.Id, Using = "uploadPicture")]
        private IWebElement InputBoxChooseFile { get; set; }

        By BtnChooseFile = By.XPath("//*[@id='uploadPicture']");

        [FindsBy(How = How.Id, Using = "currentAddress")]
        private IWebElement TxtBoxCurrentAddress { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='state']")]
        private IWebElement DDLSelectState { get; set; }

        By DDLStateOptions = By.XPath("//*[@id='state']//div[contains(@id, 'react-select')]");

        [FindsBy(How = How.XPath, Using = "//*[@id='city']")]
        private IWebElement DDLSelectCity { get; set; }

        By DDLCityOptions = By.XPath("//*[@id='city']//div[contains(@id, 'react-select')]");

        [FindsBy(How = How.XPath, Using = "//*[@id='submit']")]
        private IWebElement BtnSubmit { get; set; }

        By TableFieldValue(string FieldName)
        {
            return By.XPath("//div[text() ='Thanks for submitting the form']/ancestor::div[@class= 'modal-content']//tr/td[text() = '" + FieldName + "']/following-sibling::td");
        }

        #endregion

        #region Methods

        public void FillPracticeForm(int DtRowPtr, DataTable PracticeFormData, string UploadFilePath)
        {
            WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));
            Actions _Actions = new Actions(Driver);
            IJavaScriptExecutor Js = (IJavaScriptExecutor)Driver;

            #region Fill basic details

            //Wait for First Name textbox becomes clickable and then send key.
            BrowserWait.Until(ExpectedConditions.ElementToBeClickable(FirstName)).SendKeys(PracticeFormData.Rows[DtRowPtr]["FirstName"].ToString());
            _logger.Info("First Name value set.");

            //Fill value for Last Name
            TxtBoxLastName.SendKeys(PracticeFormData.Rows[DtRowPtr]["LastName"].ToString());
            _logger.Info("Last Name value set.");

            //Fill value for Email
            TxtBoxUserEmail.SendKeys(PracticeFormData.Rows[DtRowPtr]["Email"].ToString());
            _logger.Info("Email value set.");

            //Select Gender Radio button.
            Driver.FindElement(RadioBtnGender(PracticeFormData.Rows[DtRowPtr]["Gender"].ToString())).Click();
            _logger.Info("Gender radio button selected.");

            //Fill value for Mobile number
            TxtBoxMobileNumber.SendKeys(PracticeFormData.Rows[DtRowPtr]["MobileNumber"].ToString());
            _logger.Info("Mobile value set.");
            #endregion

            #region Birth Date

            DatePickerDateOfBirth.Click();
            _Actions.KeyDown(Keys.Control).SendKeys("A").KeyUp(Keys.Control).Build().Perform();
            DatePickerDateOfBirth.SendKeys(PracticeFormData.Rows[DtRowPtr]["DateOfBirthd"].ToString());
            DatePickerDateOfBirth.SendKeys(Keys.Enter);
            Thread.Sleep(1000);
            _logger.Info("Date of birth set.");

            #endregion

            #region Subject

            TestUtility.UtilityClass.ScrollToElement(TxtBoxSubject);
            //Fill value of subject
            TxtBoxSubject.Click();

            string[] ArrSubject = PracticeFormData.Rows[DtRowPtr]["Subject"].ToString().Split(';');

            foreach (string SubjectToSelect in ArrSubject)
            {
                TxtBoxSubject.SendKeys(SubjectToSelect);
                TxtBoxSubject.SendKeys(Keys.Enter);
                Thread.Sleep(500);
            }
            _logger.Info("All subject selected.");

            #endregion

            #region Hobbies

            string[] Hobbies = PracticeFormData.Rows[DtRowPtr]["Hobbies"].ToString().Split(';');
            IList<IWebElement> EleHobbies = Driver.FindElements(By.XPath("//*[@id='hobbiesWrapper']//input/parent::div"));

            foreach (string Hoby in Hobbies)
            {
                for (int i = 0; i < EleHobbies.Count; i++)
                {
                    if (Hoby.Equals(EleHobbies[i].Text.ToString()))
                    {
                        IWebElement checkbox = (IWebElement)Js.ExecuteScript(JSPathHobbiesCheckbox(i + 1));

                        if (!(bool)((IJavaScriptExecutor)Driver).ExecuteScript("return arguments[0].checked;", checkbox))
                        {
                            EleHobbies[i].Click();
                        }
                    }

                }
            }
            _logger.Info("Checkbox for Hobbies are selected.");

            #endregion

            #region File Upload

            _logger.Trace("Attempting to upload file.");
            WebDriverWait _wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement EleBrowseButton = _wait.Until(ExpectedConditions.ElementIsVisible(BtnChooseFile));

            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", EleBrowseButton);
            Thread.Sleep(2000);
            TestUtility.UtilityClass.FileUploader(Path.Combine(UploadFilePath, PracticeFormData.Rows[DtRowPtr]["PicturePath"].ToString()));
            _logger.Info("Files uploaded successfully.");

            #endregion

            #region Current Address

            TestUtility.UtilityClass.ScrollToElement(TxtBoxCurrentAddress);
            TxtBoxCurrentAddress.SendKeys(PracticeFormData.Rows[DtRowPtr]["CurrentAddress"].ToString());
            _logger.Info("Current address value set");

            #endregion

            #region Select State

            _Actions.KeyDown(Keys.Tab).KeyUp(Keys.Tab).Build().Perform();
            Thread.Sleep(500);
            _Actions.KeyDown(Keys.Space).KeyUp(Keys.Space).Build().Perform();


            //TestUtility.UtilityClass.ScrollToElement(DDLSelectState);
            //TestUtility.UtilityClass.JavaScriptClick(DDLSelectState);
            //DDLSelectState.Click();
            _logger.Trace("State Drop down");
            Thread.Sleep(500);
            BrowserWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(DDLStateOptions));
            IList<IWebElement> StateOptions = Driver.FindElements(DDLStateOptions);

            //IList<IWebElement> StateOptions = Driver.FindElements(By.XPath("//*[@id='state']//div[contains(@id, 'react-select')]"));
            foreach (IWebElement State in StateOptions)
            {
                if (PracticeFormData.Rows[DtRowPtr]["State"].ToString().Equals(State.Text.ToString()))
                {

                    TestUtility.UtilityClass.ScrollToElement(State);
                    Thread.Sleep(500);
                    TestUtility.UtilityClass.JavaScriptClick(State);
                    break;
                }
            }
            _logger.Info("State selected from the drop down.");

            #endregion

            #region Select City
            //TestUtility.UtilityClass.JavaScriptClick(DDLSelectCity);
            //DDLSelectCity.Click();



            _Actions.KeyDown(Keys.Tab).KeyUp(Keys.Tab).Build().Perform();
            Thread.Sleep(500);
            _Actions.KeyDown(Keys.Space).KeyUp(Keys.Space).Build().Perform();


            _logger.Trace("City Drop down open.");
            Thread.Sleep(500);
            BrowserWait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(DDLCityOptions));
            IList<IWebElement> CityOptions = Driver.FindElements(DDLCityOptions);

            //IList<IWebElement> StateOptions = Driver.FindElements(By.XPath("//*[@id='state']//div[contains(@id, 'react-select')]"));
            foreach (IWebElement City in CityOptions)
            {
                if (PracticeFormData.Rows[DtRowPtr]["City"].ToString().Equals(City.Text.ToString()))
                {
                    TestUtility.UtilityClass.JavaScriptClick(City);
                    //City.Click();
                    break;
                }
            }
            _logger.Info("City selected from the drop down.");

            #endregion

            //Click on submie button with java script executor.
            TestUtility.UtilityClass.JavaScriptClick(BtnSubmit);
            _logger.Info("Student form submitted.");

        }

        public string GetAnyFieldValueOfSubmittedForm(string FieldName)
        {
            WebDriverWait BrowserWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(20));

             return BrowserWait.Until(ExpectedConditions.ElementIsVisible(TableFieldValue(FieldName))).Text;
        }

        #endregion

    }
}
