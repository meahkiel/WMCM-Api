using Application.SeedWorks;
using Core.Campaigns;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IExternalService
    {
        Task<ServiceMessageBroker> Execute(Activity activity);
    }
}
