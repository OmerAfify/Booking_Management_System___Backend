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
    public class ScheduleController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ScheduleController(IUnitOfWork unitOfWork, IMapper mapper) {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

     
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleDTO>> GetScheduleById(int id)
        {
            try {
              
               var schedule = await _unitOfWork.Schedules.FindAsync(i => i.Id == id, new List<string> { "Route", "Train" });
          
                if (schedule == null)
                    return NotFound(new ApiResponse(404,$"Schedule id : {id} is not found."));
                

                return  _mapper.Map<ScheduleDTO>(schedule);
            
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500,
                                ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
            }

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetAllSchedules()
        {
            
            try
            {
                var schedules = await _unitOfWork.Schedules.FindRangeAsync(null, new List<string> { "Route", "Train" }, q=>q.OrderByDescending(q=>q.Id));

                return _mapper.Map<List<ScheduleDTO>>(schedules);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500, 
                                ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
            }

        }


        [HttpPost]
        public async Task<IActionResult> AddNewSchedule([FromBody] AddScheduleDTO addScheduleDTO)
        {
            try
            {
                //Validations
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var train = await _unitOfWork.Trains.GetByIdAsync(addScheduleDTO.TrainId);
                if (train == null) return BadRequest("train Id not found");

                var route = await _unitOfWork.Routes.GetByIdAsync(addScheduleDTO.RouteId);
                if (route == null) return BadRequest("route Id not found");

                var schedule = await _unitOfWork.Schedules.FindAsync(s => s.RouteId == addScheduleDTO.RouteId &&
                                                               s.TrainId == addScheduleDTO.TrainId && s.Date == addScheduleDTO.Date);
                if (schedule != null)
                    return BadRequest(new ApiResponse(400, "This Schedule already exists. Please Change either the Date, Route, or Train."));


                //Insertion        
                var newSchedule = _mapper.Map<Schedule>(addScheduleDTO);

                newSchedule.FirstClassAvailableBookings = train.FirstClassSeats;
                newSchedule.SecondClassAvailableBookings = train.SecondClassSeats;

                _unitOfWork.Schedules.InsertAsync(newSchedule);

                var result = await _unitOfWork.Save();

                return (result >= 1) ? Ok() : BadRequest(new ApiResponse(400));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500,
                                ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
            }

        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSchedule(int id, [FromBody] AddScheduleDTO addScheduleDTO)
        {

            try
            {
                //Validations
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var scheduleOld = await _unitOfWork.Schedules.GetByIdAsync(id);
                if(scheduleOld ==null) return BadRequest("Schedule Id not found");


                var train = await _unitOfWork.Trains.GetByIdAsync(addScheduleDTO.TrainId);
                if (train == null) return BadRequest("train Id not found");

                var route = await _unitOfWork.Routes.GetByIdAsync(addScheduleDTO.RouteId);
                if (route == null) return BadRequest("route Id not found");

                var schedule = await _unitOfWork.Schedules.FindAsync(s => s.RouteId == addScheduleDTO.RouteId &&
                                                               s.TrainId == addScheduleDTO.TrainId && s.Date == addScheduleDTO.Date);
                if (schedule != null)
                    return BadRequest(new ApiResponse(400, "This Schedule already exists. Please Change either the Date, Route, or Train."));


                //Updating
          
                 _mapper.Map(addScheduleDTO, scheduleOld);

                scheduleOld.FirstClassAvailableBookings = train.FirstClassSeats;
                scheduleOld.SecondClassAvailableBookings = train.SecondClassSeats;

                 _unitOfWork.Schedules.Update(scheduleOld);

                var result = await _unitOfWork.Save();

                return (result >= 1) ? Ok() : BadRequest(new ApiResponse(400));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500,
                                ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
            }

     

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSchedule(int id)
        {
            try
            {
                if (id <= 0) return BadRequest(new ApiResponse(400, "Id can not be 0 or less."));

               var schedule = await _unitOfWork.Schedules.GetByIdAsync(id);

                if (schedule == null)
                    return NotFound(new ApiResponse(404));
       
                _unitOfWork.Schedules.Delete(schedule);

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
