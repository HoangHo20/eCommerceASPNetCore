using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CustomerMVCSite.Options;
using CustomerMVCSite.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CustomerMVCSite.Services
{
    public class LocalUploadService : IUploadService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly LocalUploadOptions _localUploadOptions;
        private string wwwRootPath;

        public LocalUploadService(IWebHostEnvironment hostEnvironment, IOptionsMonitor<LocalUploadOptions> localUploadOptions)
        {
            _hostEnvironment = hostEnvironment;
            _localUploadOptions = localUploadOptions.CurrentValue;
            wwwRootPath = _hostEnvironment.WebRootPath;
        }

        public async Task<List<string>> UploadImage(IFormFile imgFile)
        {
            List<string> imgUrls = new List<string>();

            string imgFileName = Path.GetFileNameWithoutExtension(imgFile.FileName);
            imgFileName = imgFileName.Length < 10 ?
                    imgFileName.Substring(0, imgFileName.Length - 1) :
                    imgFileName.Substring(0, 10);

            string imgFileExtension = Path.GetExtension(imgFile.FileName);

            string currentTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            imgFileName = $"{imgFileName}_{currentTimeStamp}{imgFileExtension}";

            string saveFolderPath = $"{ wwwRootPath }/{ _localUploadOptions.GeneralFolder }/";
            string savePath = Path.Combine(saveFolderPath, imgFileName);

            // Create General Folder if not exist
            System.IO.Directory.CreateDirectory(saveFolderPath);

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await imgFile.CopyToAsync(fileStream);

                string relativePath = $"{_localUploadOptions.GeneralFolder}/{imgFileName}";

                imgUrls.Add(relativePath);
            }

            return imgUrls;
        }

        public async Task<List<string>> UploadImage(List<IFormFile> imageFiles)
        {
            if (imageFiles.Count < 1)
            {
                return null;
            }

            List<string> imageFilesUrls = new List<string>();

            foreach (IFormFile imgFile in imageFiles)
            {
                try
                {
                    List<string> uploadedUrls = await UploadImage(imgFile);

                    if (uploadedUrls != null && uploadedUrls.Count > 0)
                    {
                        foreach (string url in uploadedUrls)
                        {
                            imageFilesUrls.Add(url);
                        }
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine("UploadImage:" + ex);
                }
            }

            return imageFilesUrls;
        }
    }
}
