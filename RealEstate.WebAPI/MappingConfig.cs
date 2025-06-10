using AutoMapper;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.Domain.IdentityEntities;
using RealEstate.Core.DTO;

namespace RealEstate.WebAPI
{
	public class MappingConfig : Profile
	{
		public MappingConfig()
		{
			CreateMap<Estate, EstateResponseDTO>()
				.IncludeMembers(s => s.Category);

			CreateMap<Category, EstateResponseDTO>(MemberList.None)
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name));

			CreateMap<EstateResponseDTO, Estate>();
			CreateMap<Estate, EstateCreateDTO>().ReverseMap();
			CreateMap<Estate, EstateUpdateDTO>().ReverseMap();
			CreateMap<Category, CategoryResponseDTO>();
			CreateMap<CategoryCreateDTO, Category>();
			CreateMap<Category, CategoryCreateDTO>().ReverseMap();
			CreateMap<Category, CategoryUpdateDTO>().ReverseMap();
			CreateMap<ApplicationUser, UserResponseDTO>().ReverseMap();
			CreateMap<ApplicationUser, RegisterRequestDTO>().ReverseMap().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.PersonName));

		}
	}
}
