using System;

namespace Infrastructure.Core
{
    public class PostParameter
    {
   
        public string SectionName { get; set; } = "";

        public string Title { get; set; } = "";

        public string HtmlBody { get; set; } = "";

        public string BackgroundImage { get; set; } = "";

        public DateTime LaunchDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
