using AventStack.ExtentReports;
using ConsoleApp1.Base;
using ConsoleApp1.POM;
using ConsoleApp1.TestReport;
using ConsoleApp1.TestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using static ConsoleApp1.POM.ElementPage;
using static ConsoleApp1.POM.HomePage;

namespace ConsoleApp1.TestCases
{
    [TestClass]
    public class TestClass : BaseClass
    {
        #region Object Creation

        HomePage _HomePage;
        ElementPage _ElementPage;
        PracticeForm _PracticeForm;
        #endregion

        [TestMethod]
        public void Test()
        {
            try
            {
                #region Object and variable initialization.

                HomePage _HomePage = new HomePage(Driver);
                ElementPage _ElementPage = new ElementPage(Driver);

                #endregion

                #region Step: 1 Perform Operation on the Home Page.

                TestUtility.ExcelUtility.PopulateInCollection(rootpath + "//TestData//Sample Test data.xlsx", "Patel");
                _HomePage.ClickOnElements(Driver);
                UtilityClass.WaitForAjaxLoad();

                #endregion

                #region Step:2 Click on Left pane elements Checkbox.
                
                _ElementPage.ClickOnLeftPaneElement(Driver, TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Elements), TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.CheckBox));

                #endregion

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message); 
                Console.WriteLine(ex);
            }

        }

        [TestMethod]
        public void SelectValueFromDDL()
        {
            try
            {
                #region Object and variable Initialization

                _HomePage = new HomePage(Driver);
                _ElementPage = new ElementPage(Driver);
                TestUtility.ExcelUtility.PopulateInCollection(rootpath + "//TestData//Sample Test data.xlsx", "Patel");

                #endregion

                #region Step:1 Navigates to Elements page

                _HomePage.ClickOnElements(Driver);
                _ElementPage.ClickOnLeftPaneElement(Driver, TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Widgets), TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.SelectMenu));

                #endregion

                #region Step:2  Select values fromt the Drop down.

                _ElementPage.SelectValueFromDroDown();

                #endregion

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
                Console.WriteLine(ex);
            }

        }


        [TestMethod]
        public void VerifyUploadFunctionality()
        {
            try
            {
                #region Object and variable Initialization

                HomePage _HomePage = new HomePage(Driver);
                ElementPage _ElementPage = new ElementPage(Driver);
                TestUtility.ExcelUtility.PopulateInCollection(rootpath + "//TestData//Sample Test data.xlsx", "Patel");
                string filepath = "C:\\Users\\Vijay Patel\\Documents\\NewUploadAutoIT.au3";

                #endregion

                #region Step:1 Navigates to Elements page

                _HomePage.ClickOnElements(Driver);
                UtilityClass.WaitForAjaxLoad();

                #endregion

                #region Step:2 Upload file.

                _ElementPage.ClickOnLeftPaneElement(Driver, TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Elements), TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.UploadAndDownload));
                _ElementPage.UploadFile(filepath);
                Assert.AreEqual(@"C:\fakepath\NewUploadAutoIT.au3", _ElementPage.MethodUploadedFilePath());

                #endregion


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine(ex);
                Assert.Fail(ex.Message);
                
            }
        }

        [TestMethod]
        public virtual void VerifyFormFunctionality()
        {
            try
            {

                #region Object and variable Initialization

                _HomePage = new HomePage(Driver);
                _ElementPage = new ElementPage(Driver);
                _PracticeForm = new PracticeForm(Driver); ;
                DataTable FormData = TestUtility.ExcelUtility.ConvertExcelToDataTable(rootpath + "//TestData//Sample Test data.xlsx", "FormData");
                string UploadFilePath = Path.Combine(rootpath, "FilesToUpload");

                #endregion

                #region Step:1 Navigates to Elements page Verify Form Functionality.

                _HomePage.ClickOnElements(Driver);
                UtilityClass.WaitForAjaxLoad();
                _ElementPage.ClickOnLeftPaneElement(Driver, TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Forms), TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.PracticeForm));
                _PracticeForm.FillPracticeForm(0, FormData, UploadFilePath);
                Assert.AreEqual(string.Concat(FormData.Rows[0]["FirstName"].ToString(), ' ', FormData.Rows[0]["LastName"].ToString()), _PracticeForm.GetAnyFieldValueOfSubmittedForm("Student Name"), "Validation failed for Field value.");
                string[] FetchedSubject = _PracticeForm.GetAnyFieldValueOfSubmittedForm("Subjects").Split(',');
                string[] UpdatedSubject = new string[FetchedSubject.Length];

                for (int i = 0; i < FetchedSubject.Length; i++)
                {
                    UpdatedSubject[i] = FetchedSubject[i].TrimStart().ToString().TrimEnd();
                }


                CollectionAssert.AreEqual(FormData.Rows[0]["Subject"].ToString().Split(';'), UpdatedSubject, "Validation failed for Field value.");


                #endregion

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public virtual void VerifyDeleteRecordFunctionality()
        {
            try
            {
                #region Object and variable Initialization

                _HomePage = new HomePage(Driver);
                _ElementPage = new ElementPage(Driver);
                _PracticeForm = new PracticeForm(Driver);
                string RecordNameTODelete = "Vega";

                #endregion

                #region Step:1 Navigates to Elements page and verify Delete recod functionality.

                _HomePage.ClickOnElements(Driver);
                UtilityClass.WaitForAjaxLoad();
                _ElementPage.ClickOnLeftPaneElement(Driver, TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Elements), TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.WebTables));
                _ElementPage.DeleteRecordFromGrid(RecordNameTODelete);
                Assert.IsFalse(_ElementPage.VerifyDeletedRecordsInGrid(RecordNameTODelete));                

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Assert.Fail(ex.Message);
            }
        }
    }


}
