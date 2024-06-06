using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.DTO;
using TaskManager.Models;
using TaskManager.ViewModel;

namespace TaskManager.Settings.Mapping
{
    public class Mapper : Profile
    {
        public Mapper (){
            CreateMap<UserModel,UserModelDto>()
                .ReverseMap()
                .ForMember(src => src.UserName, 
                    map => map.MapFrom(dto => dto.Email))
                .ForMember(src => src.Id, map => map.Ignore());
            CreateMap<UserModel, UserModel>();
            CreateMap<TaskModel, TaskModelDto>()
                .ReverseMap();
            CreateMap<ProjectModel, ProjectModelDto>()
                .ReverseMap();
                
        }
    }
}