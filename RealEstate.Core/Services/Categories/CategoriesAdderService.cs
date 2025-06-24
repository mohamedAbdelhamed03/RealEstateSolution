using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Core.DTOs;
using RealEstate.Core.ServiceContracts.Categories;
using RealEstate.Core.ServiceContracts.Estates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Services.Categories
{
	public class CategoriesAdderService : ICategoriesAdderService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CategoriesAdderService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<CategoryResponseDTO> AddCategoryAsync(CategoryCreateDTO categoryCreateDTO)
		{

			Category category = _mapper.Map<Category>(categoryCreateDTO);
			category.Id = Guid.NewGuid();
			category.CreatedAt = DateTime.UtcNow;
			category.UpdatedAt = DateTime.UtcNow;

			await _unitOfWork.CategoryRepository.Add(category);
			return _mapper.Map<CategoryResponseDTO>(category);
		}
	}
}
