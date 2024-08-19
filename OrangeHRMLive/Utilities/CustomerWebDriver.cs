using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OrangeHRMLive.Configuration;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;

public class CustomWebDriver : IWebDriver
{
    IWebDriver Driver;

    public CustomWebDriver(IWebDriver driver)
    {
        Driver = driver;
    }

    public void Dispose() => Driver.Dispose();
    public void Close() => Driver.Close();
    public void Quit() => Driver.Quit();
    public string CurrentWindowHandle => Driver.CurrentWindowHandle;
    public ReadOnlyCollection<string> WindowHandles => Driver.WindowHandles;
    public IOptions Manage() => Driver.Manage();
    public ITargetLocator SwitchTo() => Driver.SwitchTo();
    public IJavaScriptExecutor JavaScriptExecutor => (IJavaScriptExecutor)Driver;
    public ICookieJar Cookies => Driver.Manage().Cookies;
    public INavigation Navigate() => Driver.Navigate();
    public string PageSource => Driver.PageSource;
    public string Title => Driver.Title;
    public string Url
    {
        get => Driver.Url;
        set => Driver.Url = value;
    }

    public IWindow Window => Driver.Manage().Window;
    public IWebElement FindElement(By by)
    {
        WaitForLoadingIconToDisappear();
        return Driver.FindElement(by);
    }
    public ReadOnlyCollection<IWebElement> FindElements(By by)
    {
        WaitForLoadingIconToDisappear();
        return Driver.FindElements(by);
    }
    public void Click(By by)
    {
        WaitForLoadingIconToDisappear();
        IWebElement element = Driver.FindElement(by);
        element.Click();
        WaitForLoadingIconToDisappear();
    }
    public void SendKeys(By by, string text)
    {
        WaitForLoadingIconToDisappear();
        IWebElement element = Driver.FindElement(by);
        element.SendKeys(text);
        WaitForLoadingIconToDisappear();
    }
    void WaitForLoadingIconToDisappear()
    {
        if (!string.IsNullOrEmpty(ConfigurationManager.LoadingIconXpath))
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(ConfigurationManager.LoadingIconXpath)));
        }
    }
}
