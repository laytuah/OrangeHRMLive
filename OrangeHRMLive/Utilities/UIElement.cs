//using OpenQA.Selenium;
//using OpenQA.Selenium.Interactions;
//using OpenQA.Selenium.Support.UI;
//using OrangeHRMLive.Configuration;
//using SeleniumExtras.WaitHelpers;

//public class UIElement
//{
//    IWebDriver _driver;
//    By _locator;
//    TimeSpan _timeout = TimeSpan.FromSeconds(20);
//    WebDriverWait _wait;

//    public UIElement(IWebDriver driver, By locator)
//    {
//        _driver = driver;
//        _locator = locator;
//        _wait = new WebDriverWait(_driver, _timeout);
//    }

//    public void Click()
//    {
//        if (IsElementInteractable(_driver.FindElement(_locator)))
//        {
//            _driver.FindElement(_locator).Click();
//            WaitForLoadingIconToDisappear();
//        }
//        else
//        {
//            throw new ElementNotInteractableException("The element is not interactable.");
//        }
//    }

//    public void EnterText(string text)
//    {
//        if (IsElementInteractable(_driver.FindElement(_locator)))
//        {
//            _driver.FindElement(_locator).Clear();
//            _driver.FindElement(_locator).SendKeys(text);
//        }
//        else
//        {
//            throw new ElementNotInteractableException("The element is not interactable.");
//        }
//    }

//    public string GetTrimmedText()
//    {
//        return _driver.FindElement(_locator).Text.Trim();
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

//    public void ScrollIntoViewAndClick()
//    {
//        if (IsElementInteractable(_driver.FindElement(_locator)))
//        {
//            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", _driver.FindElement(_locator));
//            WaitForClickability();
//            _driver.FindElement(_locator).Click();
//        }
//        else
//        {
//            throw new ElementNotInteractableException("The element is not interactable.");
//        }
//    }

//    public void WaitForVisibility()
//    {
//        _wait.Until(ExpectedConditions.ElementIsVisible(_locator));
//    }

//    public void WaitForClickability()
//    {
//        _wait.Until(ExpectedConditions.ElementToBeClickable(_locator));
//    }

//    public string GetAttributeOrDefault(string attribute, string defaultValue = "")
//    {
//        try
//        {
//            return _driver.FindElement(_locator).GetAttribute(attribute) ?? defaultValue;
//        }
//        catch (NoSuchElementException)
//        {
//            return defaultValue;
//        }
//    }

//    public void Hover()
//    {
//        new Actions(_driver).MoveToElement(_driver.FindElement(_locator)).Perform();
//    }

//    public bool Contains(string text)
//    {
//        return _driver.FindElement(_locator).Text.Contains(text);
//    }

//    public bool IsChecked()
//    {
//        return _driver.FindElement(_locator).Selected;
//    }

//    public void DoubleClick()
//    {
//        new Actions(_driver).DoubleClick(_driver.FindElement(_locator)).Perform();
//    }

//    public void RightClick()
//    {
//        new Actions(_driver).ContextClick(_driver.FindElement(_locator)).Perform();
//    }

//    public void DragAndDrop(IWebElement source, IWebElement target)
//    {
//        new Actions(_driver).DragAndDrop(source, target).Perform();
//    }

//    public string GetInnerHtml()
//    {
//        return _driver.FindElement(_locator).GetAttribute("innerHTML");
//    }

//    public string GetOuterHtml()
//    {
//        return _driver.FindElement(_locator).GetAttribute("outerHTML");
//    }

//    public void SetCheckbox(bool check)
//    {
//        if (_driver.FindElement(_locator).Selected != check)
//        {
//            _driver.FindElement(_locator).Click();
//        }
//    }

//    public void SelectByText(string text)
//    {
//        new SelectElement(_driver.FindElement(_locator)).SelectByText(text);
//    }

//    public void SelectByValue(string value)
//    {
//        new SelectElement(_driver.FindElement(_locator)).SelectByValue(value);
//    }

//    public void SelectByIndex(int index)
//    {
//        new SelectElement(_driver.FindElement(_locator)).SelectByIndex(index);
//    }

//    public void DeselectAll()
//    {
//        new SelectElement(_driver.FindElement(_locator)).DeselectAll();
//    }

//    public void EnterTextWithDelay(string text, int delayInMilliseconds = 100)
//    {
//        _driver.FindElement(_locator).Clear();
//        foreach (char c in text)
//        {
//            _driver.FindElement(_locator).SendKeys(c.ToString());
//            Thread.Sleep(delayInMilliseconds);
//        }
//    }

//    public string GetCssValue(string propertyName)
//    {
//        return _driver.FindElement(_locator).GetCssValue(propertyName);
//    }

//    public void SubmitForm()
//    {
//        _driver.FindElement(_locator).Submit();
//    }

//    public bool IsDisplayed()
//    {
//        try
//        {
//            return _driver.FindElement(_locator) != null && _driver.FindElement(_locator).Displayed;
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

//    public IWebElement FindElementWithRetry(int retryCount = 3, int delayInSeconds = 1)
//    {
//        IWebElement element = null;
//        for (int i = 0; i < retryCount; i++)
//        {
//            try
//            {
//                element = _driver.FindElement(_locator);
//                break;
//            }
//            catch (NoSuchElementException)
//            {
//                if (i == retryCount - 1)
//                    throw;
//                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(delayInSeconds));
//            }
//        }
//        return element;
//    }

//    private void WaitForLoadingIconToDisappear()
//    {
//        var loadingLocator = By.XPath(ConfigurationManager.LoadingIconXpath);
//        _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingLocator));
//    }

//    private bool IsElementInteractable(IWebElement element)
//    {
//        return element != null && element.Displayed && element.Enabled;
//    }

//    private string GetXPath(IWebElement element)
//    {
//        return (string)((IJavaScriptExecutor)_driver).ExecuteScript(
//            "function getXPath(node) {" +
//            "  if (node.id !== '') {" +
//            "    return '//' + node.tagName.toLowerCase() + '[@id=\"' + node.id + '\"]';" +
//            "  }" +
//            "  if (node === document.body) {" +
//            "    return '//html/' + node.tagName.toLowerCase();" +
//            "  }" +
//            "  var index = 0;" +
//            "  var siblings = node.parentNode.childNodes;" +
//            "  for (var i = 0; i < siblings.length; i++) {" +
//            "    var sibling = siblings[i];" +
//            "    if (sibling === node) {" +
//            "      return getXPath(node.parentNode) + '/' + node.tagName.toLowerCase() + '[' + (index + 1) + ']';" +
//            "    }" +
//            "    if (sibling.nodeType === 1 && sibling.tagName === node.tagName) {" +
//            "      index++;" +
//            "    }" +
//            "  }" +
//            "}" +
//            "return getXPath(arguments[0]);", element);
//    }
//}



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
//    IWebElement _element => _driver.FindElement(_locator);
//    TimeSpan _timeout = TimeSpan.FromSeconds(20);
//    WebDriverWait _wait;

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
//    public string GetAttribute(string attributeName) => _element.GetAttribute(attributeName);
//    public string GetCssValue(string propertyName) => _element.GetCssValue(propertyName);
//    public string GetDomAttribute(string attributeName) => _element.GetDomAttribute(attributeName);
//    public string GetDomProperty(string propertyName) => _element.GetDomProperty(propertyName);
//    public void SendKeys(string text) => _element.SendKeys(text);
//    public void Submit() => _element.Submit();
//    public ISearchContext GetShadowRoot() => _element.GetShadowRoot();
//    bool IsElementInteractable(IWebElement element) => element != null && element.Displayed && element.Enabled;
//    public void Click()
//    {
//        if (IsElementInteractable(_element))
//        {
//            _element.Click();
//            WaitForLoadingIconToDisappear();
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

//    public void ScrollIntoViewAndClick()
//    {
//        if (IsElementInteractable(_element))
//        {
//            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", _driver.FindElement(_locator));
//            WaitForClickability();
//            _element.Click();
//        }
//        else
//            throw new ElementNotInteractableException("The element is not interactable.");
//    }

//    public void WaitForClickability()
//    {
//        _wait.Until(ExpectedConditions.ElementToBeClickable(_locator));
//    }

//    public void WaitForVisibility()
//    {
//        _wait.Until(ExpectedConditions.ElementIsVisible(_locator));
//    }

//    void WaitForLoadingIconToDisappear()
//    {
//        var loadingLocator = By.XPath(ConfigurationManager.LoadingIconXpath);
//        _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingLocator));
//    }

//    public void DoubleClick()
//    {
//        new Actions(_driver).DoubleClick(_element).Perform();
//    }

//    public void Hover()
//    {
//        new Actions(_driver).MoveToElement(_element).Perform();
//    }

//    public void RightClick()
//    {
//        new Actions(_driver).ContextClick(_element).Perform();
//    }

//    public void DragAndDrop(PageElement source, PageElement target)
//    {
//        new Actions(_driver).DragAndDrop(source, target).Perform();
//    }

//    public void SetCheckbox(bool check)
//    {
//        if (_element.Selected != check)
//            _element.Click();
//    }

//    public void SelectByText(string text)
//    {
//        new SelectElement(_element).SelectByText(text);
//    }

//    public void SelectByValue(string value)
//    {
//        new SelectElement(_element).SelectByValue(value);
//    }

//    public void SelectByIndex(int index)
//    {
//        new SelectElement(_element).SelectByIndex(index);
//    }

//    public void DeselectAll()
//    {
//        new SelectElement(_element).DeselectAll();
//    }

//    public void EnterTextWithDelay(string text, int delayInMilliseconds = 100)
//    {
//        _element.Clear();
//        foreach (char c in text)
//        {
//            _element.SendKeys(c.ToString());
//            Thread.Sleep(delayInMilliseconds);
//        }
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

//    public IWebElement FindElementWithRetry(int retryCount = 3, int delayInSeconds = 1)
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
//                if (i == retryCount - 1)
//                    throw;
//                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(delayInSeconds));
//            }
//        }
//        return element;
//    }
//}



