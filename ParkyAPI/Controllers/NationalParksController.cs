using AutoMapper;
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
    /// National Park controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class NationalParksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork">Container of repositories</param>
        /// <param name="mapper">Mapper entity</param>
        public NationalParksController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of all the national parks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<NationalParkDto>))]
        public IActionResult GetNationalParks()
        {
            var objList = _unitOfWork.NationalParkRepository.GetAll();

            var objDto = new List<NationalParkDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<NationalParkDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual national park
        /// </summary>
        /// <param name="nationalParkId">The id of the national park</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        [ProducesResponseType(200, Type = typeof(NationalParkDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _unitOfWork.NationalParkRepository.GetById(nationalParkId);
            if (obj == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<NationalParkDto>(obj);
            return Ok(dto);
        }
        /// <summary>
        /// Create national park
        /// </summary>
        /// <param name="nationalParkDto">The model of the national park to create</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(NationalParkDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateNationalPark([FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }
            if (_unitOfWork.NationalParkRepository.Exists(nationalParkDto.Name))
            {
                ModelState.AddModelError("", "National Park Exists");
                return StatusCode(404, ModelState);
            }

            var model = _mapper.Map<NationalPark>(nationalParkDto);
            _unitOfWork.NationalParkRepository.Add(model);
            if (!_unitOfWork.SaveChanges())
            {
                ModelState.AddModelError("", $"Something went wrong saving the record {model.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = model.Id }, model);
        }
        /// <summary>
        /// Update national park
        /// </summary>
        /// <param name="nationalParkId">The id of the national park to update</param>
        /// <param name="nationalParkDto">The updated model of the national park</param>
        /// <returns></returns>
        [HttpPatch("{nationalParkId:int}", Name ="UpdateNationalPark")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDto nationalParkDto)
        {
            if (nationalParkDto == null || nationalParkId != nationalParkDto.Id)
            {
                return BadRequest(ModelState);
            }
            var model = _mapper.Map<NationalPark>(nationalParkDto);
            _unitOfWork.NationalParkRepository.Update(model);
            if (!_unitOfWork.SaveChanges())
            {
                ModelState.AddModelError("", $"Something went wrong updating the record {model.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
        /// <summary>
        /// Delete national park
        /// </summary>
        /// <param name="nationalParkId">The id of the national park to delete</param>
        /// <returns></returns>
        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            if (!_unitOfWork.NationalParkRepository.Exists(nationalParkId))
            {
                return NotFound();
            }
            var model = _unitOfWork.NationalParkRepository.GetById(nationalParkId);
            _unitOfWork.NationalParkRepository.Delete(model);
            if (!_unitOfWork.SaveChanges())
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {model.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
