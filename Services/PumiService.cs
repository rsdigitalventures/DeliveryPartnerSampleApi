using DeliveryPartnerSampleApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace DeliveryPartnerSampleApi.Services
{
    public interface IPumiService
    {
        Task<IEnumerable<KeyValuePair<string, PumiItem>>> GetAllItems(PumiItemType pumiItemType);
        Task<IEnumerable<KeyValuePair<string, PumiItem>>> GetDistrictsByProvince(string id);
        Task<IEnumerable<KeyValuePair<string, PumiItem>>> GetCommunesByParentId(string id);
        Task<IEnumerable<KeyValuePair<string, PumiItem>>> GetVillagesByParentId(string id);
    }

    public class PumiService : IPumiService
    {
        public async Task<IEnumerable<KeyValuePair<string, PumiItem>>> GetDistrictsByProvince(string id)
        {
            var districts = await GetAllItems(PumiItemType.District);
            var filteredDistricts = districts.Where(e => e.Key.StartsWith(id));
            return filteredDistricts;
        }

        public async Task<IEnumerable<KeyValuePair<string, PumiItem>>> GetCommunesByParentId(string id)
        {
            var communes = await GetAllItems(PumiItemType.Commune);
            var filteredCommunes = communes.Where(e => e.Key.StartsWith(id));
            return filteredCommunes;
        }

        public async Task<IEnumerable<KeyValuePair<string, PumiItem>>> GetVillagesByParentId(string id)
        {
            var villages = await GetAllItems(PumiItemType.Village);
            var filteredCommunes = villages.Where(e => e.Key.StartsWith(id));
            return filteredCommunes;
        }

        public async Task<IEnumerable<KeyValuePair<string, PumiItem>>> GetAllItems(PumiItemType pumiItemType)
        {
            string sampleDataBasePath = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName, "AppData", "Pumi");

            string yml = await File.ReadAllTextAsync(Path.Combine(sampleDataBasePath, $"{pumiItemType.ToString().ToLower()}s.yml"));
            var ymlReader = new StringReader(yml);
            var deserializer = new DeserializerBuilder().Build();
            var yamlObject = deserializer.Deserialize(ymlReader);

            var yamlserializer = new SerializerBuilder()
                .JsonCompatible()
                .Build();

            var json = yamlserializer.Serialize(yamlObject);

            switch (pumiItemType)
            {
                case PumiItemType.Province:
                    return JsonConvert.DeserializeObject<PumiProvinces>(json).Provinces.ToArray();
                case PumiItemType.District:
                    return JsonConvert.DeserializeObject<PumiDistricts>(json).Districts.ToArray();
                case PumiItemType.Commune:
                    return JsonConvert.DeserializeObject<PumiCommunes>(json).Communes.ToArray();
                case PumiItemType.Village:
                    return JsonConvert.DeserializeObject<PumiVillages>(json).Villages.ToArray();
                default:
                    return null;
            }
        }
    }
}
