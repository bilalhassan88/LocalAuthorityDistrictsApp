using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalAuthorityDistricts.Domain
{
    public enum GeoJsonObjectType
    {
        Feature,
        FeatureCollection,
        Polygon,
        MultiPolygon,
        Point,
        MultiPoint,
        LineString,
        MultiLineString
    }
}
