using Core.Tasks;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Marketing
{
    public interface IMarketingRepo : IRepositoryBase<MarketingTask>
    {
        Task<IEnumerable<MarketingTask>> GetListByUser(string userName);
        Task<MarketingTask> GetSubTask(Guid taskId);

    }
}
