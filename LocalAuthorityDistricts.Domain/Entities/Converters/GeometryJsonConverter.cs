using System.Text.Json;
using System.Text.Json.Serialization;

namespace LocalAuthorityDistricts.Domain
{
    public class GeometryJsonConverter : JsonConverter<Geometry>
    {
        public override Geometry Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            using JsonDocument doc = JsonDocument.ParseValue(ref reader);
            JsonElement root = doc.RootElement;

            string typeString = root.GetProperty("type").GetString()!;
            GeoJsonObjectType geometryType = Enum.Parse<GeoJsonObjectType>(typeString, ignoreCase: true);

            ICoordinates coordinates = geometryType switch
            {
                GeoJsonObjectType.Point => ParsePointCoordinates(root.GetProperty("coordinates")),
                GeoJsonObjectType.MultiPoint => ParseMultiPointCoordinates(root.GetProperty("coordinates")),
                GeoJsonObjectType.LineString => ParseLineStringCoordinates(root.GetProperty("coordinates")),
                GeoJsonObjectType.MultiLineString => ParseMultiLineStringCoordinates(root.GetProperty("coordinates")),
                GeoJsonObjectType.Polygon => ParsePolygonCoordinates(root.GetProperty("coordinates")),
                GeoJsonObjectType.MultiPolygon => ParseMultiPolygonCoordinates(root.GetProperty("coordinates")),
                _ => throw new JsonException($"Unsupported geometry type: {typeString}")
            };

            return new Geometry(geometryType, coordinates);
        }

        private PointCoordinates ParsePointCoordinates(JsonElement element)
        {
            List<double> coords = JsonSerializer.Deserialize<List<double>>(element.GetRawText())!;
            return new PointCoordinates(coords);
        }

        private MultiPointCoordinates ParseMultiPointCoordinates(JsonElement element)
        {
            List<List<double>> coords = JsonSerializer.Deserialize<List<List<double>>>(element.GetRawText())!;
            return new MultiPointCoordinates(coords);
        }

        private LineStringCoordinates ParseLineStringCoordinates(JsonElement element)
        {
            List<List<double>> coords = JsonSerializer.Deserialize<List<List<double>>>(element.GetRawText())!;
            return new LineStringCoordinates(coords);
        }

        private MultiLineStringCoordinates ParseMultiLineStringCoordinates(JsonElement element)
        {
            List<List<List<double>>> coords = JsonSerializer.Deserialize<List<List<List<double>>>>(element.GetRawText())!;
            return new MultiLineStringCoordinates(coords);
        }

        private PolygonCoordinates ParsePolygonCoordinates(JsonElement element)
        {
            List<List<List<double>>> coords = JsonSerializer.Deserialize<List<List<List<double>>>>(element.GetRawText())!;
            return new PolygonCoordinates(coords);
        }

        private MultiPolygonCoordinates ParseMultiPolygonCoordinates(JsonElement element)
        {
            List<List<List<List<double>>>> coords = JsonSerializer.Deserialize<List<List<List<List<double>>>>>(element.GetRawText())!;
            return new MultiPolygonCoordinates(coords);
        }

        public override void Write(Utf8JsonWriter writer, Geometry value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteStringValue(value.GeometryType.ToString());
            writer.WritePropertyName("coordinates");

            switch (value.GeometryType)
            {
                case GeoJsonObjectType.Point:
                    WritePointCoordinates(writer, (PointCoordinates)value.Coordinates, options);
                    break;

                case GeoJsonObjectType.MultiPoint:
                    WriteMultiPointCoordinates(writer, (MultiPointCoordinates)value.Coordinates, options);
                    break;

                case GeoJsonObjectType.LineString:
                    WriteLineStringCoordinates(writer, (LineStringCoordinates)value.Coordinates, options);
                    break;

                case GeoJsonObjectType.MultiLineString:
                    WriteMultiLineStringCoordinates(writer, (MultiLineStringCoordinates)value.Coordinates, options);
                    break;

                case GeoJsonObjectType.Polygon:
                    WritePolygonCoordinates(writer, (PolygonCoordinates)value.Coordinates, options);
                    break;

                case GeoJsonObjectType.MultiPolygon:
                    WriteMultiPolygonCoordinates(writer, (MultiPolygonCoordinates)value.Coordinates, options);
                    break;

                default:
                    throw new JsonException($"Unsupported geometry type: {value.GeometryType}");
            }

            writer.WriteEndObject();
        }

        private void WritePointCoordinates(Utf8JsonWriter writer, PointCoordinates coordinates, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, coordinates.Coordinates, options);
        }

        private void WriteMultiPointCoordinates(Utf8JsonWriter writer, MultiPointCoordinates coordinates, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, coordinates.Coordinates, options);
        }

        private void WriteLineStringCoordinates(Utf8JsonWriter writer, LineStringCoordinates coordinates, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, coordinates.Coordinates, options);
        }

        private void WriteMultiLineStringCoordinates(Utf8JsonWriter writer, MultiLineStringCoordinates coordinates, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, coordinates.Coordinates, options);
        }

        private void WritePolygonCoordinates(Utf8JsonWriter writer, PolygonCoordinates coordinates, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, coordinates.Coordinates, options);
        }

        private void WriteMultiPolygonCoordinates(Utf8JsonWriter writer, MultiPolygonCoordinates coordinates, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, coordinates.Coordinates, options);
        }
    }
}