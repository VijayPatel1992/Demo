using ConsoleApp1.Base;
using ConsoleApp1.POM;
using ConsoleApp1.TestUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.TestCases
{
    [TestClass]
    public class TestClass:BaseClass
    {

        [TestMethod]
        public void Test()
        {

            try
            {
                HomePage _HomePage = new HomePage(Driver);
                UtilityClass utilityClass = new UtilityClass();
                _HomePage.ClickOnElements(Driver);
                utilityClass.TakeScreenShot(MethodBase.GetCurrentMethod().Name, Driver);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

    }
}
