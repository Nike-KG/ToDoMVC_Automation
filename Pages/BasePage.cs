using OpenQA.Selenium;

namespace TodoMVC.Pages;

public class BasePage
{
    private WebDriver _driver;
    public BasePage(WebDriver driver) { 
    
        _driver = driver;
    }

    /// <summary>
    /// Navigating to a specific page.
    /// </summary>
    /// <param name="url"></param>
    public void NavigateToURL(string url) => _driver.Navigate().GoToUrl(url);

    /// <summary>
    /// Get current page title.
    /// </summary>
    /// <returns></returns>
    public string GetPageTitle() => _driver.Title;
}
