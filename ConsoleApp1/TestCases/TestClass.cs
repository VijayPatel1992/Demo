using ConsoleApp1.Base;
using ConsoleApp1.POM;
using ConsoleApp1.TestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static ConsoleApp1.POM.ElementPage;

namespace ConsoleApp1.TestCases
{
    [TestClass]
    public class TestClass : BaseClass
    {


        [TestMethod]
        public void Test()
        {
            try
            {
                HomePage _HomePage = new HomePage(Driver);
                ElementPage elementPage = new ElementPage(Driver);
                TestUtility.ExcelUtility.PopulateInCollection(rootpath + "//TestData//Sample Test data.xlsx", "Patel");
                _HomePage.ClickOnElements(Driver);
                UtilityClass.WaitForAjaxLoad();
                elementPage.ClickOnLeftPaneElement(Driver, TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Elements), TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.CheckBox));
                TestUtility.UtilityClass.TakeScreenShot(MethodBase.GetCurrentMethod().Name, Driver);                
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
                HomePage _HomePage = new HomePage(Driver);
                ElementPage elementPage = new ElementPage(Driver);
                TestUtility.ExcelUtility.PopulateInCollection(rootpath + "//TestData//Sample Test data.xlsx", "Patel");
                _HomePage.ClickOnElements(Driver);
                elementPage.ClickOnLeftPaneElement(Driver, TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Widgets), TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.SelectMenu));
                elementPage.SelectValueFromDroDown();
                TestUtility.UtilityClass.TakeScreenShot(MethodBase.GetCurrentMethod().Name, Driver);
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
                HomePage _HomePage = new HomePage(Driver);
                ElementPage _ElementPage = new ElementPage(Driver);
                TestUtility.ExcelUtility.PopulateInCollection(rootpath + "//TestData//Sample Test data.xlsx", "Patel");
                string filepath = "C:\\Users\\Vijay Patel\\Documents\\NewUploadAutoIT.au3";
                _HomePage.ClickOnElements(Driver);
                UtilityClass.WaitForAjaxLoad();

                _ElementPage.ClickOnLeftPaneElement(Driver, TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneGroupHeader.Elements), TestUtility.UtilityClass.GetDescriptionFromEnum(EnumLeftPaneElementList.UploadAndDownload));
                _ElementPage.UploadFile(filepath);

                TestUtility.UtilityClass.TakeScreenShot(MethodBase.GetCurrentMethod().Name, Driver);

                Assert.AreEqual(@"C:\fakepath\NewUploadAutoIT.au3", _ElementPage.MethodUploadedFilePath()); 

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
