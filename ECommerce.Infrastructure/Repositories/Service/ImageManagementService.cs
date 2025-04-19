using ECommerce.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace ECommerce.Infrastructure.Repositories.Service
{
    public class ImageManagementService : IImageManagementService
    {
        private readonly IFileProvider fileProvider;
        public ImageManagementService(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }
        public async Task<List<string>> AddImageAsync(IFormFileCollection files, string src)
        {
            var saveImageSrc=new List<string>();
            var imageDircetory = Path.Combine("wwwroot", "Images", src);
            if (Directory.Exists(imageDircetory) is not true) 
            {
                Directory.CreateDirectory(imageDircetory);
            }
            foreach (var file in files)
            {
                if(file.Length>0)
                {
                    var imageName = file.FileName;
                    var imageSrc = $"Images/{src}/{imageName}";

                    var root=Path.Combine(imageDircetory, imageName);   

                    using (FileStream fileStream=new FileStream(root,FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    saveImageSrc.Add(imageSrc);

                }
            }
            return saveImageSrc;
        }

        public void DeleteImageAsync(string src)
        {
            var info=fileProvider.GetFileInfo(src);
            var root = info.PhysicalPath;
            File.Delete(root);  
            
        }


    }
}
