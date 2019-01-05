using System.Threading.Tasks;
using CustomerUpdator.ViewModels;

namespace CustomerUpdator.Contracts
{
    public interface IPartnerService
    {
        Task<SunAccountDetail> GetSunAccountDetail(string accountCode);
        //Task<(string AccountName, string Address)> GetSunSystemAccountInfo(string accountCode);
        //List<SunAccount> GetSunAccounts(string accountCode);
    }
}