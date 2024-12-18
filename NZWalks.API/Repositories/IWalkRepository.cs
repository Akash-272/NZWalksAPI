﻿using NZWalks.API.Model.Domain;
using NZWalks.API.Model.Domain.DTO;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync(string? filterOn=null,string? filterQuery=null);

        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> UpdateAsync(Guid id,Walk walk);

        Task<Walk?> DeleteAsync(Guid id);
    }
}
