using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerMVCSite.Services.Interface
{
    public interface IUploadService
    {
        public Task<List<string>> UploadImage(IFormFile imgFile);

        public Task<List<string>> UploadImage(List<IFormFile> imageFiles);
    }
}
