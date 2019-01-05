using CustomerUpdator.ViewModels;

namespace CustomerUpdator.Contracts
{
    public interface IOdooService
    {
        SunAccount GetCustomer(string accountCode);
    }
}