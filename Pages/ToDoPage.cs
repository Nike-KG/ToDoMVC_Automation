using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoMVC.Utilities;

namespace TodoMVC.Pages;

public class ToDoPage : BasePage
{
    private WebDriver _driver;

    private IWebElement _newToDo => WaitHelper.WaitUntilElementExists(_driver, By.ClassName("new-todo"));
    private IWebElement _editToDo => WaitHelper.WaitUntilElementExists(_driver, By.XPath("//*[@data-testid='text-input']"));
    private IWebElement _completedTab => WaitHelper.WaitUntilElementExists(_driver, By.XPath("//*[@id=\"root\"]/footer/ul/li[3]/a"));

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

    public IList<string> GetAvailableToDoList()
    {
        if (!_toDoList.Any())
        {
            return [];
        }
        return _toDoList.Select(x => x.Text).ToList();
    }

    //complete todos 
    // TODO: check already completed
    public IList<string> CompleteToDosInAll(IList<string> todo)
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

    public void ClickCompletedTab()
    {  
      _completedTab.Click();
    }

    public void EditSelectedToDO(WebDriver driver)
    {
        Actions actions = new Actions(driver);
        actions.DoubleClick(_editToDo).Build().Perform();
        //actions.C;

        //_editToDo.SendKeys("");

    }
}
