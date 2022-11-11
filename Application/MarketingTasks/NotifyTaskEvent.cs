using Application.Interface;
using Core.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.MarketingTasks
{
    public class NotifyTaskEvent : INotification
    {
        public string Description { get; set; }
        public string Module { get; set; }
        public string AssignedTo { get; set; }

    }

    public class NotifyTaskEventHandler : INotificationHandler<NotifyTaskEvent>
    {
        private readonly UnitWrapper _context;
        private readonly IUserAccessorService _userAccessorService;

        public NotifyTaskEventHandler(UnitWrapper context,IUserAccessorService userAccessorService)
        {
            _context = context;
            _userAccessorService = userAccessorService;
        }

        public async Task Handle(NotifyTaskEvent notification, CancellationToken cancellationToken)
        {
            var user = await _context.GetContext().Users.FirstOrDefaultAsync(u => u.UserName ==
                        notification.AssignedTo);


            _context.Notifications.Add(Notification.Create(
                description: notification.Description,
                module: notification.Module,
                userId: user.Id.ToString()
            ));

            await _context.SaveChangesAsync();
        }
    }
}
