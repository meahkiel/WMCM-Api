using Core.Notifications;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Notifications
{
    public interface INotificationRepo : IRepositoryBase<Notification>
    {
        Task<IEnumerable<Notification>> GetAllUnread(string userId);

        Task<IEnumerable<Notification>> GetUserNotifications(string userId);

        Task<Notification> GetUnread(string notificaitonId);
        
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

        public async Task<IEnumerable<Notification>> GetUserNotifications(string userId)
        {
            return await _context.Notifications
                            .Where(x => x.UserId == userId)
                            .ToListAsync();

        }

        public async Task<IEnumerable<Notification>> GetAllUnread(string userId)
        {
            return await _context.Notifications
                    .Where(n => n.UserId == userId && n.HasRead == false)
                    .ToListAsync();
        }

        public async Task<Notification> GetUnread(string notificaitonId)
        {
            return await _context
                            .Notifications
                            .FirstOrDefaultAsync(n => n.Id.ToString() == notificaitonId);
        }

        public void Remove(Notification entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Notification entity)
        {
            _context.Notifications.Update(entity);
        }
    }
}
