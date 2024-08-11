using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repository.Interfaces
{
    public interface IRegionRepository
    {
        public Task<List<Region>> GetAllAsync();
        public Task<Region?> GetByIdAsync(Guid id);
        public Task<Region?> CreateAsync(Region region);
        public Task<Region?> UpdateAsync(Guid id , UpdateRegionRequestDto region);
        public Task<Region?> DeleteAsync(Guid id);
    }
}
