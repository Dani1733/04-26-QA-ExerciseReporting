using NUnit.Framework;
using RickAndMortyTests.Helpers;
using RickAndMortyTests.Models;

namespace RickAndMortyTests.Tests.Templates;

/// <summary>
/// Intentional failing test for demonstrating Allure auto-attachments.
/// 
/// MARKED [Explicit] so it does NOT run by default and break CI.
/// 
/// How to run this test:
/// - Remove [Explicit] attribute temporarily, OR
/// - Run explicitly: dotnet test --filter FullyQualifiedName~IntentionalFailure
/// 
/// Purpose:
/// After implementing auto-attachment in BaseTest.TearDown (Part 2),
/// this test proves that response-body.json appears in Allure report when tests fail.
/// </summary>
[TestFixture]
public class IntentionalFailureTemplate : BaseTest
{
    [Test]
    [Explicit("Demo test for Allure attachment verification - does not run by default")]
    public async Task IntentionalFailure_ForAllureDemo()
    {
        // Arrange
        var url = $"{ApiClient.BaseUrl}/character/1";

        // Act
        var response = await _apiClient.GetAsync(url);
        var character = await _apiClient.ReadJsonAsync<Character>(response);

        // Assert - This will fail intentionally
        Assert.That(character!.Name, Is.EqualTo("WRONG NAME"),
            "This assertion fails on purpose. Check Allure report for auto-attached response-body.json!");
    }
}
