using Prism.Events;

namespace CustomerUpdator.ViewModels
{
    public class SunAccountEvent:PubSubEvent<(int PartnerId,string PartnerName,int? ProjectId, string ProjectName,string AccountCode)>
    {
    }
}