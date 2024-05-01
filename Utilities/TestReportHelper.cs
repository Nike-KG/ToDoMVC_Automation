using OpenQA.Selenium;

namespace TodoMVC.Utilities;

public class TestReportHelper
{
    public static string CaptureScreenshot(string screenshotFolderPath, WebDriver driver)
    {
        // Set screenshot file name with timestamp.
        string screenshotFileName = $"screenshot_{DateTime.Now:yyyyMMddHHmmssfff}.png";
        string screenshotPath = Path.Combine(screenshotFolderPath, screenshotFileName);

        // Capture screenshot
        Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
        screenshot.SaveAsFile(screenshotPath);

        return screenshotPath;
    }

    public static void CreateScreenshotFolderPath(string screenshotFolderPath)
    {
        // Create a screenshots folder if it doesn't exist
        if (!Directory.Exists(screenshotFolderPath))
        {
            Directory.CreateDirectory(screenshotFolderPath);
        }
    }
}
