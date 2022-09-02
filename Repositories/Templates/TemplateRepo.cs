using Core.Campaigns;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Templates
{
    public class TemplateRepo : ITemplateRepo
    {
        private DataContext _context;
        public TemplateRepo(DataContext context)
        {
            _context = context;
        }
        public void Add(Template entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Template>> GetAllTemplatesAsync(string type)
        {
            return await _context.Templates.Where(t => t.Type == type).ToListAsync();
        }

        public void Remove(Template entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Template entity)
        {
            throw new NotImplementedException();
        }
    }
}
