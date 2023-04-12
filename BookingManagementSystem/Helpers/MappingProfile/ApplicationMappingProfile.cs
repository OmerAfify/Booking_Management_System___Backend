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

            CreateMap<AddTrainDTO, Train>().ReverseMap();

            CreateMap<ReturnTrainDTO,Train>().ReverseMap();

        }
    }
}
