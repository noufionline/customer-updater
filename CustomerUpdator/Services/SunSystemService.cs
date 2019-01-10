using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CustomerUpdator.Contracts;
using Dapper;

namespace CustomerUpdator.Services
{
    public class SunSystemService : ISunSystemService
    {
        public async Task<(string AccountName, string Address)?> GetCustomer(string accountCode)
        {
           
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["SunDb"].ConnectionString))
            {
                await con.OpenAsync();
                var customer =await 
                    con.QuerySingleOrDefaultAsync<SunSystemCustomer>("SELECT * FROM View_Customers_ABS WHERE CODE=@Code",
                        new {Code = accountCode});

                if (customer == null)
                {
                    return null;
                }

                return (customer.Name, customer.GetAddress());
            }


        }
    }
}