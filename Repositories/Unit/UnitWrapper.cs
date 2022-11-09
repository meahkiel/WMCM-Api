using Persistence.Context;
using Repositories.Campaigns;
using Repositories.Channels;
using Repositories.Customer;
using Repositories.Marketing;
using Repositories.Notifications;
using Repositories.Templates;
using System;
using System.Threading.Tasks;

namespace Repositories.Unit
{
    public class UnitWrapper : IDisposable
    {

        private IMarketingRepo _marketing;
        private ICustomerRepo _customer;
        private ICampaignRepo _campaign;
        private ITemplateRepo _template;
        private IChannelRepo _channel;
        private INotificationRepo _notification;


        private readonly DataContext _context;

        public UnitWrapper(DataContext context)
        {
            _context = context;

        }

        public IMarketingRepo Marketings { 
            get { 
                if(_marketing == null)
                {
                    _marketing = new MarketingRepo(_context);
                }
                return _marketing;
            } 
        }

        public ICustomerRepo Customers
        {
            get
            {
                if (_customer == null)
                {
                    _customer = new CustomerRepo(_context);
                }
                return _customer;
            }
        }

        public ICampaignRepo CampaignRepo
        {
            get
            {
                if (_campaign == null)
                {
                    _campaign = new CampaignRepo(_context);
                }
                return _campaign;
            }
        }

        public ITemplateRepo TemplateRepo
        {
            get
            {
                if (_template == null)
                {
                    _template = new TemplateRepo(_context);
                }
                return _template;
            }
        }

        public IChannelRepo Channels
        {
            get
            {
                if (_channel == null)
                {
                    _channel = new ChannelRepo(_context);
                }
                return _channel;
            }
        }

        public DataContext GetContext()
        {
            return _context;
        }

        public INotificationRepo Notifications
        {
            get
            {
                if (_notification == null)
                {
                    _notification = new NotificationRepo(_context);
                }
                return _notification;
            }
        }


        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
