﻿using Application.Interface;
using Application.SeedWorks;
using Core.Notifications;
using Microsoft.EntityFrameworkCore;

namespace Application.Notifications
{
    public class GetUnreadNotification
    {

        public class Query : IRequest<Result<IEnumerable<Notification>>>
        {
           
        }

        public class QueryHandler : IRequestHandler<Query, Result<IEnumerable<Notification>>>
        {
            private readonly UnitWrapper _wrapper;
            private readonly IUserAccessorService userAccessor;

            public QueryHandler(UnitWrapper wrapper,IUserAccessorService userAccessor)
            {
                _wrapper = wrapper;
                this.userAccessor = userAccessor;
            }

           
            public async  Task<Result<IEnumerable<Notification>>> Handle(Query request, CancellationToken cancellationToken)
            {

                try
                {
                    var currentUser = await _wrapper.GetContext()
                                                   .Users
                                                   .Where(u => u.UserName == userAccessor.GetUsername())
                                                   .FirstOrDefaultAsync();

                    var unreadNotifications = await _wrapper.Notifications
                                                .GetAllUnread(currentUser.Id);


                    return Result<IEnumerable<Notification>>.Success(unreadNotifications);
                }
                catch (Exception ex)
                {

                    return Result<IEnumerable<Notification>>.Failure(ex.Message);
                }
               
            }
        }

    }
}
