using System.Text.Json;
using System.Text.Json.Serialization;

namespace RailSim.Model.Persistence
{
    public class AdjacencyGraphConverter<TVertex, TEdge> : JsonConverter<AdjacencyGraph<TVertex, TEdge>>
    where TVertex : notnull
    where TEdge : IEdge<TVertex>
    {
        public override AdjacencyGraph<TVertex, TEdge>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var graph = new AdjacencyGraph<TVertex, TEdge>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    break;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    continue;
                }

                var propertyName = reader.GetString();
                reader.Read();

                switch (propertyName)
                {
                    case "Vertices":
                        {
                            reader.Read();
                            while (reader.TokenType != JsonTokenType.EndArray)
                            {
                                var vertex = JsonSerializer.Deserialize<TVertex>(ref reader, options);
                                graph.AddVertex(vertex);
                                reader.Read();
                            }
                        }
                        break;
                    case "Edges":
                        {
                            reader.Read();
                            while (reader.TokenType != JsonTokenType.EndArray)
                            {
                                var edge = JsonSerializer.Deserialize<TEdge>(ref reader, options);
                                graph.AddEdge(edge);
                                reader.Read();
                            }
                        }
                        break;
                }
            }

            return graph;
        }

        public override void Write(Utf8JsonWriter writer, AdjacencyGraph<TVertex, TEdge> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Vertices");
            writer.WriteStartArray();
            foreach (var vertex in value.Vertices)
            {
                JsonSerializer.Serialize(writer, vertex, options);
            }
            writer.WriteEndArray();

            writer.WritePropertyName("Edges");
            writer.WriteStartArray();
            foreach (var edge in value.Edges)
            {
                JsonSerializer.Serialize(writer, edge, options);
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}
