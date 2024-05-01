using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using TodoMVC.Utilities;

namespace TodoMVC.Pages;

public class ToDoPage : BasePage
{
    private WebDriver _driver;

    private IWebElement _newToDo => WaitHelper.WaitUntilElementExists(_driver, By.ClassName("new-todo"));

    //TODO - change xpath to relative
    private IWebElement _completedTab => WaitHelper.WaitUntilElementExists(_driver, By.XPath("//*[@id=\"root\"]/footer/ul/li[3]/a"));
    private IWebElement _allTab => WaitHelper.WaitUntilElementExists(_driver, By.XPath("*//a[text()='All']"));

    private IWebElement _clearCompletedButton => WaitHelper.WaitUntilElementExists(_driver, By.ClassName("clear-completed"));

    private IList<IWebElement> _toDoList => WaitHelper.WaitUntilElementListExists(_driver, By.XPath("//label[@data-testid='todo-item-label']")); 

    public ToDoPage(WebDriver driver) : base(driver) { 
    
        _driver = driver;
    }

    public void AddNewToDos(IList<string> todo)
     { 
        foreach(var todoItem in todo)
        {
            _newToDo.SendKeys(todoItem);
            _newToDo.SendKeys(Keys.Enter);
        }
    }

    public IList<string> GetAvailableToDoListInSelectedTab()
    {
        if (!_toDoList.Any())
        {
            return [];
        }
        return _toDoList.Select(x => x.Text).ToList();
    }

    //complete todos 
    // TODO: check already completed
    public IList<string> ToggleToDos(IList<string> todo)
    {
        var selectedToDo = new List<string>();
        if (!_toDoList.Any())
        {
            return [];
        }

        foreach (var todoItem in todo)
        {
            var toDoToggle = WaitHelper
                .WaitUntilElementExists(_driver,
                By.XPath($"//label[text()='{todoItem}']/preceding-sibling::input[@type='checkbox']"));

            if (toDoToggle != null)
            {
                toDoToggle.Click();
                selectedToDo.Add(todoItem);
            }
        }

        return selectedToDo;
    }

    public void ClickAllTab()
    {
        _allTab.Click();
    }

    public void ClickCompletedTab()
    {  
      _completedTab.Click();
    }

    public void CickClearCompleted()
    {
        _clearCompletedButton.Click();
    }

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

    public void ChangeToDoStatusInSelectedTab()
    {
        var allCompletedToDo = GetAvailableToDoListInSelectedTab();
        ToggleToDos(allCompletedToDo);
    }
}
