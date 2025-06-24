using AutoMapper;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Core.DTOs;
using RealEstate.Core.ServiceContracts.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Services.Categories
{
	public class CategoriesGetterService : ICategoriesGetterService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CategoriesGetterService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<IEnumerable<CategoryResponseDTO>> GetAllCategories()
		{
			IEnumerable<Category> categories = await _unitOfWork.CategoryRepository.GetAll();
			return _mapper.Map<IEnumerable<CategoryResponseDTO>>(categories);
		}

		public async Task<CategoryResponseDTO> GetCategoryById(Guid categoryId)
		{
			Category? category = await _unitOfWork.CategoryRepository.Get(e => e.Id == categoryId);
			return _mapper.Map<CategoryResponseDTO>(category);
		}
	}
}
