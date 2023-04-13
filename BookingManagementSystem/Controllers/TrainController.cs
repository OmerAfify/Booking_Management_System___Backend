using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookingManagementSystem.DTOs;
using BookingManagementSystem.Errors;
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
                    return NotFound(new ApiResponse(404,$"train id : {id} is not found."));
                

                return  _mapper.Map<ReturnTrainDTO>(train);
            
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500,
                                ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
            }

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnTrainDTO>>> GetAllTrainsAsync()
        {
            
            try
            {
                var trains = await _unitOfWork.Trains.GetAllAsync(null,q=>q.OrderByDescending(q=>q.Id));

                return _mapper.Map<List<ReturnTrainDTO>>(trains);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500, 
                                ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
            }

        }

        [HttpPost]
        public async Task<ActionResult> AddNewTrain([FromBody] AddTrainDTO addTrainDTO)
        {

            try
            {
                if (!ModelState.IsValid)  return BadRequest(ModelState);


                _unitOfWork.Trains.InsertAsync(_mapper.Map<Train>(addTrainDTO));
                
                var result = await _unitOfWork.Save();

                return (result >= 1 ) ? Ok() : BadRequest(new ApiResponse(400));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500,
                                ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
            }

        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTrain(int id, [FromBody] AddTrainDTO addTrainDTO)
        {
            try
            {
                if (id <= 0) return BadRequest(new ApiResponse(400, "Id can not be 0 or less."));

                if (!ModelState.IsValid)  return BadRequest(ModelState);


                var train = await _unitOfWork.Trains.GetByIdAsync(id);

                if (train == null)
                    return NotFound(new ApiResponse(404));

                _mapper.Map(addTrainDTO, train);

                _unitOfWork.Trains.Update(train);

                var result = await _unitOfWork.Save();

                return (result >= 1) ? Ok($"train of id : {id} is updated successfully!") : BadRequest(new ApiResponse(400));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500,
                                ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
            }

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrain(int id)
        {
            try
            {
                if (id <= 0) return BadRequest(new ApiResponse(400, "Id can not be 0 or less."));

               var train = await _unitOfWork.Trains.GetByIdAsync(id);

                if (train == null)
                    return NotFound(new ApiResponse(404));
       
                _unitOfWork.Trains.Delete(train);

                var result = await _unitOfWork.Save();

                return (result >= 1) ? NoContent() : BadRequest(new ApiResponse(400));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500,
                                ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
            }

        }


    }
}
