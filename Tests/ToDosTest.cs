using AventStack.ExtentReports;
using TodoMVC.Pages;
using TodoMVC.Utilities;

namespace TodoMVC.Tests;

[TestFixture(TestName = "ToDo App")]
public class ToDosTest : BaseTests
{
    private ToDoPage _toDoPage;

    public ToDosTest()
    {
        //Initializing the pages
        _toDoPage = new ToDoPage(Driver);
    }

    /// <summary>
    /// Navigate to the ToDo web page.
    /// </summary>
    [Test, Order(1)]
    public void NavigateToThePage()
    {
        //Arrange
        var url = BaseUrl;
        var expectedTitle = "TodoMVC: React";

        try
        {
            //Act 
            _toDoPage.NavigateToURL(url);

            //Assert
            Assert.That(_toDoPage.GetPageTitle(), Is.EqualTo(expectedTitle));
            Test.Log(Status.Pass, "Successfully navigated to the page.");
            Test.Log(Status.Info, $"Page Title: {expectedTitle}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
        }
        catch (AssertionException ex)
        {
            Test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
            throw;
        }
        catch (Exception ex)
        {
            Test.Log(Status.Fail, $"Error occurred: {ex.Message}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
            throw;
        }

    }

    /// <summary>
    /// Add new ToDos to the system.
    /// </summary>
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
            Test.Log(Status.Pass, "Successfully added to dos.");
            Test.Log(Status.Info, $"Added ToDos: <br>{string.Join("<br>", result)}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
        }
        catch (AssertionException ex)
        {
            Test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Test.Log(Status.Fail, $"Error occurred: {ex.Message}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
            throw;
        }
    }

    /// <summary>
    /// Change ToDos status of a selected ToDos to "Completed".
    /// </summary>
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
            Test.Log(Status.Pass, "Successfully completed to dos.");
            Test.Log(Status.Info, $"Completed ToDos: <br>{string.Join("<br>", result)}");
            Test.Log(Status.Info, $"Completed ToDos Count: {result.Count}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
        }
        catch (AssertionException ex)
        {
            Test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Test.Log(Status.Fail, $"Error occurred: {ex.Message}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
            throw;
        }

    }

    /// <summary>
    /// Check completed ToDos in Completed tab. 
    /// </summary>
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
            Test.Log(Status.Pass, "Successfully verified completed to dos in completed tab.");
            Test.Log(Status.Info, $"Completed ToDos: <br>{string.Join("<br>", result)}");
            Test.Log(Status.Info, $"Completed ToDos Count: {result.Count}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
        }
        catch (AssertionException ex)
        {
            Test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Test.Log(Status.Fail, $"Error occurred: {ex.Message}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
            throw;
        }


    }

    /// <summary>
    /// Verify Edit function.
    /// </summary>
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
            Test.Log(Status.Pass, "Successfully Edited.");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());

        }
        catch (AssertionException ex)
        {
            Test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Test.Log(Status.Fail, $"Error occurred: {ex.Message}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
            throw;
        }

    }

    /// <summary>
    /// Negative Scenario - Add Empty value as a ToDo. 
    /// </summary>
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
            Test.Log(Status.Pass, "Empty value is not added to the list");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
        }
        catch (AssertionException ex)
        {
            Test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Test.Log(Status.Fail, $"Error occurred: {ex.Message}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
            throw;
        }

    }

    /// <summary>
    ///  Change ToDos' status from Complete to active.
    /// </summary>
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
            Test.Log(Status.Pass, "No Completed ToDos in the completed tab");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());

        }
        catch (AssertionException ex)
        {
            Test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Test.Log(Status.Fail, $"Error occurred: {ex.Message}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
            throw;
        }
    }

    /// <summary>
    /// Negative Scenario - Verify "Clear Completed" button clear Active ToDos.
    /// </summary>
    [Test, Order(8)]
    public void VerifyClearCompletedButtonIfClearActiveToDos()
    {
        try
        {   // Act.

            // Get the count of active ToDos in the list before click clear completed button.
            _toDoPage.ClickActiveTab();
            var beforeCount = _toDoPage.GetAvailableToDoListInSelectedTab().Count();
            _toDoPage.CickClearCompleted();

            // Get the count of active ToDos in the list After click clear completed button.
            var afterCount = _toDoPage.GetAvailableToDoListInSelectedTab().Count();


            // Assert
            Assert.That(beforeCount, Is.EqualTo(afterCount));
            Test.Log(Status.Pass, "Active ToDos are not cleared.");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());

        }
        catch (AssertionException ex)
        {
            Test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Test.Log(Status.Fail, $"Error occurred: {ex.Message}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
            throw;
        }
    }

    /// <summary>
    /// Negative scenario - Edit a selected ToDo with empty value
    /// </summary>
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
            Test.Log(Status.Pass, "ToDo is not updated with an empty value");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());

        }
        catch (AssertionException ex)
        {
            Test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Test.Log(Status.Fail, $"Error occurred: {ex.Message}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
            throw;
        }

    }

    /// <summary>
    /// Negative scenario - Verify ToDos Count get completed ToDos count as well. 
    /// </summary>
    [Test, Order(10)]
    public void VerifyToDoCountDisplaysForCompletedToDos()
    {
        try
        {
            // Act.
            _toDoPage.ClickActiveTab();
            _toDoPage.ChangeToDoStatusInSelectedTab();
            var result = _toDoPage.GetActiveToDosCount();

            // Assert.
            Assert.That(result, Is.EqualTo(0));
            Test.Log(Status.Pass, "Count is displaaying as 0.");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());

        }
        catch (AssertionException ex)
        {
            Test.Log(Status.Fail, $"Assertion failed: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Test.Log(Status.Fail, $"Error occurred: {ex.Message}");
            Test.Log(Status.Info, "Screenshot", MediaEntityBuilder
                .CreateScreenCaptureFromPath(TestReportHelper.CaptureScreenshot(ScreenshotFolderPath, Driver)).Build());
            throw;
        }
    }
}
