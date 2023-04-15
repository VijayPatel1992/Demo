using System;
using System.IO;
using ConsoleApp1.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using OpenQA.Selenium;

namespace ConsoleApp1.TestReport
{
    public class ScreenshotTaker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IWebDriver _driver;
        private readonly TestContext _testContext;
        public static string ScreenSortPathWithFileName { get; set; }
        private string ScreenshotFileName { get; set; }

        public ScreenshotTaker(IWebDriver driver, TestContext testContext)
        {
            if (driver == null)
                return;
            _driver = driver;
            _testContext = testContext;
            ScreenshotFileName = _testContext.TestName;
        }


        public void CreateScreenshotIfTestFailed()
        {
            if (_testContext.CurrentTestOutcome == UnitTestOutcome.Failed ||
                _testContext.CurrentTestOutcome == UnitTestOutcome.Inconclusive)
                TakeScreenshotForFailure();
        }

        public string TakeScreenshot(string screenshotFileName)
        {
            var ss = GetScreenshot();
            var successfullySaved = TryToSaveScreenshot(screenshotFileName, ss);

            return successfullySaved ? BaseClass.ScreenSortPath : "";
        }

        public bool TakeScreenshotForFailure()
        {
            ScreenshotFileName = $"FAIL_{ScreenshotFileName}";

            var ss = GetScreenshot();
            var successfullySaved = TryToSaveScreenshot(ScreenshotFileName, ss);
            if (successfullySaved)
                Logger.Error($"Screenshot Of Error=>{BaseClass.ScreenSortPath}");
            return successfullySaved;
        }

        private Screenshot GetScreenshot()
        {
            return ((ITakesScreenshot)_driver)?.GetScreenshot();
        }

        private bool TryToSaveScreenshot(string screenshotFileName, Screenshot ss)
        {
            try
            {
                SaveScreenshot(screenshotFileName, ss);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e.InnerException);
                Logger.Error(e.Message);
                Logger.Error(e.StackTrace);
                return false;
            }
        }

        private void SaveScreenshot(string ScreenshotName, Screenshot ss)
        {
            if (ss == null)
                return;

            ScreenshotName = DateTime.Now.ToString("yyyy_MM_dd-HHmmss") + " "+ ScreenshotName;
            ScreenSortPathWithFileName  = $"{BaseClass.ScreenSortPath}\\{ScreenshotName}.Jpeg";
            ScreenSortPathWithFileName = ScreenSortPathWithFileName.Replace('/', ' ').Replace('"', ' ');
            ss.SaveAsFile(ScreenSortPathWithFileName, ScreenshotImageFormat.Jpeg);
        }



        /// <summary>
        /// Captures Screenshot wand has specified filename 
        /// </summary>
        /// <param name="filename"> Screenshot FileName </param>
        /// <returns>Returns the FielName</returns>
        public static string TakeScreenShot(string filename, IWebDriver Driver)
        {
            
            ScreenSortPathWithFileName = Path.Combine(BaseClass.ScreenSortPath, filename + " " + DateTime.Now.ToString("yyyy_MM_dd-HHmmss") + ".jpeg");

            ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(filename, ScreenshotImageFormat.Jpeg);

            return ScreenSortPathWithFileName;
        }
    }
}
