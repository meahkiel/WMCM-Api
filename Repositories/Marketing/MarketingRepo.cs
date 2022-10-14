using Core.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Marketing
{
    public class MarketingRepo : IMarketingRepo
    {
        private readonly DataContext _context;

        public IQueryable<MarketingTask> MarketingTasks => _context.MarketingTasks;

        public MarketingRepo(DataContext context)
        {
            _context = context;
        }
        public void Add(MarketingTask entity)
        {
            _context.MarketingTasks.Add(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MarketingTask>> GetListByUser(string userName, bool isHigherUser)
        {

            if(isHigherUser)
            {

                return await _context.MarketingTasks
                            .Where(t => t.Close == false)
                            .Where(t => t.UserName == userName)
                            .Select(m => new MarketingTask
                            {
                                Title = m.Title,
                                Close = m.Close,
                                Id = m.Id,
                                UserName = m.UserName,
                                SubTasks = m.SubTasks
                                                .Select(s => new SubTask {
                                                    Task = s.Task,
                                                    Id = s.Id,
                                                    Status = s.Status,
                                                    AssignedTo = s.AssignedTo, 
                                                    AssignedBy = s.AssignedBy }).ToList()
                            })
                            .ToListAsync();
            }
            else
            {
                return await _context.MarketingTasks
                            .Where(t => t.Close == false)
                            .Where(t => t.SubTasks.Any(s => s.AssignedTo == userName))
                            .Select(m => new MarketingTask
                            {
                                Title = m.Title,
                                Close = m.Close,
                                Id = m.Id,
                                UserName = m.UserName,
                                SubTasks = m.SubTasks
                                                .Select(s => new SubTask
                                                {
                                                    Task = s.Task,
                                                    Id = s.Id,
                                                    Status= s.Status,   
                                                    AssignedTo = s.AssignedTo,
                                                    AssignedBy = s.AssignedBy
                                                }).ToList()
                            })
                            .ToListAsync();
            }


        }

        public async Task<MarketingTask> GetSubTask(Guid taskId)
        {
            return await _context.MarketingTasks
                            .Include(t => t.SubTasks)
                            .SingleAsync(c => c.Id == taskId);
        }

        public async Task<MarketingTask> GetSubTask(Guid taskId, string userName)
        {
            var tasks = await _context.MarketingTasks
                            .Include(t => t.SubTasks.Where(s => s.AssignedTo == userName))
                            .ThenInclude(s => s.Comments)
                            .Where(c => c.Id == taskId) 
                            .FirstOrDefaultAsync();

            return tasks;

        }

        public void Remove(MarketingTask entity)
        {
            throw new NotImplementedException();
        }

        public void Update(MarketingTask entity)
        {
            _context.MarketingTasks.Update(entity);
        }
        
        public async Task<bool> CreateUpdateSubTask(Guid taskId, IList<SubTask> subTasks)
        {

            var marketing = await _context.MarketingTasks
                                .Include(s => s.SubTasks)
                                .ThenInclude(s => s.Comments)
                                .Where(m => m.Id == taskId)
                                .SingleOrDefaultAsync();
            
            if(marketing == null) return false;
            
            if (subTasks != null && subTasks.Count() > 0) {
                foreach (var subTask in subTasks) 
                {
                    var existingSubTask = marketing.SubTasks
                                            .Where(s => s.Id == subTask.Id)
                                            .FirstOrDefault();
                    
                    if(existingSubTask == null)
                    {
                        subTask.Id = Guid.NewGuid();
                        subTask.MarketingTask = marketing;
                        subTask.UpdateStatus(StatusEnum.Todo);
                        
                        _context.Add(subTask).State = EntityState.Added;
                    }
                    else {
                        if (subTask.Comments != null && subTask.Comments.Count > 0)
                        {
                            foreach (var comment in subTask.Comments)
                            {
                                existingSubTask.Comments.Add(comment);
                            }
                        }

                        StatusEnum status = (StatusEnum)Enum.Parse(typeof(StatusEnum), subTask.Status);
                        existingSubTask.UpdateStatus(status);
                        
                        _context.Update(marketing);
                    }
                }
            }

            return true;
        }

        public void AddSubTask(SubTask subTask)
        {
            _context.SubTasks.Add(subTask);
        }

        public  void DeleteUpdateSubTask(SubTask subtask)
        {
            _context.SubTasks.Remove(subtask);
        }

        public void UpdateSubTask(SubTask subTask)
        {
            _context.SubTasks.Update(subTask);
        }
    }
}
