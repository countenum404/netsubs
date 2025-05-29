using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NetSubs.ApiService.Api.Converters;

internal sealed class DateTimeConverter : JsonConverter<DateTime>
{
    private static readonly string[] SupportedFormats = new[]
    {
        "dd.MM.yyyy",
        "yyyy-MM-dd'T'HH:mm:ss.SSSK",
        "O"
    };

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException($"Expected string for DateTime but got {reader.TokenType}");
        
        var rawDate = reader.GetString();
        foreach (var fmt in SupportedFormats)
        {
            if (DateTime.TryParseExact(rawDate, fmt, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var parsedDate))
                return parsedDate;
        }

        throw new InvalidOperationException("Invalid format of datetime.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("dd.MM.yyyy"));
    }
}
