﻿using Core.Campaigns;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<IEnumerable<Campaign>> GetActiveCampaigns()
        {
            return  await _context.Campaigns.ToListAsync();
        }

        public async Task<Campaign> GetSingleCampaign(Guid id)
        {
            return await _context.Campaigns
                    .Include(c => c.Activities)
                    .FirstOrDefaultAsync(c => c.Id == id);
        }

        public void Remove(Campaign entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Campaign entity)
        {
            _context.Update(entity);
        }
    }
}