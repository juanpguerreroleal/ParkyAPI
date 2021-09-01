using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.DataAccess.Repository.IRepository;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    /// <summary>
    /// Trail controller
    /// </summary>
    //[Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/trails")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "Trailsv1")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork">Container of repositories</param>
        /// <param name="mapper">Mapper entity</param>
        public TrailsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of all the trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TrailDto>))]
        public IActionResult GetTrails()
        {
            var objList = _unitOfWork.TrailRepository.GetAll();

            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual trail
        /// </summary>
        /// <param name="trailId">The id of the trail</param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        [Authorize(Roles ="Admin")]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _unitOfWork.TrailRepository.GetById(trailId);
            if (obj == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<TrailDto>(obj);
            return Ok(dto);
        }
        /// <summary>
        /// Get individual trail
        /// </summary>
        /// <param name="trailId">The id of the trail</param>
        /// <returns></returns>
        [HttpGet("[action]/{nationalParkId:int}")]
        [ProducesResponseType(200, Type = typeof(TrailDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrailInNationalPark(int nationalParkId)
        {
            var objList = _unitOfWork.TrailRepository.GetAll(x => x.NationalParkId == nationalParkId);
            if (objList == null)
            {
                return NotFound();
            }
            var objDto = new List<TrailDto>();
            foreach (var obj in objList)
            {
                var dto = _mapper.Map<TrailDto>(obj);
                objDto.Add(dto);
            }
            return Ok(objDto);
        }

        /// <summary>
        /// Create trail
        /// </summary>
        /// <param name="trailDto">The model of the trail to create</param>
        /// <returns></returns>
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
            if (_unitOfWork.TrailRepository.Exists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists");
                return StatusCode(404, ModelState);
            }

            var model = _mapper.Map<Trail>(trailDto);
            _unitOfWork.TrailRepository.Add(model);
            if (!_unitOfWork.SaveChanges())
            {
                ModelState.AddModelError("", $"Something went wrong saving the record {model.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetTrail", new { trailId = model.Id }, model);
        }
        /// <summary>
        /// Update trail
        /// </summary>
        /// <param name="trailId">The id of the trail to update</param>
        /// <param name="trailDto">The updated model of the trail</param>
        /// <returns></returns>
        [HttpPatch("{trailId:int}", Name ="UpdateTrail")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateTrail(int trailId, [FromBody] TrailUpdateDto trailDto)
        {
            if (trailDto == null || trailId != trailDto.Id)
            {
                return BadRequest(ModelState);
            }
            var model = _mapper.Map<Trail>(trailDto);
            _unitOfWork.TrailRepository.Update(model);
            if (!_unitOfWork.SaveChanges())
            {
                ModelState.AddModelError("", $"Something went wrong updating the record {model.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        /// <summary>
        /// Delete trail
        /// </summary>
        /// <param name="trailId">The id of the trail to delete</param>
        /// <returns></returns>
        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_unitOfWork.TrailRepository.Exists(trailId))
            {
                return NotFound();
            }
            var model = _unitOfWork.TrailRepository.GetById(trailId);
            _unitOfWork.TrailRepository.Delete(model);
            if (!_unitOfWork.SaveChanges())
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {model.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
