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
	public class CategoriesUpdaterService : ICategoriesUpdaterService
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		public CategoriesUpdaterService(IMapper mapper, IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<CategoryResponseDTO> UpdateCategory(CategoryUpdateDTO categoryUpdateDTO)
		{
			Category category = _mapper.Map<Category>(categoryUpdateDTO);
			category.UpdatedAt = DateTime.UtcNow;

			await _unitOfWork.CategoryRepository.Update(category);

			return _mapper.Map<CategoryResponseDTO>(category);
		}

	}
}
