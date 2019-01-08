using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerUpdator.Contracts;
using CustomerUpdator.ViewModels;


namespace CustomerUpdator.Services
{
    public class OdooService : IOdooService
    {
        public SunAccount GetCustomer(string accountCode)
        {
            OdooCredentials creds = new OdooCredentials();
            OdooApi api = new OdooApi(creds, serverCertificateValidation: false);
            OdooModel sunAccountModel = api.GetModel<SunAccount>();
        
            List<OdooRecord> records = sunAccountModel.Search(new object[]{new object[]{"sun_account_no","=",accountCode}});

            var account = records.FirstOrDefault()?.GetEntity<SunAccount>();
            return account;

        }

        public Task<SunAccount> GetCustomerAsync(string accountCode)
        {
            return Task.Factory.StartNew(() => GetCustomer(accountCode));
        }
    }
}