using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using TodoMVC.Utilities;

namespace TodoMVC.Pages;

public class ToDoPage : BasePage
{
    private WebDriver _driver;

    private IWebElement _newToDo => WaitHelper.WaitUntilElementExists(_driver, By.ClassName("new-todo"));

    private IWebElement _activeTab => WaitHelper.WaitUntilElementExists(_driver, By.XPath("*//a[text()='Active']"));

    private IWebElement _completedTab => WaitHelper.WaitUntilElementExists(_driver, By.XPath("*//a[text()='Completed']"));
    private IWebElement _allTab => WaitHelper.WaitUntilElementExists(_driver, By.XPath("*//a[text()='All']"));

    private IWebElement _clearCompletedButton => WaitHelper.WaitUntilElementExists(_driver, By.ClassName("clear-completed"));
    private IWebElement _toDoCountLabel => WaitHelper.WaitUntilElementExists(_driver, By.ClassName("todo-count"));

    private IList<IWebElement> _toDoList => WaitHelper.WaitUntilElementListExists(_driver, By.XPath("//label[@data-testid='todo-item-label']")); 

    private IWebElement GetToDoItemByLabelText(string labelText) => WaitHelper.WaitUntilElementExists(_driver, By.XPath($"//label[text()='{labelText}']/preceding-sibling::input[@type='checkbox']"));
    public ToDoPage(WebDriver driver) : base(driver) { 
    
        _driver = driver;
    }

    /// <summary>
    /// Add New ToDos.
    /// </summary>
    /// <param name="todo">ToDo list to create</param>
    public void AddNewToDos(IList<string> todo)
     { 
        foreach(var todoItem in todo)
        {
            _newToDo.SendKeys(todoItem);
            _newToDo.SendKeys(Keys.Enter);
        }
    }

    /// <summary>
    /// Get All ToDos in the selected tab.
    /// </summary>
    /// <returns></returns>
    public IList<string> GetAvailableToDoListInSelectedTab()
    {
        if (!_toDoList.Any())
        {
            return [];
        }
        return _toDoList.Select(x => x.Text).ToList();
    }

    /// <summary>
    /// Change the status of ToDos by toggling. 
    /// </summary>
    /// <param name="todo">ToDo list to create</param>
    /// <returns></returns>
    public IList<string> ToggleToDos(IList<string> todo)
    {
        var selectedToDo = new List<string>();
        if (!_toDoList.Any())
        {
            return [];
        }

        foreach (var todoItem in todo)
        {
            var toDoToggle = GetToDoItemByLabelText(todoItem);

            if (toDoToggle != null)
            {
                toDoToggle.Click();
                selectedToDo.Add(todoItem);
            }
        }

        return selectedToDo;
    }

    /// <summary>
    /// Navigate to "All" tab.
    /// </summary>
    public void ClickAllTab()
    {
        _allTab.Click();
    }

    /// <summary>
    /// Navigate to "Complete" tab.
    /// </summary>
    public void ClickCompletedTab()
    {  
      _completedTab.Click();
    }

    /// <summary>
    /// Navigate to "Active" tab.
    /// </summary>
    public void ClickActiveTab()
    {
        _activeTab.Click();
    }

    /// <summary>
    /// Click "Clear Completed" button.
    /// </summary>
    public void CickClearCompleted()
    {
        _clearCompletedButton.Click();
    }

    /// <summary>
    /// Edit a specific ToDo.
    /// </summary>
    /// <param name="selectedTodoToEdit"> ToDo list to edit.</param>
    /// <param name="newTodoValue">new value to update.</param>
    public void EditSelectedToDo(string selectedTodoToEdit, string newTodoValue)
    {
        Actions actions = new Actions(_driver);
        var todoElement = _toDoList.Single(x => x.Text == selectedTodoToEdit);
        actions.DoubleClick(todoElement).Build().Perform();
        var selectedInputToEdit = WaitHelper.WaitUntilElementExists(_driver, 
            By.XPath($"*//input[@value='{selectedTodoToEdit}']"));

        for (var l = 0; l < selectedTodoToEdit.Length; l++)
        {
            selectedInputToEdit.SendKeys(Keys.Backspace);
        }

        selectedInputToEdit.SendKeys(newTodoValue);
        selectedInputToEdit.SendKeys(Keys.Enter);
    }

    /// <summary>
    /// Change status of ToDos in the selected tab.
    /// </summary>
    public void ChangeToDoStatusInSelectedTab()
    {
        var allCompletedToDo = GetAvailableToDoListInSelectedTab();
        ToggleToDos(allCompletedToDo);
    }

    /// <summary>
    /// Get all Active ToDos count.
    /// </summary>
    /// <returns></returns>
    public int GetActiveToDosCount()
    {
        var countText = _toDoCountLabel.Text.Replace(" items left!", string.Empty);
        int.TryParse(countText, out var count);
        return count;
    }
}
