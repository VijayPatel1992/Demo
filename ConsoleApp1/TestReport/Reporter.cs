using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using ConsoleApp1.Base;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.TestReport
{
    public  class Reporter
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static ExtentReports ReportManager { get; set; }
        public static string HtmlReportFullPath { get; set; }
        public static string LatestResultsReportFolder { get; set; }
        private static TestContext MyTestContext { get; set; }
        private static ExtentTest CurrentTestCase { get; set; }


        public static void StartReporter()
        {
            _logger.Trace("Starting a one time setup for the entire" +
                            " .CreatingReports namespace." +
                            "Going to initialize the reporter next...");
            CreateReportDirectory();
            
            var htmlReporter = new ExtentHtmlReporter(HtmlReportFullPath);
            ReportManager = new ExtentReports();
            htmlReporter.LoadConfig(BaseClass.rootpath + "\\extent-config.xml");
            ReportManager.AttachReporter(htmlReporter);
            ReportManager.AddSystemInfo("Environment","QA");
            ReportManager.AddSystemInfo("User Name", "Vija Patel");            
           
        }
        public static string CreateReportDirectory()
        {
            DirectoryInfo ReportDirectory = new System.IO.DirectoryInfo(BaseClass.ScreenSortPath);

            if (!ReportDirectory.Exists)
                Directory.CreateDirectory(BaseClass.ReportPath);
                Directory.CreateDirectory(BaseClass.ScreenSortPath);
                Directory.CreateDirectory(BaseClass.CreatedExcelFilePath);

            HtmlReportFullPath = $"{BaseClass.ReportPath}\\TestResults.html";
            _logger.Trace("Full path of HTML report=>" + HtmlReportFullPath);
            return HtmlReportFullPath;
        }

        public static void AddTestCaseMetadataToHtmlReport(TestContext testContext)
        {
            Reporter.StartReporter();
            MyTestContext = testContext;
            CurrentTestCase = ReportManager.CreateTest(MyTestContext.TestName);
        }

        public static void LogPassingTestStepToBugLogger(string message)
        {
            _logger.Info(message);
            CurrentTestCase.Log(Status.Pass, message);
        }

        public static void ReportTestOutcome(string screenshotPath)
        {
            var status = MyTestContext.CurrentTestOutcome;

            switch (status)
            {
                case UnitTestOutcome.Failed:
                    _logger.Error($"Test Failed=>{MyTestContext.TestName}");
                    CurrentTestCase.AddScreenCaptureFromPath(screenshotPath);
                    CurrentTestCase.Fail("Fail");
                    break;
                case UnitTestOutcome.Inconclusive:
                    CurrentTestCase.AddScreenCaptureFromPath(screenshotPath);
                    CurrentTestCase.Warning("Inconclusive");
                    break;
                case UnitTestOutcome.Unknown:
                    CurrentTestCase.Skip("Test skipped");
                    break;
                default:
                    _logger.Info("Test Pass successfully. Test Name - " +MyTestContext.TestName);
                    CurrentTestCase.Pass("Pass");
                    break;
            }

            ReportManager.Flush();
        }

        public static void LogTestStepForBugLogger(Status status, string message)
        {
            _logger.Info(message);
            CurrentTestCase.Log(status, message);
        }
    }
}
