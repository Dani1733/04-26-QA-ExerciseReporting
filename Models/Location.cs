using System.Text.Json.Serialization;

namespace RickAndMortyTests.Models;

public class Location
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;
}
