using OrangeHRMLive.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
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

    private IWebElement GetElement()
    {
        for (int i = 0; i < 3; i++)
        {
            try
            {
                var element = _driver.FindElement(_locator);

                try
                {
                    if (element.Displayed || element.Enabled)
                        return element;
                }
                catch (ElementNotInteractableException)
                {
                    // Not interactable yet — skip to retry
                }
            }
            catch (StaleElementReferenceException) { }
            catch (NoSuchElementException) { }
            Thread.Sleep(500);
        }

        throw new NoSuchElementException($"Element '{_locator}' not found or not stable after retries.");
    }


    private bool IsElementInteractable(IWebElement element) => element != null && element.Displayed && element.Enabled;
    public string TagName => GetElement().TagName;
    public string Text => GetElement().Text;
    public bool Enabled => GetElement().Enabled;
    public bool Selected => GetElement().Selected;
    public Point Location => GetElement().Location;
    public Size Size => GetElement().Size;
    public bool Displayed => GetElement().Displayed;
    public void Clear() => GetElement().Clear();
    public IWebElement FindElement(By by) => GetElement().FindElement(by);
    public ReadOnlyCollection<IWebElement> FindElements(By by) => GetElement().FindElements(by);
    public string GetAttribute(string name) => GetElement().GetAttribute(name);
    public string GetCssValue(string name) => GetElement().GetCssValue(name);
    public string GetDomAttribute(string name) => GetElement().GetDomAttribute(name);
    public string GetDomProperty(string name) => GetElement().GetDomProperty(name);
    public void Submit() => GetElement().Submit();
    public ISearchContext GetShadowRoot() => GetElement().GetShadowRoot();
    public void WaitForClickability() => _wait.Until(ExpectedConditions.ElementToBeClickable(_locator));
    public void WaitForVisibility() => _wait.Until(ExpectedConditions.ElementIsVisible(_locator));
    public void SelectByText(string text) => new SelectElement(GetElement()).SelectByText(text);
    public void SelectByValue(string value) => new SelectElement(GetElement()).SelectByValue(value);
    public void SelectByIndex(int index) => new SelectElement(GetElement()).SelectByIndex(index);
    public void DeselectAll() => new SelectElement(GetElement()).DeselectAll();
    public void DoubleClick() => new Actions(_driver).DoubleClick(GetElement()).Perform();
    public void Hover() => new Actions(_driver).MoveToElement(GetElement()).Perform();
    public void RightClick() => new Actions(_driver).ContextClick(GetElement()).Perform();
    public void ActionClick() => new Actions(_driver).MoveToElement(GetElement()).Click().Perform();

    public void DragAndDrop(PageElement source, PageElement target)
    {
        new Actions(_driver)
            .DragAndDrop(source.GetElement(), target.GetElement())
            .Perform();
    }

    public void Click()
    {
        try
        {
            if (IsElementInteractable(GetElement()))
            {
                GetElement().Click();
            }
            else
            {
                WaitForClickability();
                GetElement().Click();
            }
        }
        catch (StaleElementReferenceException)
        {
            GetElement().Click();
        }
        catch (ElementClickInterceptedException)
        {
            try
            {
                WaitForLoadingIconToDisappear();
                WaitForClickability();
                GetElement().Click();
            }
            catch (ElementClickInterceptedException)
            {
                JSClick();
            }
        }
        catch (ElementNotInteractableException)
        {
            WaitForLoadingIconToDisappear();
            GetElement().Click();
        }
    }


    public void SendKeys(string text)
    {
        var element = GetElement();
        try
        {
            if (IsElementInteractable(element))
            {
                element.SendKeys(text);
            }
            else
            {
                WaitForLoadingIconToDisappear();
                GetElement().SendKeys(text);
            }
        }
        catch (ElementNotInteractableException)
        {
            WaitForLoadingIconToDisappear();
            GetElement().SendKeys(text);
        }
    }

    public void JSClick()
    {
        var element = GetElement();
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
        var element = GetElement();
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
        var element = GetElement();
        if (IsElementInteractable(element))
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }
    }

    public void SendKeysByJavascript(string text)
    {
        var element = GetElement();
        if (IsElementInteractable(element))
        {
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value = arguments[1];", element, text);
        }
    }

    public void FocusOnElement()
    {
        var element = GetElement();
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
        var element = GetElement();
        if (element.Selected != check)
        {
            element.Click();
        }
    }

    public async Task EnterTextWithDelayAsync(string text, int delayInMs = 100)
    {
        var element = GetElement();
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
            return GetElement().Displayed;
        }
        catch
        {
            return false;
        }
    }

    public void SelectAllTextBeforeSendKey(string text)
    {
        var element = GetElement();
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
                return GetElement();
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
