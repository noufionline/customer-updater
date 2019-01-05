using System.Threading.Tasks;

namespace CustomerUpdator.Contracts
{
    public interface ISunSystemService
    {
        Task<(string AccountName, string Address)> GetCustomer(string accountCode);
    }
}