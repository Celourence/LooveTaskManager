using System.Text.Json.Serialization;

namespace LooveTaskManager.API.Models;

public class ProblemDetails
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "about:blank";

    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public int Status { get; set; }

    [JsonPropertyName("detail")]
    public string Detail { get; set; } = string.Empty;

    [JsonPropertyName("instance")]
    public string? Instance { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object?> Extensions { get; } = new Dictionary<string, object?>();

    public static ProblemDetails Create(
        string title,
        string detail,
        int status,
        string? errorCode = null,
        string? instance = null,
        string? type = null)
    {
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = status,
            Instance = instance,
            Type = type ?? "about:blank"
        };

        if (errorCode != null)
        {
            problemDetails.Extensions["errorCode"] = errorCode;
        }

        return problemDetails;
    }
} 