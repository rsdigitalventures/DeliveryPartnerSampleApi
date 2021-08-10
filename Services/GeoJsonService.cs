using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryPartnerSampleApi.Services
{
    public interface IGeoJsonService
    {
        Task<string> GetName(double latitude, double longitude, int level = 1);
        Task<bool> CheckCoverage(double latitude, double longitude);
    }

    public class GeoJsonService : IGeoJsonService
    {
        public async Task<bool> CheckCoverage(double latitude, double longitude)
        {
            string geoJsonPath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "AppData", "GeoJson");
            string geoJson = await File.ReadAllTextAsync(Path.Combine(geoJsonPath, $"level-1.json"));
            var serializer = GeoJsonSerializer.Create();

            using (var stringReader = new StringReader(geoJson))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                try
                {
                    var featureCollection = serializer.Deserialize<FeatureCollection>(jsonReader);
                    return featureCollection.Any(e => e.Geometry.Intersects(new Point(longitude, latitude)));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return false;
        }

        public async Task<string> GetName(double latitude, double longitude, int level = 1)
        {
            string geoJsonPath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "AppData", "GeoJson");
            string geoJson = await File.ReadAllTextAsync(Path.Combine(geoJsonPath, $"level-{level}.json"));
            var serializer = GeoJsonSerializer.Create();

            using (var stringReader = new StringReader(geoJson))
            using (var jsonReader = new JsonTextReader(stringReader))
            {
                try
                {
                    var featureCollection = serializer.Deserialize<FeatureCollection>(jsonReader);
                    var feature = featureCollection.FirstOrDefault(e => e.Geometry.Intersects(new Point(longitude, latitude)));
                    return feature?.Attributes[$"adm{level}_name"]?.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            return null;
        }
    }
}
