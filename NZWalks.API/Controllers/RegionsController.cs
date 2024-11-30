using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Model.Domain;
using NZWalks.API.Model.Domain.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // https://localhost:1234/api/region
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController (NZWalksDbContext dbContext,IRegionRepository regionRepository,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        public NZWalksDbContext DbContext { get; }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            //var regionDomains = await dbContext.Regions.ToListAsync();
            var regionDomains= await regionRepository.GetAllAsync();

            //var regionDto = new List<RegionDto>();
            //foreach (var regionDomain in regionDomains)
            //{
            //    regionDto.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl

            //    });
            //}
            //var regionDto=mapper.Map<List<RegionDto>>(regionDomains);
            return Ok(mapper.Map<List<RegionDto>>(regionDomains));
        }

        //Get Region by RegionId
        // https://localhost:1234/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            //var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            var regionDomain = await regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //var regionDto = new RegionDto()
            //{
            //    Id=regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl
            //};
            //var regionDomains = dbContext.Regions.ToList();

            //var regionDto = new List<RegionDto>();
            //foreach (var regionDomain in regionDomains)
            //{
            //    regionDto.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl

            //    });
            //}
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDtos)
        {
            if (ModelState.IsValid) 
            {
                //Map or Convert Dto to Model

                var regionDomainModel = mapper.Map<Region>(addRegionRequestDtos);

                regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

                //Map Domain Model back to Dto
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);


                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            }
            return BadRequest(ModelState);
            

        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Retrieve the region from the database
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
            //var regionDomainModel = new Region
            //{
            //    Code = updateRegionRequestDto.Code,
            //    Name = updateRegionRequestDto.Name,
            //    RegionImageUrl= updateRegionRequestDto.RegionImageUrl

            //};

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }
            

            // Map DTO to Domain Model
            //regionDomainModel.Code = updateRegionRequestDto.Code;
            //regionDomainModel.Name = updateRegionRequestDto.Name;
            //regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            //// Save changes to the database
            //await dbContext.SaveChangesAsync();

            // Convert Domain Model to DTO
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            // Return the updated region as a response
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel== null)
            {
                return NotFound();
            }
            //dbContext.Regions.Remove(regionDomainModel);
            //await dbContext.SaveChangesAsync();

            //var regionDto = new RegionDto
            //{
            //    Id = regionDomainModel.Id,
            //    Code = regionDomainModel.Code,
            //    Name = regionDomainModel.Name,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl
            //};

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }


    }
}
