using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository.Interfaces;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        public ImagesController(IImageRepository imageRepository)
        {
            ImageRepository = imageRepository;
        }

        public IImageRepository ImageRepository { get; }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageDomainModel = new Image
            {
                File = request.File,
                FileExtension = Path.GetExtension(request.File.FileName),
                FileSizeInBytes = request.File.Length,
                FileName = request.FileName,
                FileDescription = request.FileDescription,
            };

            await ImageRepository.Upload(imageDomainModel);

            return Ok(imageDomainModel);
        }



        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg" , ".jpeg" , ".png"};
            if(!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file" , "Unsupported file extension");
            }
            if(request.File.Length > 10*1024*1024)
            {
                ModelState.AddModelError("file", "File size can't be more than 10mb");
            }
        }
    }
}
