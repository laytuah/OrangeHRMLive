using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OrangeHRMLive.Configuration;
using SeleniumExtras.WaitHelpers;

public class UIElement
{
    IWebDriver _driver;
    By _locator;
    TimeSpan _timeout = TimeSpan.FromSeconds(20);
    WebDriverWait _wait;

    public UIElement(IWebDriver driver, By locator)
    {
        _driver = driver;
        _locator = locator;
        _wait = new WebDriverWait(_driver, _timeout);
    }

    public void ClickElement()
    {
        if (IsElementInteractable(_driver.FindElement(_locator)))
        {
            _driver.FindElement(_locator).Click();
            WaitForLoadingIconToDisappear();
        }
        else
        {
            throw new ElementNotInteractableException("The element is not interactable.");
        }
    }

    public void EnterText(string text)
    {
        if (IsElementInteractable(_driver.FindElement(_locator)))
        {
            _driver.FindElement(_locator).Clear();
            _driver.FindElement(_locator).SendKeys(text);
        }
        else
        {
            throw new ElementNotInteractableException("The element is not interactable.");
        }
    }

    public string GetTrimmedText()
    {
        return _driver.FindElement(_locator).Text.Trim();
    }

    public bool ElementExists()
    {
        try
        {
            return _driver.FindElements(_locator).Count > 0;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
        catch (StaleElementReferenceException)
        {
            return false;
        }
    }

    public void ScrollAndClick()
    {
        if (IsElementInteractable(_driver.FindElement(_locator)))
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", _driver.FindElement(_locator));
            WaitForClickability();
            _driver.FindElement(_locator).Click();
        }
        else
        {
            throw new ElementNotInteractableException("The element is not interactable.");
        }
    }

    public void WaitForVisibility()
    {
        _wait.Until(ExpectedConditions.ElementIsVisible(_locator));
    }

    public void WaitForClickability()
    {
        _wait.Until(ExpectedConditions.ElementToBeClickable(_locator));
    }

    public string GetAttributeOrDefault(string attribute, string defaultValue = "")
    {
        try
        {
            return _driver.FindElement(_locator).GetAttribute(attribute) ?? defaultValue;
        }
        catch (NoSuchElementException)
        {
            return defaultValue;
        }
    }

    public void Hover()
    {
        new Actions(_driver).MoveToElement(_driver.FindElement(_locator)).Perform();
    }

    public bool Contains(string text)
    {
        return _driver.FindElement(_locator).Text.Contains(text);
    }

    public bool IsChecked()
    {
        return _driver.FindElement(_locator).Selected;
    }

    public void DoubleClick()
    {
        new Actions(_driver).DoubleClick(_driver.FindElement(_locator)).Perform();
    }

    public void RightClick()
    {
        new Actions(_driver).ContextClick(_driver.FindElement(_locator)).Perform();
    }

    public void DragAndDrop(IWebElement source, IWebElement target)
    {
        new Actions(_driver).DragAndDrop(source, target).Perform();
    }

    public string GetInnerHtml()
    {
        return _driver.FindElement(_locator).GetAttribute("innerHTML");
    }

    public string GetOuterHtml()
    {
        return _driver.FindElement(_locator).GetAttribute("outerHTML");
    }

    public void SetCheckbox(bool check)
    {
        if (_driver.FindElement(_locator).Selected != check)
        {
            _driver.FindElement(_locator).Click();
        }
    }

    public void SelectByText(string text)
    {
        new SelectElement(_driver.FindElement(_locator)).SelectByText(text);
    }

    public void SelectByValue(string value)
    {
        new SelectElement(_driver.FindElement(_locator)).SelectByValue(value);
    }

    public void SelectByIndex(int index)
    {
        new SelectElement(_driver.FindElement(_locator)).SelectByIndex(index);
    }

    public void DeselectAll()
    {
        new SelectElement(_driver.FindElement(_locator)).DeselectAll();
    }

    public void EnterTextWithDelay(string text, int delayInMilliseconds = 100)
    {
        _driver.FindElement(_locator).Clear();
        foreach (char c in text)
        {
            _driver.FindElement(_locator).SendKeys(c.ToString());
            Thread.Sleep(delayInMilliseconds);
        }
    }

    public string GetCssValue(string propertyName)
    {
        return _driver.FindElement(_locator).GetCssValue(propertyName);
    }

    public void SubmitForm()
    {
        _driver.FindElement(_locator).Submit();
    }

    public bool IsDisplayed()
    {
        try
        {
            return _driver.FindElement(_locator) != null && _driver.FindElement(_locator).Displayed;
        }
        catch (NoSuchElementException)
        {
            return false;
        }
        catch (StaleElementReferenceException)
        {
            return false;
        }
    }

    public IWebElement FindElementWithRetry(int retryCount = 3, int delayInSeconds = 1)
    {
        IWebElement element = null;
        for (int i = 0; i < retryCount; i++)
        {
            try
            {
                element = _driver.FindElement(_locator);
                break;
            }
            catch (NoSuchElementException)
            {
                if (i == retryCount - 1)
                    throw;
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(delayInSeconds));
            }
        }
        return element;
    }

    private void WaitForLoadingIconToDisappear()
    {
        var loadingLocator = By.XPath(ConfigurationManager.LoadingIconXpath);
        _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingLocator));
    }

    private bool IsElementInteractable(IWebElement element)
    {
        return element != null && element.Displayed && element.Enabled;
    }

    private string GetXPath(IWebElement element)
    {
        return (string)((IJavaScriptExecutor)_driver).ExecuteScript(
            "function getXPath(node) {" +
            "  if (node.id !== '') {" +
            "    return '//' + node.tagName.toLowerCase() + '[@id=\"' + node.id + '\"]';" +
            "  }" +
            "  if (node === document.body) {" +
            "    return '//html/' + node.tagName.toLowerCase();" +
            "  }" +
            "  var index = 0;" +
            "  var siblings = node.parentNode.childNodes;" +
            "  for (var i = 0; i < siblings.length; i++) {" +
            "    var sibling = siblings[i];" +
            "    if (sibling === node) {" +
            "      return getXPath(node.parentNode) + '/' + node.tagName.toLowerCase() + '[' + (index + 1) + ']';" +
            "    }" +
            "    if (sibling.nodeType === 1 && sibling.tagName === node.tagName) {" +
            "      index++;" +
            "    }" +
            "  }" +
            "}" +
            "return getXPath(arguments[0]);", element);
    }
}
