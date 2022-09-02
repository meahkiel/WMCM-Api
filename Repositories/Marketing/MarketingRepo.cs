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

        public MarketingRepo(DataContext context)
        {
            _context = context;
        }
        public void Add(MarketingTask entity)
        {
            _context.Add(entity);
        }

        public async Task<IEnumerable<MarketingTask>> GetListByUser(string userName)
        {
            return await _context.MarketingTasks
                        .Where(t => t.Close == false)
                        .Where(t => t.UserName == userName)
                        .Include(t => t.SubTasks)
                        .ToListAsync();
        }

        public async Task<MarketingTask> GetSubTask(Guid taskId)
        {
            return await _context.MarketingTasks
                            .Include(t => t.SubTasks)
                            .ThenInclude(s => s.Comments)
                            .SingleAsync(c => c.Id == taskId);
        }

        public void Remove(MarketingTask entity)
        {
            throw new NotImplementedException();
        }

        public void Update(MarketingTask entity)
        {
            throw new NotImplementedException();
        }
    }
}
