using Persistence.Context;
using Repositories.Campaigns;
using Repositories.Customer;
using Repositories.Marketing;
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

        private readonly DataContext _context;

        public UnitWrapper(DataContext context)
        {
            _context = context;

        }

        public IMarketingRepo MarketingRepo { 
            get { 
                if(_marketing == null)
                {
                    _marketing = new MarketingRepo(_context);
                }
                return _marketing;
            } 
        }

        public ICustomerRepo CustomerRepo
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
