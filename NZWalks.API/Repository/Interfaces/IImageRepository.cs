using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository.Interfaces
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
