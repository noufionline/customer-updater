using System.Collections.Generic;
using System.Threading.Tasks;
using CustomerUpdator.ViewModels;

namespace CustomerUpdator.Contracts
{
    public interface IPartnerService
    {
        Task<SunAccountDetail> GetSunAccountDetail(string accountCode);
        //Task<(string AccountName, string Address)> GetSunSystemAccountInfo(string accountCode);
        //List<SunAccount> GetSunAccounts(string accountCode);
        Task<List<LookupItem>> GetCustomersAsync();
        Task<SunAccountDetail> GetSunAccountDetail(int id);
    }
}