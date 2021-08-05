using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyAPI.DataAccess.Repository.IRepository;
using ParkyAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NationalParksController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
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
        [HttpGet("{nationalParkId:int}")]
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
    }
}
