using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository.Interfaces;
using System.Linq;

namespace NZWalks.API.Repository.Implementations
{
    public class SqlWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SqlWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk?> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walk = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            if (walk == null)
            {
                return null;
            }
            dbContext.Walks.Remove(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllAsync(
            string? filterOn = null,
            string? filterQuery = null ,
            string? sortBy = null ,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 10
        )
        {
            var query = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false) { 
                if (filterOn.Equals("Name" , StringComparison.OrdinalIgnoreCase))     
                {
                    query = query.Where(x => x.Name.Contains(filterQuery));
                }
            }
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isAscending ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    query = isAscending ? query.OrderBy(x => x.LengthInKm) : query.OrderByDescending(x => x.LengthInKm);
                }
            }

            var skipResults = (pageNumber-1)*pageSize;

            return await query.Skip(skipResults).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var walk = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
            if(walk == null)
            {
                return null;
            }
            return walk;
        }

        public async Task<Walk?> UpdateAsync(Guid id, UpdateWalkRequestDto walkDto)
        {
            var walk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walk == null)
            {
                return null;
            }
            walk.DifficultyId = walkDto.DifficultyId;
            walk.RegionId = walkDto.RegionId;
            walk.WalkImageUrl = walkDto.WalkImageUrl;
            walk.LengthInKm = walkDto.LengthInKm;
            walk.Description = walkDto.Description;
            walk.Name = walkDto.Name;
            
            await dbContext.SaveChangesAsync();
            return walk;
        }
    }
}
