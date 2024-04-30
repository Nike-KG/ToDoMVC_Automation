using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoMVC.Utilities;

public class WaitHelper
{
    public static IWebElement WaitUntilElementExists(WebDriver driver, By elementLocator, int timeout = 10)
    {
        try 
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until((driver) => driver.FindElement(elementLocator));

        }
        catch (NoSuchElementException) 
        {
            Console.WriteLine($"Element with Locator {elementLocator} is not found in current context page.");
            throw;
        }
    }

    public static IList<IWebElement> WaitUntilElementListExists(WebDriver driver, By elementLocator, int timeout = 10)
    {
        try
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            return wait.Until((driver) => driver.FindElements(elementLocator));

        }
        catch (NoSuchElementException)
        {
            Console.WriteLine($"Element with Locator {elementLocator} is not found in current context page.");
            throw;
        }

    }
}
