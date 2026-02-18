using CloPosProject.Application.Abstract.ICloudinary;
using CloPosProject.Application.DTOs.Cloudinary;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace CloPosProject.Infrastructure.Concurate.Cloudinary
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly IConfiguration _configuration;
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;
        private readonly CloudinaryOptionsDto cloudinaryOptionsDto;
        public CloudinaryService(IConfiguration configuration)
        {
            _configuration = configuration;
            cloudinaryOptionsDto=_configuration.GetSection("CloudinarySettings").Get<CloudinaryOptionsDto>() ?? new();
            var account = new Account(
                cloud: cloudinaryOptionsDto.CloudName,
                apiKey: cloudinaryOptionsDto.ApiKey,
                apiSecret: cloudinaryOptionsDto.ApiSecret

                ); 
            _cloudinary = new CloudinaryDotNet.Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }
        public async Task<string> FileCreateAsync(IFormFile file)
        {
            string filename = string.Concat(Guid.NewGuid(), file.FileName.Substring(file.FileName.LastIndexOf(".")));
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return ("Yalnız şəkil faylları yüklənə bilər (jpg, jpeg, png, gif, webp)");

            if (file.Length > 5 * 1024 * 1024)
                return ("Şəkil ölçüsü 5MB-dan çox ola bilməz");

            var uploadresult = new ImageUploadResult();
            using var stream=file.OpenReadStream();
            var uploadparams = new ImageUploadParams
            {
                File = new FileDescription(filename, stream),
                Folder = "CloPos"
            };
            uploadresult=await _cloudinary.UploadAsync(uploadparams);
            string url = uploadresult.SecureUrl.ToString();
            return url;
        }

        public async Task<bool> FileDeleteAsync(string filePath)
        {
            try
            {
                string publicIdWithExtension = filePath.Substring(filePath.LastIndexOf("motordoctor.az"));
                string publicId = publicIdWithExtension.Substring(0, publicIdWithExtension.LastIndexOf('.'));

                var deleteParams = new DelResParams()
                {
                    PublicIds = new List<string> { publicId },
                    Type = "upload",
                    ResourceType = ResourceType.Image
                };
                var result = await _cloudinary.DeleteResourcesAsync(deleteParams);

                return result.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
