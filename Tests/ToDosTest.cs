using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics.Metrics;
using TodoMVC.Pages;

namespace TodoMVC.Tests;

[TestFixture(TestName = "ToDo App")]
public class ToDosTest : IDisposable
{
    private WebDriver _driver;
    private ToDoPage _toDoPage;
    private string Url;
    private ChromeOptions chromeOptions;

    private ExtentReports _extent;
    private ExtentTest _test;

    private string _reportFolderPath;
    private string _screenshotFolderPath;

    public ToDosTest()
    {
        Url = TestContext.Parameters["webAppUrl"] ?? string.Empty;

        // Initializing the chromeoption.
        chromeOptions = new ChromeOptions();

        //Iinitializing the driver and maximize the window
        _driver = new ChromeDriver(chromeOptions);
        _driver.Manage().Window.Maximize();

        //Initializing the pages
        _toDoPage = new ToDoPage(_driver);

        // Create a report folder if it doesn't exist
        _reportFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\TestResults");
        if (!Directory.Exists(_reportFolderPath))
        {
            Directory.CreateDirectory(_reportFolderPath);
        }

        // Create a screenshots folder if it doesn't exist
        _screenshotFolderPath = Path.Combine(_reportFolderPath, "..\\..\\TestResults\\Screenshots");
        if (!Directory.Exists(_screenshotFolderPath))
        {
            Directory.CreateDirectory(_screenshotFolderPath);
        }

        var htmlReporter = new ExtentSparkReporter(Path.Combine("..\\..\\TestResults", "extent-report.html"));
        _extent = new ExtentReports();
        _extent.AttachReporter(htmlReporter);

        _test = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
    }


    [Test, Order(1)]
    public void NavigateToThePage()
    {

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
    public void AddNewToDos()
    {

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
            var result = _toDoPage.GetAvailableToDoListInSelectedTab();

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
            var result = _toDoPage.ToggleToDos(toDoListToToggle);

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
            var result = _toDoPage.GetAvailableToDoListInSelectedTab();

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
        // Arrange.
        var selectedToDo = "Pay Bills";
        var newToDoValue = "Pay Utility Bills";

        try
        {
            // Act.
            _toDoPage.ClickAllTab();
            _toDoPage.EditSelectedToDo(selectedToDo, newToDoValue);
            var result = _toDoPage.GetAvailableToDoListInSelectedTab();

            // Assert. 
            Assert.That(result.Count(x => x == newToDoValue), Is.EqualTo(1));
            _test.Log(Status.Pass, "Successfully Edited.");
            _test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(CaptureScreenshot()).Build());

        }
        catch (AssertionException ex)
        {
            _test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }

    }

    [Test, Order(6)]
    public void VerifyEnterEmptyValue()
    {
        // Arrange.
        var listWithEmptyValue = new List<string>
        {
            string.Empty,
        };

        try
        {
            //Act.

            //Get the count of ToDos in the list before adding empty value.
            var beforeCount = _toDoPage.GetAvailableToDoListInSelectedTab().Count();
            _toDoPage.AddNewToDos(listWithEmptyValue);

            //Get the count of ToDos in the list After adding empty value.
            var afterCount = _toDoPage.GetAvailableToDoListInSelectedTab().Count();

            //Assert.
            Assert.That(beforeCount, Is.EqualTo(afterCount));
            _test.Log(Status.Pass, "Empty value is not added to the list");
            _test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(CaptureScreenshot()).Build());
        }
        catch (AssertionException ex)
        {
            _test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }

    }

    [Test, Order(7)]
    public void VerifyChangeCompletedToActiveToDos()
    {
        try
        {
            //Act
            _toDoPage.ClickCompletedTab();
            _toDoPage.ChangeToDoStatusInSelectedTab();
            var result = _toDoPage.GetAvailableToDoListInSelectedTab();

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
            _test.Log(Status.Pass, "No Completed ToDos in the completed tab");
            _test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(CaptureScreenshot()).Build());

        }
        catch (AssertionException ex)
        {
            _test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }
    }

    [Test, Order(8)]
    public void VerifyClearCompletedButtonIfClearActiveToDos()
    {
        try
        {   // Act.

            // Get the count of ToDos in the list before click clear completed button.
            var beforeCount = _toDoPage.GetAvailableToDoListInSelectedTab().Count();
            _toDoPage.CickClearCompleted();

            // Get the count of ToDos in the list After click clear completed button.
            var afterCount = _toDoPage.GetAvailableToDoListInSelectedTab().Count();
            _toDoPage.ClickAllTab();

            // Assert
            Assert.That(beforeCount, Is.EqualTo(afterCount));
            _test.Log(Status.Pass, "No completed Items to clear and Active ToDos are not cleared.");
            _test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(CaptureScreenshot()).Build());

        }
        catch (AssertionException ex)
        {
            _test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }
    }

    [Test, Order(9)]
    public void VerifyEditFuctionWithEmptyValue()
    {
        // Arrange.
        var selectedToDo = "Pay Utility Bills";
        var newToDoValue = " ";

        try
        {
            // Act.
            _toDoPage.ClickAllTab();
            _toDoPage.EditSelectedToDo(selectedToDo, newToDoValue);
            _toDoPage.ClickAllTab();
            var result = _toDoPage.GetAvailableToDoListInSelectedTab();
            

            // Assert. 
            Assert.That(result.Count(x => x == selectedToDo), Is.EqualTo(1));
            _test.Log(Status.Pass, "ToDo is not updated with an empty value");
            _test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(CaptureScreenshot()).Build());

        }
        catch (AssertionException ex)
        {
            _test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }

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
        // Set screenshot file name with timestamp.

        string screenshotFileName = $"screenshot_{DateTime.Now:yyyyMMddHHmmssfff}.png";
        string screenshotPath = Path.Combine(_screenshotFolderPath, screenshotFileName);

        // Capture screenshot
        Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
        screenshot.SaveAsFile(screenshotPath);

        return screenshotPath;
    }
}
