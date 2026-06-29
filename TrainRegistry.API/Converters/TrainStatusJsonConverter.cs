using System.Text.Json;
using System.Text.Json.Serialization;
using TrainRegistry.Domain.ValueObjects;

namespace TrainRegistry.API.Converters
{
    public class TrainStatusJsonConverter: JsonConverter<TrainStatus>
    {
        public override void Write(Utf8JsonWriter utf8JsonWriter, TrainStatus trainStatus, JsonSerializerOptions jsonSerializerOptions)
        {
            utf8JsonWriter.WriteStringValue(trainStatus.Value);
        }

        public override TrainStatus? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            return value is null ? throw new JsonException("Train status cannot be nnull!") : TrainStatus.From(value);
        }
    }
}
