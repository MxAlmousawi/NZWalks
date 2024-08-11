using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository.Implementations;
using NZWalks.API.Repository.Interfaces;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionController> logger;

        public RegionController(NZWalksDbContext dbContext , IRegionRepository regionRepository , IMapper mapper , ILogger<RegionController> logger)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }


        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetALL() {
            logger.LogInformation("GetAllRegions Action Method was invoked");
            var regionsDomain = await regionRepository.GetAllAsync();
            /*var regionDto = new List<RegionDto>();
            foreach (var region in regionsDomain)
            {
                regionDto.Add(new RegionDto() {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }*/
            logger.LogInformation($"Finished GetAllRegions with data : {JsonSerializer.Serialize(regionsDomain)}");
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }


        [HttpGet]
        //[Authorize(Roles = "Reader")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var regionDomain =await regionRepository.GetByIdAsync(id);
            if(regionDomain == null){
                return NotFound();
            }
            /*var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };*/
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }


        [HttpPost]
        //[Authorize(Roles = "Writer")]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            
            /*var regionDomain = new Region()
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl=addRegionRequestDto.RegionImageUrl,
            };*/

            var regionDomain = mapper.Map<Region>(addRegionRequestDto);

            await regionRepository.CreateAsync(regionDomain);
            /*var regionDto = new RegionDto()
            {
                Id=regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };*/

            var regionDto = mapper.Map<RegionDto>(regionDomain);

            return CreatedAtAction(nameof(GetById), new {id=regionDomain.Id} , regionDto);
        }


        [HttpPut]
        //[Authorize(Roles = "Writer")]
        [ValidateModel]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute]Guid id , [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            
            var regionDomain = await regionRepository.UpdateAsync(id, updateRegionRequestDto);
            if (regionDomain == null)
            {
                return NotFound();
            }
            
            /*var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };*/

          
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }


        [HttpDelete]
        //[Authorize(Roles = "Writer")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var regionDomain = await regionRepository.DeleteAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            /*var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };*/

            return Ok(mapper.Map<RegionDto>(regionDomain));
        }
    }
}
