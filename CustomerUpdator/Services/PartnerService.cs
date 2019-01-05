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
            var client = new RestClient("https://localhost:5051/api/partners/accountInfo/141320");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "efd93b6f-5cc0-4637-93ec-83c54c026a78");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Authorization", "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6IjZiYjYzZWVmYTMzZjRmNjY3OTUyZWRkNTkwZTBlYTkxIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NDY0MDc0NjcsImV4cCI6MTU0NjQxMTA2NywiaXNzIjoiaHR0cHM6Ly9hYnMuY2ljb25vbmxpbmUuY29tL3plb24iLCJhdWQiOlsiaHR0cHM6Ly9hYnMuY2ljb25vbmxpbmUuY29tL3plb24vcmVzb3VyY2VzIiwiYWJzY2xhaW1zYXBpIiwiYWJzY29yZWFwaSJdLCJjbGllbnRfaWQiOiJhYnNlUk9QIiwic3ViIjoiZTAxZmUzMjItMTc2ZS00YTVlLTg1MmUtYWQxYTUzODk3ZWIzIiwiYXV0aF90aW1lIjoxNTQ2NDA3NDY3LCJpZHAiOiJsb2NhbCIsIkRpdmlzaW9uSWQiOiIxIiwiRW1wbG95ZWVJZCI6IjE1MzQiLCJuYW1lIjoiTm91ZmFsIEFib29iYWNrZXIiLCJFbXBsb3llZU5hbWUiOiJOb3VmYWwgQWJvb2JhY2tlciIsIkVtYWlsIjoibm91ZmFsQGNpY29uLm5ldCIsInNjb3BlIjpbImFic2NsYWltc2FwaSIsImFic2NvcmVhcGkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsicHdkIl19.CqItK_fuCNYlj_mo2zTlZL_tJmPv5qg_VbMvFuPoAKEYdBHqSpNi2bgH1ZUChfVMCNfwwwt9snaUrYi75dOnzb3DgCKuEtIYtG6rznpohPT0_X0LYtmCMupiXrrdSt4fquu_MDxV5q89memtYU6zUCjWGHiM7on5ryK6sAGFDvi8SP_RyNmvTfIwfzql0rKNPqMmr13CXBCIqtiI7E6x9YiBXPpdbk-bKLjdRtO_tUnEhmxQ1nhk3xhwbyvL_DFUZSl52-yo9WHXi-NHBx0iF9D7JVeagJOTMpWnb6CnDB_Y0-T_uOYDroUWUz4Fk1Fjc9tIMSZBN_XWGitCrO_LKw");
            IRestResponse response = await client.ExecuteTaskAsync(request);

            return JsonConvert.DeserializeObject<SunAccountDetail>(response.Content);
        }
  
    }
}