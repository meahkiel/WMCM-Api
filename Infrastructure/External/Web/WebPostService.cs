
using Infrastructure.Core;
using Repositories.Unit;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Infrastructure.External.Web
{
    public class WebPostService : IWebPostService
    {

        
        private readonly UnitWrapper _context;

        public WebPostService(
            UnitWrapper context)
        {
           
            _context = context;
        }

        public string BaseUrl { get; set; }


        public async Task<bool> Post(PostParameter postParameter) {
            try
            {
                var url = BaseUrl;
                var client = new RestClient(url);
                var request = new RestRequest("campaign", Method.Post);
               
                request.AddBody(postParameter);

                var response = await client.PostAsync(request);

                return true;
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}
