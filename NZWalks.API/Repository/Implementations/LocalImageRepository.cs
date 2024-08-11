﻿using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repository.Interfaces;

namespace NZWalks.API.Repository.Implementations
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext dbContext;

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment , IHttpContextAccessor httpContextAccessor , NZWalksDbContext dbContext )
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }
        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images" , $"{image.FileName}{image.FileExtension}");
            using var steam = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(steam);

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            await dbContext.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }
    }
}