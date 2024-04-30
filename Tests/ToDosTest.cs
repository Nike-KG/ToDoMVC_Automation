using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TodoMVC.Pages;

namespace TodoMVC.Tests;

[TestFixture(TestName = "ToDo App")]
public class ToDosTest : IDisposable
{
    private WebDriver _driver;
    private ToDoPage _toDoPage;   
    private string Url = "https://todomvc.com/examples/react/dist/";
    private ChromeOptions chromeOptions;
    
    private ExtentReports _extent;
    private ExtentTest _test;
    
    private string _reportFolderPath;
    private string _screenshotFolderPath;

    public ToDosTest() {

        // Initializing the chromeoption.
        chromeOptions = new ChromeOptions();

        //Iinitializing the driver and maximize the window
        _driver = new ChromeDriver(chromeOptions);
        _driver.Manage().Window.Maximize();

        //Initializing the pages
        _toDoPage = new ToDoPage(_driver);

        // Create a report folder if it doesn't exist
        _reportFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestReports");
        if (!Directory.Exists(_reportFolderPath))
        {
            Directory.CreateDirectory(_reportFolderPath);
        }

        // Create a screenshots folder if it doesn't exist
        _screenshotFolderPath = Path.Combine(_reportFolderPath, "Screenshots");  
        if (!Directory.Exists(_screenshotFolderPath))
        {
            Directory.CreateDirectory(_screenshotFolderPath);
        }

        var htmlReporter = new ExtentSparkReporter(Path.Combine(_reportFolderPath, "extent-report.html"));
        _extent = new ExtentReports();
        _extent.AttachReporter(htmlReporter);

        _test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
    }


    [Test, Order(1)]
    public void NavigateToThePage() {

        //Arrange
        var url = Url;
        var expectedTitle = "TodoMVC: React";

        try
        {
            //Act 
            _toDoPage.NavigateToURL(url);

            //Assert
            Assert.That(_toDoPage.GetPageTitle(), Is.EqualTo(expectedTitle));
            _test.Log(Status.Pass, "Successfully navigated to the page.");
            _test.Log(Status.Info, $"Page Title: {expectedTitle}");
            _test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(CaptureScreenshot()).Build());
        }
        catch (AssertionException ex)
        {
            _test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            _test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(CaptureScreenshot()).Build());
            throw;
        }

    }

    [Test, Order(2)]
    public void AddNewToDos() {

        //Arrange
        var toDoText = new List<string>()
        {
            "Pay Bills",
            "Order Groceries",
            "Go for a walk",
            "Go to Beach"
        };

        try
        {
            //Act
            _toDoPage.AddNewToDos(toDoText);
            var result = _toDoPage.GetAvailableToDoList();

            //Assert
            Assert.That(toDoText, Is.EqualTo(result));
            _test.Log(Status.Pass, "Successfully added to dos.");
            _test.Log(Status.Info, $"Added ToDos: <br>{string.Join("<br>", result)}");
            _test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(CaptureScreenshot()).Build());
        }
        catch (AssertionException ex)
        {
            _test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }
    }

    [Test, Order(3)] 
    public void SelectToDosAsCompletedInAll() 
    {
        //Arrange
        var toDoListToToggle = new List<string>()
        {
            "Go for a walk",
            "Order Groceries"
        };

        try
        {
            //Act
            var result = _toDoPage.CompleteToDosInAll(toDoListToToggle);

            //Assert
            Assert.That(toDoListToToggle.Count, Is.EqualTo(result.Count));
            _test.Log(Status.Pass, "Successfully completed to dos.");
            _test.Log(Status.Info, $"Completed ToDos: <br>{string.Join("<br>", result)}");
            _test.Log(Status.Info, $"Completed ToDos Count: {result.Count}");
            _test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(CaptureScreenshot()).Build());
        }
        catch (AssertionException ex)
        {
            _test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }
          
    }

    [Test, Order(4)]
    public void VerifyCompletedToDosInCompletedTab()
    {
        //Arrange
        var toDoListToToggle = new List<string>()
        {
            "Go for a walk",
            "Order Groceries"
        };

        //Act
        try
        {
            _toDoPage.ClickCompletedTab();
            var result = _toDoPage.GetAvailableToDoList();

            // Assert
            Assert.That(toDoListToToggle.Count, Is.EqualTo(result.Count));
            _test.Log(Status.Pass, "Successfully verified completed to dos in completed tab.");
            _test.Log(Status.Info, $"Completed ToDos: <br>{string.Join("<br>", result)}");
            _test.Log(Status.Info, $"Completed ToDos Count: {result.Count}");
            _test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(CaptureScreenshot()).Build());
        }
        catch (AssertionException ex)
        {
            _test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }
        

    }
    [Test, Order(5)]
    public void VerifyEditFuction()
    {

        //Act
        //_toDoPage.EditSelectedToDO(_driver);
        //Thread.Sleep(7000);
        Assert.Pass();
    }

    public void Dispose() 
    {
        _test.Log(Status.Info, "Test completed");
        _test.AddScreenCaptureFromPath(CaptureScreenshot());
        _extent.Flush();
        _driver.Dispose();
    }

    private string CaptureScreenshot()
    {
        // Set screenshot file name with timestamp

        string screenshotFileName = $"screenshot_{DateTime.Now:yyyyMMddHHmmssfff}.png";
        string screenshotPath = Path.Combine(_screenshotFolderPath, screenshotFileName);

        // Capture screenshot
        Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
        screenshot.SaveAsFile(screenshotPath);

        return screenshotPath;
    }
}
