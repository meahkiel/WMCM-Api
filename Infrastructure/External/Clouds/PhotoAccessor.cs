using Application.Core;
using Application.Interface;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Infrastructure.External.Credential;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.External.Clouds
{
    public class PhotoAccessor : IPhotoAccessor
    {
       
        private readonly Cloudinary _cloudinary;

        public PhotoAccessor(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                 config.Value.CloudName,
                 config.Value.ApiKey,
                 config.Value.ApiSecret);

            _cloudinary = new Cloudinary(account);
        }


        public async Task<PhotoUplodResult> AddPhoto(IFormFile file)
        {

            if(file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream)
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                return new PhotoUplodResult
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.SecureUrl.ToString()
                };
            }

            return null;
            
        }

        public Task<string> DeletePhoto(string publicId)
        {
            throw new NotImplementedException();
        }
    }
}
