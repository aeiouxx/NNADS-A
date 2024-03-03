using System.Text.Json;
using System.Text.Json.Serialization;

namespace RailSim.Model.Persistence
{
    public class EdgeConverter<TVertex> : JsonConverter<IEdge<TVertex>>
        where TVertex : notnull
    {
        public override IEdge<TVertex> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? edgeType = null;
            TVertex? from = default;
            TVertex? to = default;
            Option<TVertex>? traversibleFor = Option<TVertex>.None;

            reader.Read();
            while (reader.TokenType != JsonTokenType.EndObject)
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propName = reader.GetString();
                    reader.Read();
                    switch (propName)
                    {
                        case "Type":
                            edgeType = reader.GetString();
                            break;
                        case "From":
                            from = JsonSerializer.Deserialize<TVertex>(ref reader, options);
                            break;
                        case "To":
                            to = JsonSerializer.Deserialize<TVertex>(ref reader, options);
                            break;
                        case "TraversibleFor":
                            if (reader.TokenType == JsonTokenType.Null)
                            {
                                traversibleFor = Option<TVertex>.None;
                                reader.Read(); // Skip the null value
                            }
                            else
                            {
                                var value = JsonSerializer.Deserialize<TVertex>(ref reader, options);
                                traversibleFor = new Option<TVertex>(value);
                            }
                            break;
                    }
                }
                reader.Read();
            }
            return edgeType switch
            {
                "Edge" => new Edge<TVertex>(from!, to!),
                "GauntletEdge" => new GauntletEdge<TVertex>(from!, to!, traversibleFor!.Value),
                _ => throw new JsonException($"Unknown edge type: {edgeType}")
            };
        }
        public override void Write(Utf8JsonWriter writer, IEdge<TVertex> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            switch (value)
            {
                case GauntletEdge<TVertex> gauntletEdge:
                    {
                        writer.WriteString("Type", "GauntletEdge");
                        writer.WritePropertyName("From");
                        JsonSerializer.Serialize(writer, gauntletEdge.From, options);
                        writer.WritePropertyName("To");
                        JsonSerializer.Serialize(writer, gauntletEdge.To, options);
                        writer.WritePropertyName("TraversibleFor");
                        if (gauntletEdge.TraversibleFor.HasValue)
                        {
                            JsonSerializer.Serialize(writer, gauntletEdge.TraversibleFor.Value, options);
                        }
                        else
                        {
                            writer.WriteNullValue();
                        }
                    }
                    break;
                case Edge<TVertex> simpleEdge:
                    {
                        writer.WriteString("Type", "Edge");
                        writer.WritePropertyName("From");
                        JsonSerializer.Serialize(writer, simpleEdge.From, options);
                        writer.WritePropertyName("To");
                        JsonSerializer.Serialize(writer, simpleEdge.To, options);
                    }
                    break;
            }

            writer.WriteEndObject();
        }
    }
}
