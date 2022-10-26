using Core.Channels;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Channels
{
    public class ChannelRepo : IChannelRepo
    {

        private readonly DataContext _context;

        public ChannelRepo(DataContext context)
        {
            _context = context;
        }

        public void Add(ChannelSetting entity)
        {
            _context.ChannelSettings.Add(entity);
        }

        public async Task<ChannelSetting> FindByIdAsync(string id)
        {
            var channel = await _context.ChannelSettings.FindAsync(Guid.Parse(id));
            if (channel == null)
                throw new Exception("Channel cannot find");
            
            return channel;
        }

        public async Task<ChannelSetting> FindByTypeAsync(string type)
        {
            var channel = await _context.ChannelSettings.FirstOrDefaultAsync(c => c.Type == type);
            if (channel == null)
                throw new Exception("Channel cannot find");

            return channel;
        }

        public async Task<IEnumerable<ChannelSetting>> GetChannels()
        {
            return await _context.ChannelSettings
                                    .ToListAsync();
        }

        public void Remove(ChannelSetting entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ChannelSetting entity)
        {
            _context.ChannelSettings.Update(entity);
        }
    }
}
