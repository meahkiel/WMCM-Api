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


        /// <summary>
        /// Get all the Task of the specific user. if no task will result to null value. If no value will result to NullExecption
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>
        /// Task result contains IEnumrable that contains marketingtask element 
        /// </returns>
        Task<IEnumerable<MarketingTask>> GetListByUser(string userName,bool isHigherUser);

        /// <summary>
        /// Return a specific task with Subtask element 
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        Task<MarketingTask> GetSubTask(Guid taskId);
        Task<MarketingTask> GetSubTask(Guid taskId, string userName);

        Task<bool> CreateUpdateSubTask(Guid taskId,IList<SubTask> subTasks);
        

        

    }
}
