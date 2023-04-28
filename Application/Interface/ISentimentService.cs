using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ISentimentService
    {
        Task<object> GetSentiment(string text,string filename);
    }
}
