using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookingManagementSystem.DTOs;
using Core.Models;
using Infrastructure.Interfaces.IUnitOfWork;
using Microsoft.AspNetCore.Mvc;


namespace BookingManagementSystem.Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class TrainController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TrainController(IUnitOfWork unitOfWork, IMapper mapper) {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnTrainDTO>> GetTrainById(int id)
        {
            try {
              
                var train = await _unitOfWork.Trains.GetByIdAsync(id);

                if (train == null)
                {
                    return NotFound();
                }


                return  _mapper.Map<ReturnTrainDTO>(train);
            
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnTrainDTO>>> GetAllTrainsAsync()
        {
            
            try
            {
                var trains = await _unitOfWork.Trains.GetAllAsync();

                return _mapper.Map<List<ReturnTrainDTO>>(trains);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ReturnTrainDTO>>> AddNewTrain([FromBody] AddTrainDTO addTrainDTO)
        {

            try
            {

                //modelState check

                var train = _mapper.Map<Train>(addTrainDTO);

                _unitOfWork.Trains.InsertAsync(train);
                
                var result = await _unitOfWork.Save();

                if (result <= 0)
                    return BadRequest();

                return Ok("added successfully!");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }




    }
}
