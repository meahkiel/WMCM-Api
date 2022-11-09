using Application.Core;
using Application.DTO;
using Application.Interface;
using Core.Campaigns;
using Infrastructure.External.Clouds;
using Infrastructure.External.Web;
using Repositories.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class WebPostActivityService : IActivityServiceAccessor
    {
        private readonly UnitWrapper _wrapper;
        private readonly IWebPostService _webPostService;
        private readonly IPhotoAccessor _photoAccessor;

        public WebPostActivityService(
            UnitWrapper wrapper,
            IWebPostService webPostService,
            IPhotoAccessor photoAccessor)
        {
            _wrapper = wrapper;
            _webPostService = webPostService;
            _photoAccessor = photoAccessor;
        }
        public async Task<Activity> Execute(ActivityEntryDTO dto)
        {

            PhotoUplodResult result = new PhotoUplodResult();
            if(dto.CoverImageFile != null)
            {
                result = await _photoAccessor.AddPhoto(dto.CoverImageFile);

            }
            
            
            var setting = await _wrapper.Channels.FindByTypeAsync("web");
            if (setting == null)
                throw new Exception("Cannot find channel");
            
            _webPostService.BaseUrl = setting.BaseUrl;
            
            await _webPostService.Post(new Core.PostParameter {
                BackgroundImage = result.Url == null ? "" : result.Url,
                HtmlBody = dto.Body,
                SectionName = dto.To,
                Title = dto.Title
            });

            return new Activity
            {
                CoverImage = result.Url ?? "",
                Type = dto.Type,
                To = dto.To,
                Body = dto.Body,
                Title = dto.Title,
                DispatchDate = DateTime.Today,
                DateSchedule = DateTime.Today,
                Status = "completed"
            };
        }
    }
}
