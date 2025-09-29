using OpenQA.Selenium;
using HudlProject.Utils;
using SeleniumExtras.WaitHelpers;

namespace HudlProject.Pages
{
    public class LandingPage : BasePage
    {
        public LandingPage(IWebDriver driver) : base(driver) {}

        // Locators
        private By DisplayName => By.CssSelector(".hui-globaluseritem__display-name");
        private By LogoutOption => By.CssSelector("[data-qa-id='webnav-usermenu-logout']");

        // Methods
        public bool IsUserLoggedIn()
        {
            try
            {
                //Implemented a wait as tests were failing due to the element loading slowly
                return ElementWait.Until(ExpectedConditions.ElementIsVisible(DisplayName)).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public void Logout()
        {
            ElementWait.Until(ExpectedConditions.ElementToBeClickable(DisplayName)).Click();
            ElementWait.Until(ExpectedConditions.ElementToBeClickable(LogoutOption)).Click();
        }
    }
}