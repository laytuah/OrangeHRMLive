using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using OrangeHRMLive.Configuration;

namespace OrangeHRMLive.Utilities
{
    public static class WebDriverExtention
    {
        private static IWebDriver _driver;
        public static void InitializeDriver(IWebDriver driver)
        {
            _driver = driver;
        }
        public static void CClick(this IWebElement locator)
        {
            locator.Click();
            InitializeDriver(_driver);
            WaitForLoadingIconToDisappear();
        }

        static void WaitForLoadingIconToDisappear()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.LoadingIconXpath))
            {
                var loadingElements = _driver.FindElements(By.XPath(ConfigurationManager.LoadingIconXpath));
                if (loadingElements.Count > 0 && loadingElements[0].Displayed)
                {
                    WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(ConfigurationManager.LoadingIconXpath)));
                }
            }
        }
    }
}


//public static IWebElement FindElementWithRetry(this IWebDriver driver, By by, int retryCount = 3, int delayInSeconds = 1)
//{
//    IWebElement element = null;
//    for (int i = 0; i < retryCount; i++)
//    {
//        try
//        {
//            element = driver.FindElement(by);
//            break;
//        }
//        catch (NoSuchElementException)
//        {
//            if (i == retryCount - 1)
//                throw;
//            Thread.Sleep(TimeSpan.FromSeconds(delayInSeconds));
//        }
//    }
//    return element;
//}

//public static void WaitForElementVisible(this IWebDriver driver, By by, int timeoutInSeconds)
//{
//    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
//    wait.Until(ExpectedConditions.ElementIsVisible(by));
//}

//public static void CClick(this IWebElement locator, IWebDriver driver)
//{
//    locator.Click();
//    WaitForLoadingIconToDisappear(driver);
//}


