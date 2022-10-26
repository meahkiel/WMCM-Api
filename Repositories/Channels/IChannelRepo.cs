using Core.Channels;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Channels
{
    public interface IChannelRepo : IRepositoryBase<ChannelSetting>
    {
        Task<IEnumerable<ChannelSetting>> GetChannels();
        Task<ChannelSetting> FindByIdAsync(string id);
        Task<ChannelSetting> FindByTypeAsync(string type);
    }
}
