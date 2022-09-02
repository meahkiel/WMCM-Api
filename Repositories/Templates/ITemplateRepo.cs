using Core.Campaigns;
using Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Templates
{
    public interface ITemplateRepo : IRepositoryBase<Template>
    {
        Task<IEnumerable<Template>> GetAllTemplatesAsync(string type);
    }
}
