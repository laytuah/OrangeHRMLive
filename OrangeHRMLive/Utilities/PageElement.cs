using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OrangeHRMLive.Configuration;
using SeleniumExtras.WaitHelpers;
using System.Collections.ObjectModel;
using System.Drawing;

public class PageElement : IWebElement
{
    IWebDriver _driver;
    By _locator;
    IWebElement _element => _driver.FindElement(_locator);
    TimeSpan _timeout = TimeSpan.FromSeconds(20);
    WebDriverWait _wait;

    public PageElement(IWebDriver driver, By locator)
    {
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        _locator = locator ?? throw new ArgumentNullException(nameof(locator));
        _wait = new WebDriverWait(_driver, _timeout);
    }

    public string TagName => _element.TagName;
    public string Text => _element.Text;
    public bool Enabled => _element.Enabled;
    public bool Selected => _element.Selected;
    public Point Location => _element.Location;
    public Size Size => _element.Size;
    public bool Displayed => _element.Displayed;
    public void Clear() => _element.Clear();
    public IWebElement FindElement(By by) => _element.FindElement(by);
    public ReadOnlyCollection<IWebElement> FindElements(By by) => _element.FindElements(by);
    public string GetAttribute(string attributeName) => _element.GetAttribute(attributeName);
    public string GetCssValue(string propertyName) => _element.GetCssValue(propertyName);
    public string GetDomAttribute(string attributeName) => _element.GetDomAttribute(attributeName);
    public string GetDomProperty(string propertyName) => _element.GetDomProperty(propertyName);
    public void SendKeys(string text) => _element.SendKeys(text);
    public void Submit() => _element.Submit();
    public ISearchContext GetShadowRoot() => _element.GetShadowRoot();
    bool IsElementInteractable(IWebElement element) => element != null && element.Displayed && element.Enabled;
    public void Click()
    {
        if (IsElementInteractable(_element))
        {
            _element.Click();
            WaitForLoadingIconToDisappear();
        }
        else
            throw new ElementNotInteractableException("The element is not interactable.");
    }

    public void ClearAndSendKeys(string text)
    {
        if (IsElementInteractable(_element))
        {
            _element.Clear();
            _element.SendKeys(text);
        }
        else
            throw new ElementNotInteractableException("The element is not interactable.");
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

    public void ScrollIntoViewAndClick()
    {
        if (IsElementInteractable(_element))
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", _driver.FindElement(_locator));
            WaitForClickability();
            _element.Click();
        }
        else
            throw new ElementNotInteractableException("The element is not interactable.");
    }

    public void WaitForClickability()
    {
        _wait.Until(ExpectedConditions.ElementToBeClickable(_locator));
    }

    public void WaitForVisibility()
    {
        _wait.Until(ExpectedConditions.ElementIsVisible(_locator));
    }

    void WaitForLoadingIconToDisappear()
    {
        var loadingLocator = By.XPath(ConfigurationManager.LoadingIconXpath);
        _wait.Until(ExpectedConditions.InvisibilityOfElementLocated(loadingLocator));
    }

    public void DoubleClick()
    {
        new Actions(_driver).DoubleClick(_element).Perform();
    }

    public void Hover()
    {
        new Actions(_driver).MoveToElement(_element).Perform();
    }

    public void RightClick()
    {
        new Actions(_driver).ContextClick(_element).Perform();
    }

    public void DragAndDrop(PageElement source, PageElement target)
    {
        new Actions(_driver).DragAndDrop(source, target).Perform();
    }

    public void SetCheckbox(bool check)
    {
        if (_element.Selected != check)
            _element.Click();
    }

    public void SelectByText(string text)
    {
        new SelectElement(_element).SelectByText(text);
    }

    public void SelectByValue(string value)
    {
        new SelectElement(_element).SelectByValue(value);
    }

    public void SelectByIndex(int index)
    {
        new SelectElement(_element).SelectByIndex(index);
    }

    public void DeselectAll()
    {
        new SelectElement(_element).DeselectAll();
    }

    public void EnterTextWithDelay(string text, int delayInMilliseconds = 100)
    {
        _element.Clear();
        foreach (char c in text)
        {
            _element.SendKeys(c.ToString());
            Thread.Sleep(delayInMilliseconds);
        }
    }

    public bool IsDisplayed()
    {
        try
        {
            return _element != null && _element.Displayed;
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
                element = _element;
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
}
