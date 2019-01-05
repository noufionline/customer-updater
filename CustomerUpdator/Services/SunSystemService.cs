using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CustomerUpdator.Contracts;

namespace CustomerUpdator.Services
{
    public class SunSystemService : ISunSystemService
    {
        public async Task<(string AccountName, string Address)> GetCustomer(string accountCode)
        {
            using (var db = new AbsContext())
            {
                var item = await db.SunSystemCustomers.Where(x => x.SunAccountCode == accountCode).SingleOrDefaultAsync();
                if (item != null)
                {
                    return (item.Name, item.GetAddress());
                }
            }


            return (null, null);
        }
    }
}