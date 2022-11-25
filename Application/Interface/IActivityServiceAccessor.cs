using Application.DTO;
using Core.Campaigns;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IActivityServiceAccessor<TInput,TOutput>
    {

        Task<TOutput> ForExecute(TInput dto);
        Task<Activity> Execute(TInput dto);

        bool CanExecute(TInput dto);
    }
}
