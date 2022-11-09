using Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.External.Web
{
    public interface IWebPostService
    {
        public string BaseUrl { get; set; }

        Task<bool> Post(PostParameter postParameter);
        
    }
}
