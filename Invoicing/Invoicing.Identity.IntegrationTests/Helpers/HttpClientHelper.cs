using System.Text.Json;
using System.Text.Json.Serialization;

namespace Invoicing.Identity.Tests.Helpers;

public static class HttpClientHelper
{
    private static readonly JsonSerializerOptions _defaultSerializerOptions;

    static HttpClientHelper()
    {
        _defaultSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };
    }

    public static async Task<T> GetDeserializedResponseAsync<T>(HttpResponseMessage response)
    {
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(responseBody, _defaultSerializerOptions);
    }
}