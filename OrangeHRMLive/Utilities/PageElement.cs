//using OpenQA.Selenium;
//using OpenQA.Selenium.Interactions;
//using OpenQA.Selenium.Support.UI;
//using OrangeHRMLive.Configuration;
//using SeleniumExtras.WaitHelpers;
//using System.Collections.ObjectModel;
//using System.Drawing;

//public class PageElement : IWebElement
//{
//    IWebDriver _driver;
//    By _locator;
//    WebDriverWait _wait;
//    ReadOnlyCollection<IWebElement> elements => _driver.FindElements(_locator);
//    TimeSpan _timeout = TimeSpan.FromSeconds(20);

//    IWebElement _element
//    {
//        get
//        {
//            for (int i = 0; i < 3; i++)
//            {
//                try
//                {
//                    return _driver.FindElement(_locator);
//                }
//                catch (StaleElementReferenceException) { }
//                catch (NoSuchElementException) { }

//                Thread.Sleep(500);
//            }

//            throw new NoSuchElementException($"Element with locator '{_locator}' not found after retries.");
//        }
//    }


//    public PageElement(IWebDriver driver, By locator)
//    {
//        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
//        _locator = locator ?? throw new ArgumentNullException(nameof(locator));
//        _wait = new WebDriverWait(_driver, _timeout);
//    }

//    public string TagName => _element.TagName;
//    public string Text => _element.Text;
//    public bool Enabled => _element.Enabled;
//    public bool Selected => _element.Selected;
//    public Point Location => _element.Location;
//    public Size Size => _element.Size;
//    public bool Displayed => _element.Displayed;
//    public void Clear() => _element.Clear();
//    public IWebElement FindElement(By by) => _element.FindElement(by);
//    public ReadOnlyCollection<IWebElement> FindElements(By by) => _element.FindElements(by);
//    public List<PageElement> GetAllElements() => elements.Select(e => new PageElement(_driver, _locator)).ToList();
//    public string GetAttribute(string attributeName) => _element.GetAttribute(attributeName);
//    public string GetCssValue(string propertyName) => _element.GetCssValue(propertyName);
//    public string GetDomAttribute(string attributeName) => _element.GetDomAttribute(attributeName);
//    public string GetDomProperty(string propertyName) => _element.GetDomProperty(propertyName);
//    public void Submit() => _element.Submit();
//    public ISearchContext GetShadowRoot() => _element.GetShadowRoot();
//    public void WaitForClickability() => _wait.Until(ExpectedConditions.ElementToBeClickable(_locator));
//    public void WaitForVisibility() => _wait.Until(ExpectedConditions.ElementIsVisible(_locator));
//    public void SelectByText(string text) => new SelectElement(_element).SelectByText(text);
//    public void SelectByValue(string value) => new SelectElement(_element).SelectByValue(value);
//    public void SelectByIndex(int index) => new SelectElement(_element).SelectByIndex(index);
//    public void DeselectAll() => new SelectElement(_element).DeselectAll();
//    public void DoubleClick() => new Actions(_driver).DoubleClick(_element).Perform();
//    public void Hover() => new Actions(_driver).MoveToElement(_element).Perform();
//    public void RightClick() => new Actions(_driver).ContextClick(_element).Perform();
//    public void DragAndDrop(PageElement source, PageElement target) => new Actions(_driver).DragAndDrop(source._element, target._element).Perform();
//    public void ActionClick() => new Actions(_driver).MoveToElement(_element).Click().Perform();
//    bool IsElementInteractable(IWebElement element) => element != null && element.Displayed && element.Enabled;

//    public void Click()
//    {
//        try
//        {
//            if (IsElementInteractable(_element))
//            {
//                _element.Click();
//            }
//            else
//            {
//                WaitForClickability();
//                _element.Click();
//            }
//        }
//        catch (ElementClickInterceptedException)
//        {
//            WaitForLoadingIconToDisappear();
//            _element.Click();
//        }
//        catch (ElementNotInteractableException)
//        {
//            WaitForLoadingIconToDisappear();
//            _element.Click();
//        }
//    }

//    public void SendKeys(string text)
//    {
//        try
//        {

//            if (IsElementInteractable(_element))
//            {
//                _element.SendKeys(text);
//            }
//            else
//            {
//                WaitForLoadingIconToDisappear();
//                _element.SendKeys(text);
//            }
//        }
//        catch (ElementNotInteractableException)
//        {
//            WaitForLoadingIconToDisappear();
//            _element.Click();
//        }
//    }

//    public void JSClick()
//    {
//        if (IsElementInteractable(_element))
//        {
//            WaitForClickability();
//            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", _element);
//        }
//        else
//            throw new ElementNotInteractableException("The element is not interactable.");
//    }

//    public void ClearAndSendKeys(string text)
//    {
//        if (IsElementInteractable(_element))
//        {
//            _element.Clear();
//            _element.SendKeys(text);
//        }
//        else
//            throw new ElementNotInteractableException("The element is not interactable.");
//    }

//    public bool ElementExists()
//    {
//        try
//        {
//            return _driver.FindElements(_locator).Count > 0;
//        }
//        catch (NoSuchElementException)
//        {
//            return false;
//        }
//        catch (StaleElementReferenceException)
//        {
//            return false;
//        }
//    }

//    public void ScrollIntoView()
//    {
//        if (IsElementInteractable(_element))
//            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", _element);
//        else
//            throw new ElementNotInteractableException("The element is not interactable.");
//    }

//    public void SendKeysByJavascript(string text)
//    {
//        if (IsElementInteractable(_element))
//            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value = arguments[1];", _element, text);
//        else
//            throw new ElementNotInteractableException("The element is not interactable.");
//    }

//    public void FocusOnElement()
//    {
//        if (IsElementInteractable(_element))
//            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].focus();", _element);
//        else
//            throw new ElementNotInteractableException("The element is not interactable.");
//    }

//    void WaitForLoadingIconToDisappear()
//    {
//        By loadingIconLocator = By.XPath(ConfigurationManager.LoadingIconXpath);
//        if (_driver.FindElements(loadingIconLocator).FirstOrDefault(e => e.Displayed) != null)
//        {
//            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingIconLocator));
//        }
//    }

//    public void SetCheckbox(bool check)
//    {
//        if (_element.Selected != check)
//        {
//            _element.Click();
//        }
//    }

//    public async Task EnterTextWithDelayAsync(string text, int delayInMilliseconds = 100)
//    {
//        if (IsElementInteractable(_element))
//        {
//            _element.Clear();
//            foreach (char c in text)
//            {
//                _element.SendKeys(c.ToString());
//                await Task.Delay(delayInMilliseconds);
//            }
//        }
//        else
//            throw new ElementNotInteractableException($"Cannot enter text on the element located by '{_locator}' on page '{_driver.Url}'.");
//    }

//    public bool IsDisplayed()
//    {
//        try
//        {
//            return _element != null && _element.Displayed;
//        }
//        catch (NoSuchElementException)
//        {
//            return false;
//        }
//        catch (StaleElementReferenceException)
//        {
//            return false;
//        }
//    }

//    public void SelectAllTextBeforeSendKey(string text)
//    {
//        _element.Click();
//        _element.SendKeys(Keys.Control + "a");
//        _element.SendKeys(Keys.Backspace);
//        _element.SendKeys(text);
//    }

//    public async Task<IWebElement> FindElementWithRetryAsync(int retryCount = 3, int delayInSeconds = 1)
//    {
//        IWebElement element = null;
//        for (int i = 0; i < retryCount; i++)
//        {
//            try
//            {
//                element = _element;
//                break;
//            }
//            catch (NoSuchElementException)
//            {
//                if (i == retryCount - 1) throw;
//                await Task.Delay(TimeSpan.FromSeconds(delayInSeconds));
//            }
//        }
//        return element;
//    }
//}

using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OrangeHRMLive.Configuration;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using System.Drawing;

public class PageElement
{
    private readonly IWebDriver _driver;
    private readonly By _locator;
    private readonly WebDriverWait _wait;
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(20);

    public PageElement(IWebDriver driver, By locator)
    {
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        _locator = locator ?? throw new ArgumentNullException(nameof(locator));
        _wait = new WebDriverWait(_driver, _timeout);
    }

    private IWebElement FindFreshElement(int retries = 3, int delay = 500)
    {
        for (int i = 0; i < retries; i++)
        {
            try
            {
                var element = _driver.FindElement(_locator);
                if (element.Displayed || element.Enabled)
                    return element;
            }
            catch (StaleElementReferenceException) { }
            catch (NoSuchElementException) { }
            Thread.Sleep(delay);
        }
        throw new NoSuchElementException($"Element '{_locator}' not found or not stable after retries.");
    }

    private bool IsElementInteractable(IWebElement element) =>
        element != null && element.Displayed && element.Enabled;

    public string TagName => FindFreshElement().TagName;
    public string Text => FindFreshElement().Text;
    public bool Enabled => FindFreshElement().Enabled;
    public bool Selected => FindFreshElement().Selected;
    public Point Location => FindFreshElement().Location;
    public Size Size => FindFreshElement().Size;
    public bool Displayed => FindFreshElement().Displayed;
    public void Clear() => FindFreshElement().Clear();
    public IWebElement FindElement(By by) => FindFreshElement().FindElement(by);
    public ReadOnlyCollection<IWebElement> FindElements(By by) => FindFreshElement().FindElements(by);
    public string GetAttribute(string name) => FindFreshElement().GetAttribute(name);
    public string GetCssValue(string name) => FindFreshElement().GetCssValue(name);
    public string GetDomAttribute(string name) => FindFreshElement().GetDomAttribute(name);
    public string GetDomProperty(string name) => FindFreshElement().GetDomProperty(name);
    public void Submit() => FindFreshElement().Submit();
    public ISearchContext GetShadowRoot() => FindFreshElement().GetShadowRoot();
    public void WaitForClickability() => _wait.Until(ExpectedConditions.ElementToBeClickable(_locator));
    public void WaitForVisibility() => _wait.Until(ExpectedConditions.ElementIsVisible(_locator));
    public void SelectByText(string text) => new SelectElement(FindFreshElement()).SelectByText(text);
    public void SelectByValue(string value) => new SelectElement(FindFreshElement()).SelectByValue(value);
    public void SelectByIndex(int index) => new SelectElement(FindFreshElement()).SelectByIndex(index);
    public void DeselectAll() => new SelectElement(FindFreshElement()).DeselectAll();
    public void DoubleClick() => new Actions(_driver).DoubleClick(FindFreshElement()).Perform();
    public void Hover() => new Actions(_driver).MoveToElement(FindFreshElement()).Perform();
    public void RightClick() => new Actions(_driver).ContextClick(FindFreshElement()).Perform();
    public void ActionClick() => new Actions(_driver).MoveToElement(FindFreshElement()).Click().Perform();

    public void DragAndDrop(PageElement source, PageElement target)
    {
        new Actions(_driver)
            .DragAndDrop(source.FindFreshElement(), target.FindFreshElement())
            .Perform();
    }

    public void Click()
    {
        var element = FindFreshElement();
        if (IsElementInteractable(element))
        {
            try
            {
                element.Click();
            }
            catch (ElementClickInterceptedException)
            {
                WaitForLoadingIconToDisappear();
                FindFreshElement().Click();
            }
        }
        else
        {
            WaitForClickability();
            FindFreshElement().Click();
        }
    }

    public void SendKeys(string text)
    {
        var element = FindFreshElement();
        try
        {
            if (IsElementInteractable(element))
            {
                element.SendKeys(text);
            }
            else
            {
                WaitForLoadingIconToDisappear();
                FindFreshElement().SendKeys(text);
            }
        }
        catch (ElementNotInteractableException)
        {
            WaitForLoadingIconToDisappear();
            FindFreshElement().SendKeys(text);
        }
    }

    public void JSClick()
    {
        var element = FindFreshElement();
        if (IsElementInteractable(element))
        {
            WaitForClickability();
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
        }
        else
        {
            throw new ElementNotInteractableException("JSClick: Element not interactable.");
        }
    }

    public void ClearAndSendKeys(string text)
    {
        var element = FindFreshElement();
        if (IsElementInteractable(element))
        {
            element.Clear();
            element.SendKeys(text);
        }
        else
        {
            throw new ElementNotInteractableException("ClearAndSendKeys: Element not interactable.");
        }
    }

    public bool ElementExists()
    {
        try
        {
            return _driver.FindElements(_locator).Count > 0;
        }
        catch
        {
            return false;
        }
    }

    public void ScrollIntoView()
    {
        var element = FindFreshElement();
        if (IsElementInteractable(element))
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }
    }

    public void SendKeysByJavascript(string text)
    {
        var element = FindFreshElement();
        if (IsElementInteractable(element))
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value = arguments[1];", element, text);
        }
    }

    public void FocusOnElement()
    {
        var element = FindFreshElement();
        if (IsElementInteractable(element))
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].focus();", element);
        }
    }

    private void WaitForLoadingIconToDisappear()
    {
        By loadingIconLocator = By.XPath(ConfigurationManager.LoadingIconXpath);
        var loadingIcon = _driver.FindElements(loadingIconLocator).FirstOrDefault(e => e.Displayed);
        if (loadingIcon != null)
        {
            _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingIconLocator));
        }
    }

    public void SetCheckbox(bool check)
    {
        var element = FindFreshElement();
        if (element.Selected != check)
        {
            element.Click();
        }
    }

    public async Task EnterTextWithDelayAsync(string text, int delayInMs = 100)
    {
        var element = FindFreshElement();
        if (IsElementInteractable(element))
        {
            element.Clear();
            foreach (char c in text)
            {
                element.SendKeys(c.ToString());
                await Task.Delay(delayInMs);
            }
        }
    }

    public bool IsDisplayed()
    {
        try
        {
            return FindFreshElement().Displayed;
        }
        catch
        {
            return false;
        }
    }

    public void SelectAllTextBeforeSendKey(string text)
    {
        var element = FindFreshElement();
        element.Click();
        element.SendKeys(Keys.Control + "a");
        element.SendKeys(Keys.Backspace);
        element.SendKeys(text);
    }

    public async Task<IWebElement> FindElementWithRetryAsync(int retryCount = 3, int delayInSeconds = 1)
    {
        for (int i = 0; i < retryCount; i++)
        {
            try
            {
                return FindFreshElement();
            }
            catch
            {
                if (i == retryCount - 1) throw;
                await Task.Delay(TimeSpan.FromSeconds(delayInSeconds));
            }
        }
        throw new NoSuchElementException($"Element '{_locator}' not found after retry.");
    }

    public List<PageElement> GetAllElements() =>
        _driver.FindElements(_locator).Select(e => new PageElement(_driver, _locator)).ToList();
}
