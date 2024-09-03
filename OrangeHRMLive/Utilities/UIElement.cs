using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OrangeHRMLive.Configuration;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;

public class UIElement
{
    IWebDriver _driver;

    public UIElement(IWebDriver driver)
    {
        _driver = driver;
    }

    public void ClickElement(IWebElement element)
    {
        if (element != null && element.Displayed && element.Enabled)
        {
            element.Click();
            WaitForLoadingIconToDisappear();
        }
        else
            throw new ElementNotInteractableException("The element is not interactable.");
    }

    public void EnterText(IWebElement element, string text)
    {
        if (element != null && element.Displayed && element.Enabled)
        {
            element.Clear();
            element.SendKeys(text);
        }
        else
            throw new ElementNotInteractableException("The element is not interactable.");
    }

    public string GetTrimmedText(IWebElement element)
    {
        return element.Text.Trim();
    }

    public bool ElementExits(By locator)
    {
        try
        {
            return _driver.FindElements(locator).Count() > 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void ScrollAndClick(IWebElement element)
    {
        if (element != null && element.Displayed && element.Enabled)
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
            element.Click();
        }
        else
            throw new ElementNotInteractableException("The element is not interactable.");
    }

    public void WaitForVisibility(IWebElement element, TimeSpan timeout)
    {
        var wait = new WebDriverWait(_driver, timeout);
        wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(GetXPath(element))));
    }

    public void WaitForClickability(IWebElement element, TimeSpan timeout)
    {
        var wait = new WebDriverWait(_driver, timeout);
        wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(GetXPath(element))));
    }

    public string GetAttributeOrDefault(IWebElement element, string attribute, string defaultValue = "")
    {
        return element?.GetAttribute(attribute) ?? defaultValue;
    }

    public void Hover(IWebElement element)
    {
        Actions actions = new Actions(_driver);
        actions.MoveToElement(element).Perform();
    }

    public bool ContainsText(IWebElement element, string text)
    {
        return element?.Text.Contains(text) ?? false;
    }

    public bool IsChecked(IWebElement element)
    {
        return element.Selected;
    }

    public void DoubleClick(IWebElement element)
    {
        Actions actions = new Actions(_driver);
        actions.DoubleClick(element).Perform();
    }

    public void RightClick(IWebElement element)
    {
        Actions actions = new Actions(_driver);
        actions.ContextClick(element).Perform();
    }

    public void DragAndDrop(this IWebElement source, IWebElement target)
    {
        Actions actions = new Actions(_driver);
        actions.DragAndDrop(source, target).Perform();
    }

    public string GetInnerHtml(IWebElement element)
    {
        return element.GetAttribute("innerHTML");
    }

    public string GetOuterHtml(IWebElement element)
    {
        return element.GetAttribute("outerHTML");
    }

    public void SetCheckbox(IWebElement element, bool check)
    {
        if (element.Selected != check)
            element.Click();
        else
            throw new Exception("Unable to click checkbox");
    }

    public void SelectByText(IWebElement element, string text)
    {
        var selectElement = new SelectElement(element);
        selectElement.SelectByText(text);
    }

    public void SelectByValue(IWebElement element, string value)
    {
        var selectElement = new SelectElement(element);
        selectElement.SelectByValue(value);
    }

    public void SelectByIndex(IWebElement element, int index)
    {
        var selectElement = new SelectElement(element);
        selectElement.SelectByIndex(index);
    }

    public void DeselectAll(IWebElement element)
    {
        var selectElement = new SelectElement(element);
        selectElement.DeselectAll();
    }

    public void EnterTextWithDelay(IWebElement element, string text, int delayInMilliseconds = 100)
    {
        element.Clear();
        foreach (char c in text)
        {
            element.SendKeys(c.ToString());
            Thread.Sleep(delayInMilliseconds);
        }
    }

    public string GetCssValue(IWebElement element, string propertyName)
    {
        return element.GetCssValue(propertyName);
    }

    public void SubmitForm(IWebElement element)
    {
        element.Submit();
    }

    public bool IsDisplayed(IWebElement element)
    {
        try
        {
            return element != null && element.Displayed;
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

    public IWebElement? FindElementWithRetry(IWebElement locator, int retryCount = 3, int delayInSeconds = 1)
    {
        IWebElement? element = null;
        for (int i = 0; i < retryCount; i++)
        {
            try
            {
                element = locator;
                break;
            }
            catch (NoSuchElementException)
            {
                if (i == retryCount - 1)
                    throw;
                Thread.Sleep(TimeSpan.FromSeconds(delayInSeconds));
            }
        }
        return element;
    }

    void WaitForLoadingIconToDisappear()
    {
        var loadingElements = _driver.FindElements(By.XPath(ConfigurationManager.LoadingIconXpath));
        if (loadingElements.Count > 0 && loadingElements[0].Displayed)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(ConfigurationManager.LoadingIconXpath)));
        }
    }

    string GetXPath(IWebElement element)
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
