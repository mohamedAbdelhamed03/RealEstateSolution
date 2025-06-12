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
				.IncludeMembers(s => s.Category).IncludeMembers(s => s.Company);

			CreateMap<Category, EstateResponseDTO>(MemberList.None)
				.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name));

			CreateMap<Company, EstateResponseDTO>(MemberList.None)
				.ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Name));

			CreateMap<EstateResponseDTO, Estate>();
			CreateMap<Estate, EstateCreateDTO>().ReverseMap();
			CreateMap<Estate, EstateUpdateDTO>().ReverseMap();
			CreateMap<Category, CategoryResponseDTO>();
			CreateMap<CategoryCreateDTO, Category>();
			CreateMap<Category, CategoryCreateDTO>().ReverseMap();
			CreateMap<Category, CategoryUpdateDTO>().ReverseMap();
			CreateMap<Company, CompanyResponseDTO>();
			CreateMap<CompanyCreateDTO, Company>();
			CreateMap<Company, CompanyCreateDTO>().ReverseMap();
			CreateMap<Company, CompanyUpdateDTO>().ReverseMap();
			CreateMap<ApplicationUser, UserResponseDTO>().ReverseMap();
			CreateMap<ApplicationUser, RegisterRequestDTO>().ReverseMap().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.PersonName));

		}
	}
}
