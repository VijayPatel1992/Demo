using AventStack.ExtentReports;
using ConsoleApp1.Base;
using ConsoleApp1.POM;
using ConsoleApp1.TestReport;
using ConsoleApp1.TestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using static ConsoleApp1.POM.ElementPage;

namespace ConsoleApp1.TestCases
{
    [TestClass]
    public class TestClass : BaseClass
    {
        #region Object Creation

        HomePage _HomePage;
        ElementPage _ElementPage;

        #endregion

        [TestMethod]
        public void Test()
        {
            try
            {
                #region Object and variable initialization.

                HomePage _HomePage = new HomePage(Driver);
                ElementPage elementPage = new ElementPage(Driver);

                #endregion

                #region Step: 1 Perform Operation on the Home Page.

                TestUtility.ExcelUtility.PopulateInCollection(rootpath + "//TestData//Sample Test data.xlsx", "Patel");
                _HomePage.ClickOnElements(Driver);
                UtilityClass.WaitForAjaxLoad();

                #endregion

                #region Step:2 Click on Left pane elements Checkbox.

                elementPage.ClickOnLeftPaneElement(Driver, TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Elements), TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.CheckBox));

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
    }


}
