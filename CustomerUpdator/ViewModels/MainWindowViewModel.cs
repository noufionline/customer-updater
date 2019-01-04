using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using PostSharp.Patterns.Model;
using Prism.Mvvm;
using System.Data.Entity;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Input;
using CookComputing.XmlRpc;
using PostSharp.Patterns.Xaml;

namespace CustomerUpdator.ViewModels
{

    [NotifyPropertyChanged]
    public class MainWindowViewModel : BindableBase
    {
        private readonly IMapper _mapper;
        private readonly List<SunSystemCustomer> _sunCustomerCollection;
        private readonly List<SunAccount> _odooSunAccounts;

        public MainWindowViewModel(IMapper mapper)
        {
            _mapper = mapper;
            using (var db = new AbsContext())
            {
                Partners = new ObservableCollection<PartnerModel>(db.Partners
                    .Include(x=> x.PartnerAccountCodes)
                    .Where(x => x.SunAccountCode == null && x.TaxRegistrationNo != null && !new[] { "111111111111111", "000000000000000"}.Contains(x.TaxRegistrationNo))
                    .ProjectTo<PartnerModel>(_mapper.ConfigurationProvider).ToList());


                _sunCustomerCollection = db.SunSystemCustomers.ToList();
                _sunCustomerCollection.ForEach(x=> x.VatCode=x.VatCode.Replace(" ",""));


                _odooSunAccounts = GetSunAccounts();
            }
        }


        public List<SunAccount> GetSunAccounts() 
        {

            OdooCredentials creds = new OdooCredentials();
            OdooApi api = new OdooApi(creds, serverCertificateValidation: false);
            OdooModel sunAccountModel = api.GetModel<SunAccount>();
            List<SunAccount> sunAccounts = new List<SunAccount>();

            List<OdooRecord> records = sunAccountModel.SearchAll();

            foreach (var record in records)
            {
                var account = record.GetEntity<SunAccount>();
                sunAccounts.Add(account);
            }

            return sunAccounts;
        }


        public ObservableCollection<PartnerModel> Partners { get; set; }

        public ObservableCollection<SunSystemCustomer> SunCustomers { get; set; } = new ObservableCollection<SunSystemCustomer>();
        public ObservableCollection<SunAccount> OdooCustomers { get; set; } = new ObservableCollection<SunAccount>();
            

        #region SelectCommand

        [Command] public ICommand SelectCommand { get; set; }
        public string SearchText { get;set; }


        private void ExecuteSelect(PartnerModel partner)
        {
            try
            {
                var name = partner.Name;
                var startIndex = name.IndexOf(" ", StringComparison.InvariantCulture) + 1;
                SearchText = name.Substring(0, name.IndexOf(" ",startIndex, StringComparison.InvariantCulture));
                SunCustomers.Clear();
                SunCustomers = new ObservableCollection<SunSystemCustomer>(_sunCustomerCollection
                    .Where(x => x.VatCode.Trim() == partner.TaxRegistrationNo.Trim()).ToList());

                var sunCodes = SunCustomers.Select(x => x.Code.Trim()).ToArray();
                OdooCustomers = new ObservableCollection<SunAccount>(_odooSunAccounts.Where(x => sunCodes.Contains(x.SunAccountCode)).ToList());

                foreach (var customer in SunCustomers)
                {
                    if (OdooCustomers.Any(x => x.SunAccountCode == customer.Code.Trim() && x.IsProject))
                    {
                        customer.IsProject = true;
                    }
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                //throw;
            }
        }


        protected bool CanExecuteSelect(PartnerModel partner) => partner!=null;

        #endregion
    }


    



[OdooModelName("cic.sun.account")]
public class SunAccount
{
	[OdooFieldName("partner_id")]
	public int PartnerId { get; set; }

	[OdooFieldName("partner_id", index: 1)]
	public string PartnerName { get; set; }

	[OdooFieldName("project_id")]
	public int ProjectId { get; set; }

	[OdooFieldName("project_id", index: 1)]
	public string ProjectName { get; set; }
	[OdooFieldName("sun_account_no")]
	public string SunAccountCode { get; set; }

	//	[OdooFieldName("old_sun_account_no")]
	//	public string OldSunAccountCode { get; set; }

	[OdooFieldName("sun_db")]
	public string SunDb { get; set; }

	public bool IsProject => ProjectId > 0;
}

[OdooModelName("project.project")]
public class Project
{
	[OdooFieldName("id")]
	public int Id { get; set; }
	[OdooFieldName("partner_id")]
	public int PartnerId { get; set; }
	[OdooFieldName("name")]
	public string Name { get; set; }
}

[OdooModelName("res.partner")]
public class OdooPartner
{
	[OdooFieldName("id")]
	public int Id { get; set; }


	[OdooFieldName("name")]
	public string Name { get; set; }
}

[XmlRpcUrl("object")]
public interface IOdooObjectRpc : IXmlRpcProxy
{
	[XmlRpcMethod("execute")]
	int Create(string database, int userId, string password, string model, string method, XmlRpcStruct fieldValues);

	[XmlRpcMethod("execute")]
	int[] Search(string database, int userId, string password, string model, string method, object[] filter);

	[XmlRpcMethod("execute")]
	bool Write(string database, int userId, string password, string model, string method, int[] ids, XmlRpcStruct fieldValues);

	[XmlRpcMethod("execute")]
	bool Unlink(string database, int userId, string password, string model, string method, int[] ids);

	[XmlRpcMethod("execute")]
	object[] Read(string database, int userId, string password, string model, string method, int[] ids, object[] fields);

	[XmlRpcMethod("exec_workflow")]
	bool ExecuteWorkflow(string dbName, int userId, string password, string model, string action, int ids);

	[XmlRpcMethod("execute")]
	ValidationInfo ValidatePartner(string dbName, int userId, string pwd, string model, string method, string sunAccountCode, string sunDb);

	[XmlRpcMethod("execute")]
	SunAccountInfo[] ValidateTrn(string dbName, int userId, string pwd, string model, string method,
		string trn);

	[XmlRpcMethod("execute")]
	TaxCodeWithAccountName[] ValidateAccountCode(string dbName, int userId, string pwd, string model, string method,
		string sunAccountCode);
}

public class SunAccountInfo
{
	[XmlRpcMember("ACCNT_CODE")]
	public string AccountCode { get; set; }
	[XmlRpcMember("ACCNT_NAME")]
	public string AccountName { get; set; }


}

[XmlRpcUrl("common")]
public interface IOdooCommonRpc : IXmlRpcProxy
{
	[XmlRpcMethod("login")]
	int Login(string database, string userName, string password);
}

public class OdooModel
{
	private readonly string _modelName;
	private readonly OdooApi _api;
	private readonly List<string> _fields = new List<string>();

	public OdooModel(string modelName, OdooApi api)
	{
		_api = api;
		_modelName = modelName;
	}

	public List<OdooRecord> SearchAll()
	{
		return Search(new object[] { });
	}
	public List<OdooRecord> Search(object[] filter)
	{
		int[] ids = _api.Search(_modelName, filter ?? new object[] { });
		return Search(ids);
	}


	public List<OdooRecord> Search(int[] ids)
	{
		List<OdooRecord> records = new List<OdooRecord>();
		object[] result = _api.Read(_modelName, ids, _fields.ToArray());

		foreach (object entry in result)
		{
			XmlRpcStruct vals = (XmlRpcStruct)entry;

			int id = (int)vals["id"];
			OdooRecord record = new OdooRecord(_api, _modelName, id);

			foreach (string field in _fields)
			{
				record.SetValue(field, vals[field]);
			}

			records.Add(record);
		}

		return records;
	}

	public void AddField(string field)
	{
		if (!_fields.Contains(field))
		{
			_fields.Add(field);
		}
	}

	public void AddFields(List<string> fields)
	{
		foreach (string field in fields)
		{
			AddField(field);
		}
	}

	public void AddFields(params string[] fields)
	{
		foreach (string field in fields)
		{
			AddField(field);
		}
	}

	public void Remove(List<OdooRecord> records)
	{
		int[] toRemove = records
								.Where(r => r.Id >= 0)
								.Select(r => r.Id)
								.ToArray();

		_api.Remove(_modelName, toRemove);
	}

	public void Remove(OdooRecord record)
	{
		Remove(new List<OdooRecord> { record });
	}

	public void Save(List<OdooRecord> records)
	{
		foreach (OdooRecord record in records)
		{
			record.Save();
		}
	}

	public void Save(OdooRecord record)
	{
		Save(new List<OdooRecord> { record });
	}

	public OdooRecord CreateNew()
	{
		return new OdooRecord(_api, _modelName, -1);
	}
}

public class OdooRecord
{
	private readonly OdooApi _api;
	private readonly string _model;
	private readonly Dictionary<string, object> _fields = new Dictionary<string, object>();
	private readonly List<string> _modifiedFields = new List<string>();

	public OdooRecord(OdooApi api, string model, int id)
	{
		_model = model;
		_api = api;
		Id = id;
	}

	public bool SetValue(string field, object value)
	{
		if (_fields.ContainsKey(field))
		{
			if (!_modifiedFields.Contains(field))
			{
				_modifiedFields.Add(field);
			}

			_fields[field] = value;
		}
		else
		{
			_fields.Add(field, value);
		}
		return true;
	}

	public object GetValue(string field)
	{
		if (_fields.ContainsKey(field))
		{
			return _fields[field];
		}
		else
		{
			return null;
		}
	}


	public TEntity GetEntity<TEntity>() where TEntity : class, new()
	{
		var entity = new TEntity();
		PropertyInfo[] props = typeof(TEntity).GetProperties();
		foreach (PropertyInfo prop in props)
		{
			object[] attrs = prop.GetCustomAttributes(true);
			foreach (object attr in attrs)
			{
				if (attr is OdooFieldNameAttribute authAttr)
				{
					string auth = authAttr.Name;
					if (_fields.ContainsKey(auth))
					{
						object value = _fields[auth];
						SetValue(prop, entity, value, index: authAttr.Index, type: authAttr.Type);
					}

				}
			}
		}

		return entity;
	}

	void SetValue(PropertyInfo info, object instance, object value, int index = 0, Type type = null)
	{
		if (value is object[] obj)
		{
			if (type == null)
			{
				if (((object[])value).Any())
				{
					info.SetValue(instance,
						index == 0
							? Convert.ChangeType(obj[index], typeof(int))
							: Convert.ChangeType(obj[index], typeof(string)));
				}
			}
			else
			{
				info.SetValue(instance, Convert.ChangeType(obj[index], type));
			}
		}
		else
		{
			info.SetValue(instance, Convert.ChangeType(value, info.PropertyType));
		}
	}


	public T GetValue<T>(string field)
	{

		if (_fields.ContainsKey(field))
		{
			object value = _fields[field];

			if (value.GetType() == typeof(T))
				return (T)value;
			if (typeof(T).GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
				return (T)value;
			return (T)Convert.ChangeType(value, typeof(T));
		}

		return default(T);


	}


	public bool TryGetValue<T>(string field, out T value)
	{
		if (_fields.ContainsKey(field))
		{
			object result = _fields[field];
			if (result.GetType() == typeof(T))
				value = (T)result;
			else if (typeof(T).GetTypeInfo().IsAssignableFrom(result.GetType().GetTypeInfo()))
				value = (T)result;
			else if (result is object[] && !((object[])result).Any())
			{
				value = default(T);
				return false;
			}
			else
			{
				value = (T)Convert.ChangeType(result, typeof(T));
			}

			return true;

		}
		else
		{
			value = default(T);
			return false;
		}




	}

	public int Id { get; private set; }

	public void Save()
	{
		XmlRpcStruct values = new XmlRpcStruct();

		if (Id >= 0)
		{
			foreach (string field in _modifiedFields)
			{
				values[field] = _fields[field];
			}

			_api.Write(_model, new int[1] { Id }, values);
		}
		else
		{
			foreach (string field in _fields.Keys)
			{
				values[field] = _fields[field];
			}

			Id = _api.Create(_model, values);
		}
	}
}

public class OdooFieldNameAttribute : Attribute
{
	public OdooFieldNameAttribute(string name, int index = 0, Type type = null)
	{
		Name = name;
		Index = index;
		Type = type;
	}
	public string Name { get; set; }
	public int Index { get; }
	public Type Type { get; }
}


public class OdooModelNameAttribute : Attribute
{
	public string Name { get; }

	public OdooModelNameAttribute(string name)
	{
		Name = name;
	}
}
public class OdooApi
{
	private readonly OdooCredentials _credentials;
	private readonly WebProxy _networkProxy;
	private readonly bool _serverCertificateValidation;
	private IOdooObjectRpc _objectRpc;

	public OdooApi(OdooCredentials credentials, bool immediateLogin = true, WebProxy networkProxy = null, bool serverCertificateValidation = true)
	{
		_serverCertificateValidation = serverCertificateValidation;
		_networkProxy = networkProxy;
		_credentials = credentials;

		if (immediateLogin)
		{
			Login();
		}
	}

	public bool Login()
	{
		IOdooCommonRpc loginRpc = XmlRpcProxyGen.Create<IOdooCommonRpc>();
		loginRpc.Url = _credentials.CommonUrl;

		if (_networkProxy != null)
		{
			loginRpc.Proxy = _networkProxy;
		}

		if (_serverCertificateValidation)
		{
			ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
		}

		int userId = loginRpc.Login(_credentials.Database, _credentials.Username, _credentials.Password);

		_credentials.UserId = userId;


		_objectRpc = XmlRpcProxyGen.Create<IOdooObjectRpc>();
		_objectRpc.Url = _credentials.ObjectUrl;

		return true;
	}

	private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
	{
		return true;
	}

	public int Create(string model, XmlRpcStruct fieldValues)
	{
		return _objectRpc.Create(_credentials.Database, _credentials.UserId, _credentials.Password, model, "create", fieldValues);
	}

	public int[] Search(string model, object[] filter)
	{
		return _objectRpc.Search(_credentials.Database, _credentials.UserId, _credentials.Password, model, "search", filter);
	}

	public object[] Read(string model, int[] ids, string[] fields)
	{
		return _objectRpc.Read(_credentials.Database, _credentials.UserId, _credentials.Password, model, "read", ids, fields);
	}

	public bool Write(string model, int[] ids, XmlRpcStruct fieldValues)
	{
		return _objectRpc.Write(_credentials.Database, _credentials.UserId, _credentials.Password, model, "write", ids, fieldValues);
	}

	public bool Remove(string model, int[] ids)
	{
		return _objectRpc.Unlink(_credentials.Database, _credentials.UserId, _credentials.Password, model, "unlink", ids);
	}

	public bool Execute_Workflow(string model, string action, int id)
	{
		return _objectRpc.ExecuteWorkflow(_credentials.Database, _credentials.UserId, _credentials.Password, model, action, id);
	}



	public ValidationInfo ValidatePartner(string model, string method, string accountCode, string db = "CAD")
	{
		ValidationInfo result = _objectRpc.ValidatePartner(_credentials.Database,
			_credentials.UserId, _credentials.Password,
			model, method, accountCode, db);
		return result;
	}


	public TaxCodeWithAccountName ValidateAccountCode(string accountCode)
	{
		TaxCodeWithAccountName[] result = _objectRpc.ValidateAccountCode(_credentials.Database,
			_credentials.UserId, _credentials.Password,
			"cic.rating.category", "validate_sun_account_no", accountCode);

		return result.Length == 0 ? null : result.First();
	}


	public List<SunAccountInfo> GetSunAccountDetails(string trn)
	{
		var result = _objectRpc.ValidateTrn(_credentials.Database,
			_credentials.UserId, _credentials.Password,
			"cic.rating.category", "validate_trn", trn);
		return result.ToList();
	}
	public OdooModel GetModel(string model)
	{
		return new OdooModel(model, this);
	}

	public OdooModel GetModel<T>(bool populateFields = true) where T : class
	{
		Attribute attribute = typeof(T).GetCustomAttribute(typeof(OdooModelNameAttribute));
		if (attribute is OdooModelNameAttribute modelNameAttribute)
		{
			OdooModel model = GetModel(modelNameAttribute.Name);
			if (populateFields)
			{
				model.AddFields(GetOdooFields(typeof(T)));
			}

			return model;
		}

		return null;
	}

	private string[] GetOdooFields(Type entity)
	{
		PropertyInfo[] props = entity.GetProperties();
		var list = new List<string>();
		foreach (PropertyInfo prop in props)
		{
			object[] attrs = prop.GetCustomAttributes(true);
			foreach (object attr in attrs)
			{
				if (attr is OdooFieldNameAttribute authAttr)
				{
					string auth = authAttr.Name;
					list.Add(auth);

				}
			}
		}

		return list.ToArray();
	}
}

public class OdooCredentials
{

	private string _commonUrl = "common";
	private string _objectUrl = "object";

	//	public OdooCredentials() : this(
	//		"https://cicononline.com:10443",
	//		"CICON-DB")
	//	{
	//	}


	public OdooCredentials() : this(
		"http://cicononline.com:83",
		"CICON_NEW")
	{
	}


	public OdooCredentials(string server, string database)
	{
		Server = server;
		Database = database;
		Username = "noufal";
		Password = "MtpsF42";
	}

	public string Server { get; }
	public string Database { get; }
	public string Username { get; }
	public string Password { get; }
	public int UserId { get; set; }

	public string CommonUrl => $"{Server}/xmlrpc/{_commonUrl}";

	public string ObjectUrl => $"{Server}/xmlrpc/{_objectUrl}";
}
public class ValidationInfo
{
	// Fields
	[XmlRpcMember("allow_transaction")]
	public bool AllowTransaction;
	[XmlRpcMember("partner_id")]
	public int CustomerId;
	[XmlRpcMember("partner_name")]
	public string CustomerName;
	[XmlRpcMember("description")]
	public string Description;
	[XmlRpcMember("invalid_sun_code")]
	public bool InvalidSunAccountCode;
	[XmlRpcMember("is_project")]
	public bool IsProject;
	[XmlRpcMember("project_id")]
	public int ProjectId;
	[XmlRpcMember("sun_account_name")]
	public string ProjectName;
	[XmlRpcMember("rating_category")]
	public string RatingCategory;

	[XmlRpcMember("trn")]
	public string TaxRegistrationNo { get; set; }

	public override string ToString() =>
		$"Rating :({this.RatingCategory}) Descriptoin:({this.Description}) Allow Transaction:({this.AllowTransaction})";

}

public class TaxCodeWithAccountName
{
	[XmlRpcMember("TAX_CODE")]
	public string TaxRegistationNo { get; set; }
	[XmlRpcMember("ACCNT_NAME")]
	public string AccountName { get; set; }


}
}
