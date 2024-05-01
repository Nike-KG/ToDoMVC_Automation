using AventStack.ExtentReports;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium;
using AventStack.ExtentReports.Reporter;
using TodoMVC.Utilities;

namespace TodoMVC.Tests
{
    public class BaseTests : IDisposable
    {
        private ChromeOptions chromeOptions;
        private FirefoxOptions firefoxOptions;
        private EdgeOptions edgeOptions;
        private ExtentReports _extent;        

        private string _browser;
        private string _reportFolderPath;

        public WebDriver Driver { get; private set; }
        public string BaseUrl { get; private set; }
        public string ScreenshotFolderPath { get; private set; }

        public ExtentTest Test { get; private set; }

        public BaseTests()
        {
            SetBrowserOptions();
            SetBrowserWindowSettings();
            SetTestBaseUrl();
            SetTestReportSettings();

            _reportFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\TestResults");
            ScreenshotFolderPath = Path.Combine(_reportFolderPath, "..\\..\\TestResults\\Screenshots");

            CreateFolderPaths();
        }

        private void SetBrowserOptions()
        {
            _browser = TestContext.Parameters["browser"] ?? string.Empty;

            switch (_browser.ToLower())
            {
                case "chrome":
                    chromeOptions = new ChromeOptions();
                    Driver = new ChromeDriver(chromeOptions);
                    break;
                case "firefox":
                    firefoxOptions = new FirefoxOptions();
                    Driver = new FirefoxDriver(firefoxOptions);
                    break;
                case "edge":
                    edgeOptions = new EdgeOptions();
                    edgeOptions.AddArgument("InPrivate");
                    Driver = new EdgeDriver(edgeOptions);
                    break;
                default:
                    chromeOptions = new ChromeOptions();
                    Driver = new ChromeDriver(chromeOptions);
                    break;
            }
        }

        private void SetBrowserWindowSettings()
        {
            Driver.Manage().Window.Maximize();
        }

        private void SetTestBaseUrl() 
        { 
            BaseUrl = TestContext.Parameters["webAppUrl"] ?? string.Empty; 
        }

        private void SetTestReportSettings()
        {
            var htmlReporter = new ExtentSparkReporter(Path.Combine("..\\..\\TestResults", "extent-report.html"));
            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);

            Test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        private void CreateFolderPaths()
        {
            TestReportHelper.CreateScreenshotFolderPath(_reportFolderPath);
            TestReportHelper.CreateScreenshotFolderPath(ScreenshotFolderPath);
        }

        public void Dispose()
        {
            Test.Log(Status.Info, "Test completed");
            _extent.Flush();
            Driver.Dispose();
        }
    }
}
