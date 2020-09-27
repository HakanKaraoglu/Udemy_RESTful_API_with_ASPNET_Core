using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using ParkyAPI.Models.Repository.IRepository;
using System.Collections.Generic;

namespace ParkyAPI.Controllers
{
    //[Route("api/Trails")]
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "ParkOpenAPISpecTrails")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : ControllerBase
    {
        private ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public TrailsController(ITrailRepository trailRepository, IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of national parks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200,Type =typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var objList = _trailRepository.GetTrails();
            var objDtos = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDtos.Add(_mapper.Map<TrailDto>(obj));
            }
            return Ok(objDtos);
        }

        /// <summary>
        /// Get individual national park
        /// </summary>
        /// <param name="nationalParkId"></param> 
        /// <returns></returns>
        [HttpGet("[action]/{nationalParkId:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var objList = _trailRepository.GetTrailsInNationalPark(nationalParkId);
            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }
            
            return Ok(objDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        
        public IActionResult CreateTrail([FromBody] TrailCreateDto trailDto)
        {
            if (trailDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_trailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists");
                return StatusCode(404, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var TrailObj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepository.CreateTrail(TrailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {TrailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail",new { TrailId=TrailObj.Id},TrailObj);
        }

        [HttpPatch("{TrailId:int}",Name ="UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId,[FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || trailId != trailDto.Id)
            {
                return BadRequest(ModelState);
            }

            var TrailObj = _mapper.Map<Trail>(trailDto);
            if (!_trailRepository.UpdateTrail(TrailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {TrailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{TrailId:int}",Name ="DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int TrailId)
        {
            if (_trailRepository.TrailExists(TrailId))
            {
                return NotFound();
            }

            var TrailObj = _trailRepository.GetTrail(TrailId);
            if (!_trailRepository.DeleteTrail(TrailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when delete the record {TrailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
