using System.Net;
using System.Net.Http.Json;

namespace RickAndMortyTests.Helpers;

/// <summary>
/// Simple HTTP client wrapper that captures response body and status code.
/// Does NOT throw on non-success status codes - lets tests handle failures explicitly.
/// This makes "expected failures" predictable for Allure reporting.
/// </summary>
public class ApiClient
{
    private readonly HttpClient _client;
    public const string BaseUrl = "https://rickandmortyapi.com/api";

    public ApiClient()
    {
        _client = new HttpClient();
    }

    /// <summary>
    /// Last HTTP response body as string. Used for Allure attachments when tests fail.
    /// </summary>
    public string LastResponseBody { get; private set; } = string.Empty;

    /// <summary>
    /// Last HTTP status code. Useful for debugging.
    /// </summary>
    public HttpStatusCode? LastStatusCode { get; private set; }

    /// <summary>
    /// Sends GET request and captures response body + status code.
    /// Does NOT throw on non-2xx responses - you must check StatusCode yourself.
    /// </summary>
    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        var response = await _client.GetAsync(url);
        LastStatusCode = response.StatusCode;
        LastResponseBody = await response.Content.ReadAsStringAsync();
        return response;
    }

    /// <summary>
    /// Deserializes response content to type T using System.Text.Json.
    /// Updates LastResponseBody in case it was called on a response from another source.
    /// </summary>
    public async Task<T?> ReadJsonAsync<T>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();
        LastResponseBody = json; // Update in case response came from elsewhere
        LastStatusCode = response.StatusCode;
        
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public void Dispose()
    {
        _client.Dispose();
    }
}
