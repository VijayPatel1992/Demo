using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Base
{
    public class BaseClass
    {

        #region Variables

        public static IWebDriver _Driver;
        public static string _Browser = ConfigurationManager.AppSettings["Browser"].ToUpper();
        public static string rootpath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

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
                private static DriverOptions GetBrowserOptions()
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

            InitializeDriver(GetBrowserOptions());
        }

        public static void InitializeDriver(object browserOptions = null)
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
                        //Driver = new FirefoxDriver("C:\\Users\\Vijay Patel\\Downloads\\geckodriver-v0.32.0-win-aarch64\\geckodriver.exe");
                        Driver.Manage().Window.Maximize();

                    }
                    break;

                default:
                    break;
            }

            Driver.Url = ConfigurationManager.AppSettings["URL"];

        }

        #endregion

        #region Utilities

        /// <summary>
        /// Captures Screenshot wand has specified filename 
        /// </summary>
        /// <param name="filename"> Screenshot FileName </param>
        /// <returns>Returns the FielName</returns>
        //public string TakeScreenShot(string filename, IWebDriver Driver)
        //{
        //    string pathString = System.IO.Path.Combine(rootpath, "ScreenShots");
        //    System.IO.DirectoryInfo ScreenShotdir = new System.IO.DirectoryInfo(pathString);

        //    if (!ScreenShotdir.Exists)
        //        System.IO.Directory.CreateDirectory(pathString);

        //    filename = filename + " " + DateTime.UtcNow.ToString("yyyy-MM-dd-mm-ss") + ".jpeg";
        //    filename = Path.Combine(pathString, filename);

        //    ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(filename, ScreenshotImageFormat.Jpeg);

        //    return filename;
        //}

        #endregion

        #region Test Cleanup
        [TestCleanup]
        public void TearDown()
        {
            Driver.Quit();
        }

        #endregion

    }
}
