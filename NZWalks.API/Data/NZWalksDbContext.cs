using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext: DbContext
    {
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions ): base( dbContextOptions ) {}
        
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed data

            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("e718c795-1405-4a29-bd70-ead243c8ae41"),
                    Name = ""
                },new Difficulty()
                {
                    Id = Guid.Parse("2359a0be-6e42-495f-b50b-dc4e623ccdb2"),
                    Name = ""
                },new Difficulty()
                {
                    Id = Guid.Parse("9ebaf802-a467-41ea-9ec7-ba3f2a9ed229"),
                    Name = ""
                },
            };
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            var regions = new List<Region>()
            {
                new Region()
                {
                    Id = Guid.Parse("ba6553c1-0b0d-4875-9e7c-67dbb16fc80e"),
                    Name = "Region a",
                    Code = "AAA",
                    RegionImageUrl = "image.com"
                },
                new Region()
                {
                    Id = Guid.Parse("e4db930c-6717-4284-bbf1-d8323e52c09c"),
                    Name = "Region b",
                    Code = "BBB",
                    RegionImageUrl = "image.com"
                },
                new Region()
                {
                    Id = Guid.Parse("5be09984-07e4-47c7-a09c-27cd5f326dbb"),
                    Name = "Region c",
                    Code = "CCC",
                    RegionImageUrl = "image.com"
                }
            };
            modelBuilder.Entity<Region>().HasData(regions);
        }

    }
}
