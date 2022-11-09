using Core.Notifications;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Notifications
{
    public interface INotificationRepo : IRepositoryBase<Notification>
    {
        Task<IEnumerable<Notification>> GetAll(string module);

    }
    public class NotificationRepo : INotificationRepo
    {
        private readonly DataContext _context;

        public NotificationRepo(DataContext context)
        {
            _context = context;
        }
        public void Add(Notification entity)
        {
            _context.Notifications.Add(entity);
        }

        public async Task<IEnumerable<Notification>> GetAll(string module)
        {
            return await _context.Notifications
                    .Where(n => n.Module == module)
                    .ToListAsync();
        }

        public void Remove(Notification entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Notification entity)
        {
            throw new NotImplementedException();
        }
    }
}
