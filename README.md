# Hudl Login Test Automation

A comprehensive Selenium WebDriver test automation framework for testing Hudl's login functionality using C# and NUnit.

## 🚀 Features

- **Page Object Model (POM)** pattern implementation
- **NUnit** test framework with categorized tests
- **Selenium WebDriver** for browser automation
- **Chrome WebDriver** with anti-detection configurations
- **Screenshot capture** on test failures
- **JSON-based configuration** for test credentials
- **Comprehensive test coverage** including:
  - Valid login scenarios
  - Invalid credential handling
  - Password masking/unmasking
  - Username editing
  - Session management
  - Form validation

## 🛠️ Prerequisites

Before running this project, ensure you have:

- **.NET 9.0 SDK** or later
- **Google Chrome** browser installed
- **Visual Studio 2022** or **VS Code** (recommended)
- **Git** for version control

## 📥 Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/hudl-test-automation.git
cd hudl-test-automation
```

### 2. Restore NuGet Packages
```bash
cd HudlTestProject
dotnet restore
```

### 3. Configure Test Credentials
Create a `TestCredentials.json` file in the `Utils` folder:

```json
{
  "ValidUsername": "your-valid-email@example.com",
  "ValidPassword": "your-valid-password",
  "InvalidPassword": "wrongPassword123",
  "InvalidEmail": "invalid-email-format",
  "TestUsername": "test-user@example.com"
}
```

**⚠️ IMPORTANT**: Never commit real credentials to version control!

### 4. Build the Project
```bash
dotnet build
```

## 🧪 Running Tests

### Run All Tests
```bash
dotnet test
```

### Run Tests by Category
```bash
# Run only smoke tests
dotnet test --filter Category=Smoke

# Run security tests
dotnet test --filter Category=Security

# Run negative tests
dotnet test --filter Category=Negative

# Run validation tests
dotnet test --filter Category=Validation
```

### Run Specific Test
```bash
dotnet test --filter "ValidLogin_ShouldRedirectToHome"
```

### Generate Test Report
```bash
dotnet test --logger:trx --results-directory ./TestResults
```

## 📁 Project Structure

```
HudlTestProject/
├── Pages/                          # Page Object Models
│   ├── HomePage.cs                 # Home page interactions
│   ├── LoginPage.cs                # Login page interactions
│   └── LandingPage.cs              # Post-login page interactions
├── Tests/
│   └── LoginTests.cs               # Login test scenarios
├── Utils/
│   ├── BasePage.cs                 # Base page with common functionality
│   ├── CredentialConfig.cs         # Configuration model
│   └── TestCredentials.json        # Test data (not in repo)
└── HudlTestProject.csproj          # Project configuration
```

## 🧪 Test Categories

- **Smoke**: Critical happy path tests
- **Security**: Password masking and session tests  
- **Negative**: Invalid input scenarios
- **Validation**: Form validation tests
- **Functional**: Feature-specific tests

## 📊 Test Coverage

### Valid Login Tests
- ✅ Standard login flow
- ✅ Case-insensitive username
- ✅ Successful logout

### Security Tests  
- ✅ Password field masking
- ✅ Show/hide password toggle
- ✅ Session expiry handling

### Validation Tests
- ✅ Empty password validation
- ✅ Invalid email format
- ✅ Non-existent user handling

### Functional Tests
- ✅ Username editing
- ✅ Cookie consent handling
- ✅ Error message display

## 🔧 Configuration

### Browser Settings
The framework uses Chrome with these configurations:
- Anti-automation detection disabled
- Window maximized
- 5-second implicit wait
- 10-second explicit wait timeout

### Screenshots
Failed tests automatically capture screenshots to:
```
C:\Pics\Hudl\Screenshots\
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📝 Best Practices

- **Page Object Model**: Each page has its own class with locators and methods
- **Explicit Waits**: Uses WebDriverWait for reliable element interactions
- **Error Handling**: Comprehensive exception handling and logging
- **Test Data**: JSON-based configuration for maintainability
- **Documentation**: XML comments for all public methods

## 🚨 Troubleshooting

### Common Issues

**ChromeDriver Version Mismatch**
```bash
# Update to latest ChromeDriver
dotnet add package Selenium.WebDriver.ChromeDriver
```

**Test Credentials Not Found**
- Ensure `TestCredentials.json` exists in `Utils/` folder
- Verify JSON format is correct
- Check file is being copied to output directory

**Element Not Found Errors**
- Check if page locators have changed
- Verify wait conditions are appropriate
- Ensure Chrome browser is up to date

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👨‍💻 Author

**Your Name**
- GitHub: [AdeRick23](https://github.com/yourusername)

## 🙏 Acknowledgments

- [Selenium WebDriver](https://selenium.dev/)
- [NUnit Testing Framework](https://nunit.org/)
- [Hudl](https://www.hudl.com/) for providing the test application
