using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;
using NZWalks.API.Model.Domain.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null)
        {
            // Start with the base query including related entities
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            // Apply filter if both filterOn and filterQuery are provided
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                // Apply filtering based on the filterOn value
                walks = filterOn.ToLower() switch
                {
                    "difficulty" => walks.Where(w => w.Difficulty.Name.Contains(filterQuery)),
                    "region" => walks.Where(w => w.Region.Name.Contains(filterQuery)),
                    _ => walks
                };
            }

            // Execute the query and return the results as a list
            return await walks.ToListAsync();
        }


        //public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null)
        //{
        //    var walk = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
        //    //if (string.IsNullOrWhiteSpace)

        //    return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();

        //}

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existingWalk=await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if(existingWalk==null)
            {
                return null;
            }
            dbContext.Walks.Remove(existingWalk);
            await dbContext.SaveChangesAsync();
            return existingWalk;

        }

        Task<Walk?> IWalkRepository.GetByIdAsync(Guid id)
        {
            return dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        async Task<Walk?> IWalkRepository.UpdateAsync(Guid id, Walk walk)
        {
            var walkDomainModel=await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walkDomainModel == null)
            {
                return null;
            }
            walkDomainModel.Name= walk.Name;
            walkDomainModel.Description= walk.Description;
            walkDomainModel.LengthInKm= walk.LengthInKm;
            walkDomainModel.WalkImageUrl= walk.WalkImageUrl;
            walkDomainModel.DifficultyId= walk.DifficultyId;
            walkDomainModel.RegionId= walk.RegionId;
            await dbContext.SaveChangesAsync();

            return walkDomainModel;

        }
    }
}
