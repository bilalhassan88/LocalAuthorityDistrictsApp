//using LocalAuthorityDistricts.Domain;
//using System.Text.Json.Serialization;
//using System.Text.Json;

//public class CoordinatesJsonConverter : JsonConverter<ICoordinates>
//{
//    public override ICoordinates Read(
//        ref Utf8JsonReader reader,
//        Type typeToConvert,
//        JsonSerializerOptions options
//    )
//    {
//        // Use the parent GeometryType to determine the coordinates structure
//        if (reader.TokenType == JsonTokenType.StartArray)
//        {
//            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
//            {
//                var root = doc.RootElement;

//                // Get the GeometryType from the parent Geometry object
//                if (options.TryGetProperty("GeometryType", out var geometryTypeProp) &&
//                    geometryTypeProp is GeoJsonObjectType geometryType)
//                {
//                    switch (geometryType)
//                    {
//                        case GeoJsonObjectType.Point:
//                            return DeserializePoint(root);
//                        case GeoJsonObjectType.MultiPolygon:
//                            return DeserializeMultiPolygon(root);
//                        case GeoJsonObjectType.Polygon:
//                            return DeserializePolygon(root);
//                        case GeoJsonObjectType.LineString:
//                            return DeserializeLineString(root);
//                        default:
//                            throw new JsonException($"Unsupported geometry type: {geometryType}");
//                    }
//                }
//                else
//                {
//                    throw new JsonException("GeometryType not found in JsonSerializerOptions.");
//                }
//            }
//        }

//        throw new JsonException("Expected an array for GeoJSON coordinates.");
//    }

//    private PointCoordinates DeserializePoint(JsonElement root)
//    {
//        var coordinates = JsonSerializer.Deserialize<List<double>>(root.GetRawText());
//        return new PointCoordinates(coordinates);
//    }

//    private MultiPolygonCoordinates DeserializeMultiPolygon(JsonElement root)
//    {
//        var coordinates = JsonSerializer.Deserialize<List<List<List<List<double>>>>>(root.GetRawText());
//        return new MultiPolygonCoordinates(coordinates);
//    }

//    private PolygonCoordinates DeserializePolygon(JsonElement root)
//    {
//        var coordinates = JsonSerializer.Deserialize<List<List<List<double>>>>(root.GetRawText());
//        return new PolygonCoordinates(coordinates);
//    }

//    private LineStringCoordinates DeserializeLineString(JsonElement root)
//    {
//        var coordinates = JsonSerializer.Deserialize<List<List<double>>>(root.GetRawText());
//        return new LineStringCoordinates(coordinates);
//    }

//    public override void Write(Utf8JsonWriter writer, ICoordinates value, JsonSerializerOptions options)
//    {
//        throw new NotSupportedException();
//    }
//}