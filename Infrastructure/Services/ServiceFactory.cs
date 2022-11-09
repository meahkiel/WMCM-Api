using Application.DTO;
using Application.Interface;
using Infrastructure.External.Web;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly UnitWrapper context;
        private readonly IPhotoAccessor photoAccessor;
        private readonly IWebPostService webPostService;

        public ServiceFactory(UnitWrapper context,
            IPhotoAccessor photoAccessor,
            IWebPostService webPostService)
        {
            this.context = context;
            this.photoAccessor = photoAccessor;
            this.webPostService = webPostService;
        }

        public IActivityServiceAccessor GetWebPost()
        {
            return new WebPostActivityService(context, webPostService, photoAccessor);
        }
    }
}
