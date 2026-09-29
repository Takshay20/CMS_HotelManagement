using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CMS_HotelBooking.Helpers
{
    public static class FileUploadHelper
    {
        public static async Task<string> SaveAsync(IFormFile file, IWebHostEnvironment environment, string subFolder)
        {
            string folderPath = Path.Combine(environment.WebRootPath, "uploads", subFolder);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(folderPath, fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("uploads", subFolder, fileName).Replace("\\", "/");
        }
    }
}
