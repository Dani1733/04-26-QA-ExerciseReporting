using NUnit.Framework;
using RickAndMortyTests.Helpers;
using Allure.NUnit;
using System.Text;
using NUnit.Framework.Interfaces;

// Add Basic Metadata
using Allure.Net.Commons;
// This gives access to shared Allure types, most importantly:
// •	SeverityLevel (the enum used by [AllureSeverity])
// •	AllureApi (used for steps and attachments later)

using Allure.NUnit.Attributes;
//This gives you the NUnit-specific Allure attributes like:
//•	[AllureSuite], [AllureFeature], [AllureDescription], [AllureSeverity], [AllureTag]
// таг [AllureSuite] - напр. за да кажем, че група тестове ще са част от едно множество
// is used to group tests in the report. You can have multiple [AllureSuite] attributes on a test, 
//and they will be shown as nested groups in the report. For example, you can use [AllureSuite("Characters")] to group all character-related tests together.
// таг [AllureFeature] - маркираме теста като част от дадена функционалност (feature, като може да е login, update, и т.н.).
//is used to specify the feature or functionality that the test covers. 
//This is also shown in the report and can be used for filtering. For example, you can use [AllureFeature("Get Character")] 
//to indicate that a test is related to the "Get Character" feature of the API.
// [AllureDescription] - добавя описание към теста, което се показва в отчета. Много полезно за обяснение на целта на теста и какво точно проверява.
// [AllureSeverity] - задава нивото на важност на теста (например, Blocker, Critical, Normal, Minor, Trivial). Това помага при приоритизиране на тестовете и анализ на резултатите.
// [AllureTag] - добавя произволен таг към теста, който може да се използва за филтриране в отчета. Например, можете да използвате [AllureTag("Regression")] за да маркирате тестовете, които са част от регресионния набор.

namespace RickAndMortyTests.Tests;

/// <summary>
/// Base class for all API tests.
/// Contains common setup and teardown logic.
/// 
/// EXERCISE TODO:
/// In Part 2, you will add Allure auto-attachment logic to TearDown.
/// </summary>
[AllureNUnit]
public class BaseTest
{
    protected ApiClient _apiClient = null!;

    [SetUp]
    public void Setup()
    {
        _apiClient = new ApiClient();
    }

    [TearDown]
    public void TearDown()
    {
        // EXERCISE TODO (Part 2):
        // 1. Check if test failed using TestContext.CurrentContext.Result.Outcome.Status
        // 2. If failed AND _apiClient.LastResponseBody is not empty:
        //    - Use AllureApi.AddAttachment() to attach the response body
        //    - Name: "response-body.json"
        //    - Type: "application/json"
        //    - Content: System.Text.Encoding.UTF8.GetBytes(_apiClient.LastResponseBody)
        // 
        // This is the "killer feature" - automatic response capture on test failure!

        var status = TestContext.CurrentContext.Result.Outcome.Status;

        if (status == TestStatus.Failed && !string.IsNullOrWhiteSpace(_apiClient.LastResponseBody))
        {
            AllureApi.AddAttachment(
                "response-body",
                "application/json",
                Encoding.UTF8.GetBytes(_apiClient.LastResponseBody),
                "json"
            );
        }

        _apiClient.Dispose();
    }
}
