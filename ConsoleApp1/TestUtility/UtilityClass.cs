using ConsoleApp1.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.TestUtility
{
    public class UtilityClass : BaseClass
    {       
        #region Utilities

        /// <summary>
        /// Captures Screenshot wand has specified filename 
        /// </summary>
        /// <param name="filename"> Screenshot FileName </param>
        /// <returns>Returns the FielName</returns>
        public string TakeScreenShot(string filename, IWebDriver Driver)
        {
            rootpath = Directory.GetParent(rootpath).FullName;
            string pathString = System.IO.Path.Combine(rootpath, "ScreenShots");
            
            System.IO.DirectoryInfo ScreenShotdir = new System.IO.DirectoryInfo(pathString);

            if (!ScreenShotdir.Exists)
                System.IO.Directory.CreateDirectory(pathString);

            filename = filename + " " + DateTime.UtcNow.ToString("yyyy-MM-dd-mm-ss") + ".jpeg";
            filename = Path.Combine(pathString, filename);

            ((ITakesScreenshot)Driver).GetScreenshot().SaveAsFile(filename, ScreenshotImageFormat.Jpeg);

            return filename;
        }

        #endregion


        private static WebDriverWait _browserWait;
        public static WebDriverWait BrowserWait
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
    }
}
