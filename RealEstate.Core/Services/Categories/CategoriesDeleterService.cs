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
	public class CategoriesDeleterService : ICategoriesDeleterService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CategoriesDeleterService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<CategoryResponseDTO> DeleteCategory(Guid? categoryId)
		{

			Category category = await _unitOfWork.CategoryRepository.Get(e => e.Id == categoryId);


			await _unitOfWork.CategoryRepository.Remove(category);
			return _mapper.Map<CategoryResponseDTO>(category);

		}

	}

}
