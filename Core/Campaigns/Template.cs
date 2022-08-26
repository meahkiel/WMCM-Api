using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Campaigns
{
    public class Template
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TemplateUrl { get; set; }
        public string TemplateHtml { get; set; }
        public string Type { get; set; }

    }
}
