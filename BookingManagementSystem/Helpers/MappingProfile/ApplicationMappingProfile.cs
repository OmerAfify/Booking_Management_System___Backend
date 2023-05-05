using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookingManagementSystem.DTOs;
using Core.Models;

namespace BookingManagementSystem.Helpers.MappingProfile
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            //Train
            CreateMap<AddTrainDTO, Train>().ReverseMap();
            CreateMap<ReturnTrainDTO,Train>().ReverseMap();

            //Route
            CreateMap<AddRouteDTO, Route>().ReverseMap();
            CreateMap<RouteDTO, Route>().ReverseMap();
               

            //Schedule
            CreateMap<AddScheduleDTO, Schedule>().ReverseMap();
            CreateMap<ScheduleDTO, Schedule>().ReverseMap()
                .ForMember(s => s.TrainName, opt => opt.MapFrom(s => s.Train.Name))
                .ForMember(s => s.Route, opt => opt.MapFrom(s => "From "+ s.Route.Departure + " to " + s.Route.Arrival));

            //Users
            //CreateMap<ReturnedUserDTO, ApplicationUser>().ReverseMap()
            //    .ForMember(s => s.Roles, opt => opt.MapFrom(s => s.Roles.Select(x=>x.Name).ToList()  ) );
       
        
        }
    }
}
