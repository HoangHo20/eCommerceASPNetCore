using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CustomerMVCSite.Options;
using CustomerMVCSite.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly CloudinaryOptions _cloudinaryOptions;
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptionsMonitor<CloudinaryOptions> cloudinaryOptions)
        {
            _cloudinaryOptions = cloudinaryOptions.CurrentValue;

            CloudinaryDotNet.Account account = 
                new CloudinaryDotNet.Account(
                    _cloudinaryOptions.CloudName,
                    _cloudinaryOptions.APIKey,
                    _cloudinaryOptions.APISecret);

            _cloudinary = new CloudinaryDotNet.Cloudinary(account);
        }

        public async Task<List<string>> UploadImage(IFormFile imgFile)
        {
            List<string> imageURLs = new List<string>();
            ImageUploadParams uploadParams = new ImageUploadParams();

            using (var memoryStream = new MemoryStream())
            {
                await imgFile.CopyToAsync(memoryStream);

                memoryStream.Position = 0;

                uploadParams.File = new FileDescription(imgFile.FileName, memoryStream);
                uploadParams.EagerTransforms = new List<Transformation>
                {
                    new EagerTransformation().Width(200).Height(150).Crop("scale"),
                    new EagerTransformation().Width(500).Height(200).Crop("scale")
                };
                uploadParams.Folder = _cloudinaryOptions.GeneralFolder;
                ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

                string url = uploadResult.SecureUrl.ToString();
                imageURLs.Add(url);
            }

            return imageURLs;
        }

        public async Task<List<string>> UploadImage(List<IFormFile> imageFiles)
        {
            if (imageFiles.Count < 1)
            {
                return null;
            }

            List<string> imageURLs = new List<string>();

            foreach (IFormFile imgFile in imageFiles)
            {
                ImageUploadParams uploadParams = new ImageUploadParams();

                using (var memoryStream = new MemoryStream())
                {
                    await imgFile.CopyToAsync(memoryStream);

                    memoryStream.Position = 0;

                    uploadParams.File = new FileDescription(imgFile.FileName, memoryStream);
                    uploadParams.Folder = _cloudinaryOptions.GeneralFolder;
                    ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    string url = uploadResult.SecureUrl.ToString();
                    imageURLs.Add(url);
                }
            }

            return imageURLs;
        }
    }
}
