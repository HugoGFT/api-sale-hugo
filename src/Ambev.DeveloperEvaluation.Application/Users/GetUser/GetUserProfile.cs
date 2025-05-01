using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

/// <summary>
/// Profile for mapping between User entity and GetUserResponse
/// </summary>
public class GetUserProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUser operation
    /// </summary>
    public GetUserProfile()
    {
        CreateMap<GetUserResult, User>()
            .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.Name.Firstname))
            .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.Name.Lastname))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Address.Number))
            .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Address.ZipCode))
            .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Address.Geolocation.Lat))
            .ForMember(dest => dest.Long, opt => opt.MapFrom(src => src.Address.Geolocation.Long))
            .ReverseMap()
            .ForPath(src => src.Name.Firstname, opt => opt.MapFrom(dest => dest.Firstname))
            .ForPath(src => src.Name.Lastname, opt => opt.MapFrom(dest => dest.Lastname))
            .ForPath(src => src.Address.Street, opt => opt.MapFrom(dest => dest.Street))
            .ForPath(src => src.Address.City, opt => opt.MapFrom(dest => dest.City))
            .ForPath(src => src.Address.Number, opt => opt.MapFrom(dest => dest.Number))
            .ForPath(src => src.Address.ZipCode, opt => opt.MapFrom(dest => dest.ZipCode))
            .ForPath(src => src.Address.Geolocation.Lat, opt => opt.MapFrom(dest => dest.Lat))
            .ForPath(src => src.Address.Geolocation.Long, opt => opt.MapFrom(dest => dest.Long));
    }
}
