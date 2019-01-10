using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerUpdator.Contracts;
using CustomerUpdator.ViewModels;
using Newtonsoft.Json;
using RestSharp;

namespace CustomerUpdator.Services
{
    public class PartnerService : IPartnerService
    {
        public async Task<SunAccountDetail> GetSunAccountDetail(string accountCode)
        {
            var client = new RestClient($"https://localhost:5051/api/partners/accountInfo/{accountCode}");
            var request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteTaskAsync(request);
            return JsonConvert.DeserializeObject<SunAccountDetail>(response.Content);
        }

        public async Task<SunAccountDetail> GetSunAccountDetail(int id)
        {
            var client = new RestClient($"https://localhost:5051/api/partners/{id}/accountInfo");
            var request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteTaskAsync(request);
            return JsonConvert.DeserializeObject<SunAccountDetail>(response.Content);
        }


        public async Task<List<LookupItem>> GetCustomersAsync()
        {
             var client = new RestClient("https://localhost:5051/api/lookup/partner-lookup");
            var request = new RestRequest(Method.GET);
            IRestResponse response = await client.ExecuteTaskAsync(request);
            return JsonConvert.DeserializeObject<List<LookupItem>>(response.Content);
        }
    }
}