//using OpenQA.Selenium;
//using OpenQA.Selenium.Support.UI;
//using OrangeHRMLive.Configuration;
//using SeleniumExtras.WaitHelpers;
//using System.Collections.ObjectModel;

//public class CustomWebDriver : IWebDriver
//{
//    IWebDriver Driver;

//    public CustomWebDriver(IWebDriver driver)
//    {
//        Driver = driver;
//    }

//    public void Dispose() => Driver.Dispose();
//    public void Close() => Driver.Close();
//    public void Quit() => Driver.Quit();
//    public string CurrentWindowHandle => Driver.CurrentWindowHandle;
//    public ReadOnlyCollection<string> WindowHandles => Driver.WindowHandles;
//    public ReadOnlyCollection<IWebElement> FindElements(By by) => Driver.FindElements(by);
//    public IWebElement FindElement(By by) => Driver.FindElement(by);
//    public IWindow Window => Driver.Manage().Window;
//    public IOptions Manage() => Driver.Manage();
//    public ITargetLocator SwitchTo() => Driver.SwitchTo();
//    public IJavaScriptExecutor JavaScriptExecutor => (IJavaScriptExecutor)Driver;
//    public ICookieJar Cookies => Driver.Manage().Cookies;
//    public INavigation Navigate() => Driver.Navigate();
//    public string PageSource => Driver.PageSource;
//    public string Title => Driver.Title;
//    public string Url
//    {
//        get => Driver.Url;
//        set => Driver.Url = value;
//    }
//    public void Click(By by)
//    {
//        IWebElement element = Driver.FindElement(by);
//        element.Click();
//        WaitForLoadingIconToDisappear();
//    }
//    public void SendKeys(By by, string text)
//    {
//        IWebElement element = Driver.FindElement(by);
//        element.SendKeys(text);
//        WaitForLoadingIconToDisappear();
//    }
//    void WaitForLoadingIconToDisappear()
//    {
//        if (!string.IsNullOrEmpty(ConfigurationManager.LoadingIconXpath))
//        {
//            var loadingElements = Driver.FindElements(By.XPath(ConfigurationManager.LoadingIconXpath));
//            if (loadingElements.Count > 0 && loadingElements[0].Displayed)
//            {
//                WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(30));
//                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(ConfigurationManager.LoadingIconXpath)));
//            }
//        }
//    }
//}
