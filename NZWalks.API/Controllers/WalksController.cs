﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository.Implementations;
using NZWalks.API.Repository.Interfaces;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var walk = mapper.Map<Walk>(addWalkRequestDto);
            await walkRepository.CreateAsync(walk);
            return Ok(mapper.Map<WalkDto>(walk));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterOn, 
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool? isAscending,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10
        )
        {
            var walks = await walkRepository.GetAllAsync(filterOn , filterQuery ,sortBy , isAscending ?? true , pageNumber , pageSize);
            return Ok(mapper.Map<List<WalkDto>>(walks));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walk =await walkRepository.GetByIdAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walk));
        }

        [HttpPut]
        [ValidateModel]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id , [FromBody] UpdateWalkRequestDto walkDto)
        {
            
            var walk = await walkRepository.UpdateAsync(id, walkDto);
            if(walk == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walk));
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walk = await walkRepository.DeleteAsync(id);
            if(walk == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(walk));
        }
    }
}
