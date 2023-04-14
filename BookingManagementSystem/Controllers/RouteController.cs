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
    public class RouteController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RouteController(IUnitOfWork unitOfWork, IMapper mapper) {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

     
        
        [HttpGet("{id}")]
        public async Task<ActionResult<RouteDTO>> GetRouteById(int id)
        {
            try {
              
                var route = await _unitOfWork.Routes.GetByIdAsync(id);

                if (route == null)
                    return NotFound(new ApiResponse(404,$"route id : {id} is not found."));
                

                return  _mapper.Map<RouteDTO>(route);
            
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500,
                                ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
            }

        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<RouteDTO>>> GetAllRoutes()
        {
            
            try
            {
                var routes = await _unitOfWork.Routes.GetAllAsync(null,q=>q.OrderByDescending(q=>q.Id));

                return _mapper.Map<List<RouteDTO>>(routes);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500, 
                                ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
            }

        }

        [HttpPost]
        public async Task<ActionResult> AddNewRoute([FromBody] AddRouteDTO addRouteDTO)
        {

            try
            {
                if (!ModelState.IsValid)  return BadRequest(ModelState);


                _unitOfWork.Routes.InsertAsync(_mapper.Map<Route>(addRouteDTO));
                
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
        public async Task<ActionResult> UpdateRoute(int id, [FromBody] AddRouteDTO addRouteDTO)
        {
            try
            {
                if (id <= 0) return BadRequest(new ApiResponse(400, "Id can not be 0 or less."));

                if (!ModelState.IsValid)  return BadRequest(ModelState);


                var route = await _unitOfWork.Routes.GetByIdAsync(id);

                if (route == null)
                    return NotFound(new ApiResponse(404));

                _mapper.Map(addRouteDTO, route);

                _unitOfWork.Routes.Update(route);

                var result = await _unitOfWork.Save();

                return (result >= 1) ? NoContent() : BadRequest(new ApiResponse(400));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiExceptionResponse(500,
                                ex.Message, (ex.InnerException != null) ? ex.InnerException.Message : ex.StackTrace));
            }

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRoute(int id)
        {
            try
            {
                if (id <= 0) return BadRequest(new ApiResponse(400, "Id can not be 0 or less."));

               var route = await _unitOfWork.Routes.GetByIdAsync(id);

                if (route == null)
                    return NotFound(new ApiResponse(404));
       
                _unitOfWork.Routes.Delete(route);

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
