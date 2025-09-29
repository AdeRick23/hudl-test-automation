using OpenQA.Selenium;
using HudlProject.Utils;
using SeleniumExtras.WaitHelpers;

namespace HudlProject.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver) {}
        // Locators
        private  By UsernameField => By.Id("username");
        private  By PasswordField => By.Id("password");
        private  By LoginButton => By.ClassName("c6397d3dd");
        private  By ShowPasswordToggle => By.CssSelector("[aria-label='Show password']");
        private  By EditUserNameButton => By.CssSelector("[data-link-name='edit-username']");
        private  By cookieConsentButton => By.Id("onetrust-accept-btn-handler");
        private By NoUsernameErrorMessage => By.Id("error-cs-username-required");
        private  By InvaidUsernameErrorMessage => By.Id("error-cs-email-invalid");
        private  By NoPasswordErrorMessage => By.Id("error-cs-password-required");
        private  By InvalidPasswordErrorMessage => By.Id("error-element-password");

        // Methods
        public void EnterUsername(string username)
        {
            Driver?.FindElement(UsernameField).Clear();
            Driver?.FindElement(UsernameField).SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            Driver?.FindElement(PasswordField).SendKeys(password);
        }

        public void ClickLogin()
        {
            Driver?.FindElement(LoginButton).Click();
        }
        public bool IsPasswordMasked()
        {
            return Driver.FindElement(PasswordField).GetAttribute("type") == "password";
        }
        public void ToggleShowPassword()
        {
            Driver.FindElement(ShowPasswordToggle).Click();
        }

        public void ClickEditUsername()
        {
            Driver.FindElement(EditUserNameButton).Click();
        }
        public void AcceptCookiesIfPresent()
        {
            try
            {
                ElementWait.Until(ExpectedConditions.InvisibilityOfElementWithText(cookieConsentButton, "Accept All Cookies"));
                var cookieButton = Driver.FindElement(cookieConsentButton);
                if (cookieButton.Displayed)
                {
                    cookieButton.Click();
                }
            }
            catch (NoSuchElementException)
            {
                // Cookie consent button not present, do nothing
            }
        }
        public string GetErrorMessage()
        {
            try
            {
                if (Driver.FindElement(NoUsernameErrorMessage).Displayed)
                    return Driver.FindElement(NoUsernameErrorMessage).Text;
            }
            catch (NoSuchElementException) { }

            try
            {
                if (Driver.FindElement(InvaidUsernameErrorMessage).Displayed)
                    return Driver.FindElement(InvaidUsernameErrorMessage).Text;
            }
            catch (NoSuchElementException) { }

            try
            {
                if (Driver.FindElement(NoPasswordErrorMessage).Displayed)
                    return Driver.FindElement(NoPasswordErrorMessage).Text;
            }
            catch (NoSuchElementException) { }

            try
            {
                if (Driver.FindElement(InvalidPasswordErrorMessage).Displayed)
                    return Driver.FindElement(InvalidPasswordErrorMessage).Text;
            }
            catch (NoSuchElementException) { }

            return string.Empty;
        }
    }
}