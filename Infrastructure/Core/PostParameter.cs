using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core
{
    public class PostParameter
    {
        public string SectionName { get; set; } = "";

        public string Title { get; set; } = "";

        public string HtmlBody { get; set; } = "";

        public string BackgroundImage { get; set; } = "";
    }
}
