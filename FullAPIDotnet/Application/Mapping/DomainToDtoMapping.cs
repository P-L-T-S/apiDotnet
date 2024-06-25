using AutoMapper;
using FullAPIDotnet.Domain.DTOs;
using FullAPIDotnet.Domain.Model;

namespace FullAPIDotnet.Application.Mapping;

public class DomainToDtoMapping : Profile
{
    public DomainToDtoMapping()
    {
        CreateMap<Employee, EmployeeDto>();
        // caso fosse nescessario resolver algum conflito entre o nome da classe e do dto       
        // .ForMember(dest => dest.Name, member => member.MapFrom(origin => origin.Name));
    }
}