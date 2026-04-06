using NUnit.Framework;
using RickAndMortyTests.Helpers;
using RickAndMortyTests.Models;

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
/// API tests for Rick and Morty character endpoints.
/// 
/// EXERCISE TODO:
/// In Part 3, you will add Allure metadata to organize these tests:
/// - [AllureSuite], [AllureFeature] attributes on the class
/// - [AllureDescription], [AllureSeverity], [AllureTag] on individual tests
/// </summary>

[AllureSuite("Rick and Morty API")]
// In the Allure UI, it helps you group tests under a common parent.
[AllureFeature("Character Endpoints")]
// In Behaviors view, Feature is especially useful for navigating by product/API features.



public class CharacterTests : BaseTest
{
    [Test]
    [AllureDescription("This description is for test purpose only //dx")]
    public async Task GetAllCharacters_ReturnsSuccessfully()
    {
        // Arrange
        var url = $"{ApiClient.BaseUrl}/character";

        // Act
        var response = await _apiClient.GetAsync(url);
        var data = await _apiClient.ReadJsonAsync<ApiResponse<Character>>(response);

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(200), 
            $"Expected 200 OK but got {response.StatusCode}");
        Assert.That(data, Is.Not.Null);
        Assert.That(data!.Results, Is.Not.Empty);
        Assert.That(data.Info.Count, Is.GreaterThan(0));
    }

    [Test]
    [AllureDescription("Verify Rick Sanchez character data is correct")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureTag("smoke")]
    [AllureTag("character")]

    public async Task GetCharacter_RickSanchez_ReturnsCorrectData()
    {
        // Arrange
        var url = $"{ApiClient.BaseUrl}/character/1";

        // Act
        var response = await _apiClient.GetAsync(url);
        var character = await _apiClient.ReadJsonAsync<Character>(response);

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(200));
        Assert.That(character, Is.Not.Null);
        Assert.That(character!.Id, Is.EqualTo(1));
        Assert.That(character.Name, Is.EqualTo("Rick Sanchez"));
        Assert.That(character.Status, Is.EqualTo("Alive"));
        Assert.That(character.Species, Is.EqualTo("Human"));
    }

    [Test]
    public async Task GetCharacter_MortySmith_ReturnsCorrectData()
    {
        // Arrange
        var url = $"{ApiClient.BaseUrl}/character/2";

        // Act
        var response = await _apiClient.GetAsync(url);
        var character = await _apiClient.ReadJsonAsync<Character>(response);

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(200));
        Assert.That(character, Is.Not.Null);
        Assert.That(character!.Id, Is.EqualTo(2));
        Assert.That(character.Name, Is.EqualTo("Morty Smith"));
        Assert.That(character.Status, Is.EqualTo("Alive"));
    }

    [Test]
    public async Task FilterCharacters_ByNameRick_ReturnsMultipleResults()
    {
        // Arrange
        var url = $"{ApiClient.BaseUrl}/character?name=rick";

        // Act
        var response = await _apiClient.GetAsync(url);
        var data = await _apiClient.ReadJsonAsync<ApiResponse<Character>>(response);

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(200));
        Assert.That(data, Is.Not.Null);
        Assert.That(data!.Results, Is.Not.Empty);
        
        foreach (var character in data.Results)
        {
            Assert.That(character.Name.ToLower(), Does.Contain("rick"));
        }
    }

    [Test]
    public async Task FilterCharacters_ByStatus_ReturnsAliveCharacters()
    {
        // Arrange
        var url = $"{ApiClient.BaseUrl}/character?status=alive";

        // Act
        var response = await _apiClient.GetAsync(url);
        var data = await _apiClient.ReadJsonAsync<ApiResponse<Character>>(response);

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(200));
        Assert.That(data, Is.Not.Null);
        Assert.That(data!.Results, Is.Not.Empty);
        
        foreach (var character in data.Results)
        {
            Assert.That(character.Status, Is.EqualTo("Alive"));
        }
    }

    [Test]
    [AllureDescription("Verify filtering by both name and status works correctly")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureTag("regression")]
    [AllureTag("filter")]

    public async Task FilterCharacters_ByNameAndStatus_ReturnsFilteredResults()
    {
        // Arrange
        var url = $"{ApiClient.BaseUrl}/character?name=rick&status=alive";

        // Act
        var response = await _apiClient.GetAsync(url);
        var data = await _apiClient.ReadJsonAsync<ApiResponse<Character>>(response);

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(200));
        Assert.That(data, Is.Not.Null);
        Assert.That(data!.Results, Is.Not.Empty);
        
        foreach (var character in data.Results)
        {
            Assert.That(character.Name.ToLower(), Does.Contain("rick"));
            Assert.That(character.Status, Is.EqualTo("Alive"));
        }
    }

    [Test]
    public async Task GetMultipleCharacters_Returns3Characters()
    {    
        // // Arrange
        // var url = $"{ApiClient.BaseUrl}/character/1,2,3";

        // // Act
        // var response = await _apiClient.GetAsync(url);
        // var characters = await _apiClient.ReadJsonAsync<List<Character>>(response);

        // // Assert
        // Assert.That((int)response.StatusCode, Is.EqualTo(200));
        // Assert.That(characters, Is.Not.Null);
        // Assert.That(characters!.Count, Is.EqualTo(3));
        // Assert.That(characters[0].Id, Is.EqualTo(1));
        // Assert.That(characters[1].Id, Is.EqualTo(2));
        // Assert.That(characters[2].Id, Is.EqualTo(3));

        // EXERCISE TODO (Part 4):
        // This test will be enhanced with AllureApi.Step() to show step-by-step execution.

        List<Character>? characters = null;

            await AllureApi.Step("Step 1: Request characters 1, 2, 3", async () =>
            {
                var url = $"{ApiClient.BaseUrl}/character/1,2,3";
                var response = await _apiClient.GetAsync(url);
                characters = await _apiClient.ReadJsonAsync<List<Character>>(response);
                Assert.That((int)response.StatusCode, Is.EqualTo(200)); });

            AllureApi.Step("Step 2: Verify count is 3", () =>
            {
                Assert.That(characters, Is.Not.Null);
                Assert.That(characters!.Count, Is.EqualTo(3));
            });

            AllureApi.Step("Step 3: Verify character IDs", () =>
            {
                Assert.That(characters![0].Id, Is.EqualTo(1));
                Assert.That(characters[1].Id, Is.EqualTo(2));
                Assert.That(characters[2].Id, Is.EqualTo(3));
            });



    }

    [Test]
    public async Task GetCharacter_VerifyOriginAndLocation()
    {
        // Arrange
        var url = $"{ApiClient.BaseUrl}/character/1";

        // Act
        var response = await _apiClient.GetAsync(url);
        var character = await _apiClient.ReadJsonAsync<Character>(response);

        // Assert
        Assert.That((int)response.StatusCode, Is.EqualTo(200));
        Assert.That(character, Is.Not.Null);
        Assert.That(character!.Origin, Is.Not.Null);
        Assert.That(character.Origin.Name, Is.Not.Empty);
        Assert.That(character.Location, Is.Not.Null);
        Assert.That(character.Location.Name, Is.Not.Empty);
    }
}
