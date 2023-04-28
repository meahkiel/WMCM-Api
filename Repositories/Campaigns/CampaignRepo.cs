using Core.Campaigns;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Repositories.Campaigns
{
    public class CampaignRepo : ICampaignRepo
    {

        private readonly DataContext _context;
        
        public CampaignRepo(DataContext context)
        {
            _context = context;
            
        }
        public void Add(Campaign entity)
        {
            _context.Campaigns.Add(entity);
        }

        public void AddActivity(Activity activity)
        {
            _context.Activities.Add(activity);
        }

       

        public void DeleteActivity(Activity activity)
        {
            _context.Activities.Remove(activity);
        }

        public async Task<IEnumerable<Campaign>> GetActiveCampaigns()
        {
            return await _context.Campaigns
                .Include(c => c.Activities)
                //.Where(c => c.DateFrom <= DateTime.Now && c.DateTo >= DateTime.Now)
                .ToListAsync();
        }

        public async Task<Activity?> GetActivityByType(string type)
        {
            return await _context.Campaigns.Include(c => c.Activities)
                            .Where(c => c.DateTo >= DateTime.UtcNow)
                            .SelectMany(c => c.Activities)
                            .Where(a => a.Type == type)
                            .FirstOrDefaultAsync();
        }

        public async Task<Activity> GetByActivity(string activityId)
        {

            return await _context.Activities
                            .Where(a => a.Id == Guid.Parse(activityId))
                            .FirstOrDefaultAsync();
        }

        public async Task<Campaign> GetSingleCampaign(Guid id)
        {
            return await _context.Campaigns
                    .Include(c => c.Activities)
                    .FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Remove(Campaign entity)
        {
            _context.Campaigns.Remove(entity);
        }

        public void Update(Campaign entity)
        {
            _context.Update(entity);
        }

        public void UpdateActivity(Activity activity)
        {
            _context.Activities.Update(activity);
        }

       
    }
}
