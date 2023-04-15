using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Configuration;
using System.IO;
using OpenQA.Selenium.Support.UI;
using ConsoleApp1.TestReport;
using NLog;

namespace ConsoleApp1.Base
{

    public class BaseClass
    {
        #region Variables

        public static IWebDriver _Driver;
        public static string _Browser = ConfigurationManager.AppSettings["Browser"].ToUpper();
        public static string rootpath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
        public WebDriverWait _browserWait;
        public static string ReportPath = Path.Combine(Path.GetDirectoryName(Directory.GetParent(Directory.GetParent(rootpath).ToString()).ToString()), "ReportAndScreenShot-" + DateTime.Now.ToString("yyyy-MM-dd"));
        public static string ScreenSortPath = Path.Combine(ReportPath, "Screenshot");
        
        //private static TestContext _testContext;
        private  Logger _logger = LogManager.GetCurrentClassLogger();

        public TestContext TestContext { get; set; }
        private ScreenshotTaker ScreenshotTaker { get; set; }

        private Reporter Reporter { get; set; }

        public WebDriverWait BrowserWait
        {
            get
            {
                if (_browserWait == null || Driver == null)
                {
                    throw new NullReferenceException("The WebDriver browser wait instance was not initialized. You should first call the method Start.");
                }
                return _browserWait;
            }
            set
            {
                _browserWait = value;
            }
        }

        #region Enum
        public enum EnumBrowser
        {
            CHROME,
            CHROME_HEADLESS,
            FIREFOX,
            FIREFOX_HEADLESS
        }
      

        #endregion

        #endregion

        #region DriverOptions
        private DriverOptions GetBrowserOptions()
        {
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsBrowserOptionEnable"]).Equals(true))
            {
                if (_Browser.Equals(EnumBrowser.CHROME.ToString()))
                {
                    ChromeOptions option = new ChromeOptions();
                    option.AddArgument("start-maximized");
                    option.AcceptInsecureCertificates = true;
                    option.AddUserProfilePreference("disable-popup-blocking", "true");
                    return option;
                }
                else if (_Browser.Equals(EnumBrowser.FIREFOX.ToString()))
                {
                    FirefoxOptions option = new FirefoxOptions();
                    option.AddArgument("start-maximized");
                    option.AcceptInsecureCertificates = true;
                    option.SetPreference("disable-popup-blocking", "true");
                    return option;
                }
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Initialization        

        public static IWebDriver Driver
        {
            get
            {
                if (_Driver == null)
                {
                    throw new NullReferenceException("The WebDriver browser instance was not initialized. You should first call the method Start.");
                }
                return _Driver;
            }
            set
            {
                _Driver = value;
            }
        }

        [TestInitialize]
        public void InitializeTest()
        {

            _logger.Trace("******************Initializing Test******************");
            Reporter.AddTestCaseMetadataToHtmlReport(TestContext);          
            InitializeDriver(GetBrowserOptions());
            ScreenshotTaker = new ScreenshotTaker(Driver, TestContext);
            _logger.Info("Test initialization done successfully. Test Name - " + TestContext.TestName);
        }
        public void InitializeDriver(object browserOptions = null)
        {            

            switch (_Browser)
            {

                case "CHROME":

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsBrowserOptionEnable"]).Equals(true))
                    {
                        Driver = new ChromeDriver(rootpath + "\\Driver\\chromedriver.exe", (ChromeOptions)browserOptions);

                    }
                    else
                    {
                        Driver = new ChromeDriver(rootpath + "\\Driver\\chromedriver.exe");
                        Driver.Manage().Window.Maximize();

                    }

                    break;

                case "FIREFOX":

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsBrowserOptionEnable"]).Equals(true))
                    {
                        Driver = new FirefoxDriver(rootpath + "\\Driver\\geckodriver.exe", (FirefoxOptions)browserOptions);

                    }
                    else
                    {
                        Driver = new FirefoxDriver();
                        Driver.Manage().Window.Maximize();

                    }
                    break;

                default:
                    break;
            }
            _logger.Info(_Browser + " Is initialized successfully " + "With Browser option = " + Convert.ToBoolean(ConfigurationManager.AppSettings["IsBrowserOptionEnable"]) );
            Driver.Url = ConfigurationManager.AppSettings["URL"];
            TestUtility.UtilityClass.WaitForBrowserLoad();
        }

        #endregion

        #region Test Cleanup

        [TestCleanup]
        public void TearDown()
        {
            _logger.Trace("Driver clean up is started.");
            ScreenshotTaker.CreateScreenshotIfTestFailed();
            Reporter.ReportTestOutcome(ScreenshotTaker.ScreenSortPathWithFileName);
            Driver.Quit();
            _logger.Info("******************Driver clean up performed successfully.******************");
            
        }

        #endregion

    }
}
