using AutoMapper;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Core.DTOs;
using RealEstate.Core.Enums;
using RealEstate.Core.ServiceContracts.Estates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Services.Estates
{
	public class EstatesSorterService : IEstatesSorterService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public EstatesSorterService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<IEnumerable<EstateResponseDTO>> SortEstatesAsync(string sortBy, SortedOrderOptions sortedOrder)
		{
			IEnumerable<EstateResponseDTO> estates = _mapper.Map<IEnumerable<EstateResponseDTO>>(await _unitOfWork.EstateRepository.GetAll(null, ["Category", "Company"]));
			if (string.IsNullOrEmpty(sortBy))
				return estates;
			IEnumerable<EstateResponseDTO> sortedEstates = (sortBy, sortedOrder)
				switch
			{
				(nameof(Estate.Name), SortedOrderOptions.ASC) => estates.OrderBy(e => e.Name),
				(nameof(Estate.Name), SortedOrderOptions.DESC) => estates.OrderByDescending(e => e.Name),
				(nameof(Estate.Rate), SortedOrderOptions.ASC) => estates.OrderBy(e => e.Rate),
				(nameof(Estate.Rate), SortedOrderOptions.DESC) => estates.OrderByDescending(e => e.Rate),
				(nameof(Estate.Price), SortedOrderOptions.ASC) => estates.OrderBy(e => e.Price),
				(nameof(Estate.Price), SortedOrderOptions.DESC) => estates.OrderByDescending(e => e.Price),
				(nameof(Estate.CreatedAt), SortedOrderOptions.ASC) => estates.OrderBy(e => e.CreatedAt),
				(nameof(Estate.CreatedAt), SortedOrderOptions.DESC) => estates.OrderByDescending(e => e.CreatedAt),
				(nameof(Estate.UpdatedAt), SortedOrderOptions.ASC) => estates.OrderBy(e => e.UpdatedAt),
				(nameof(Estate.UpdatedAt), SortedOrderOptions.DESC) => estates.OrderByDescending(e => e.UpdatedAt),
				(nameof(Estate.Bedrooms), SortedOrderOptions.ASC) => estates.OrderBy(e => e.Bedrooms),
				(nameof(Estate.Bedrooms), SortedOrderOptions.DESC) => estates.OrderByDescending(e => e.Bedrooms),
				(nameof(Estate.Bathrooms), SortedOrderOptions.ASC) => estates.OrderBy(e => e.Bathrooms),
				(nameof(Estate.Bathrooms), SortedOrderOptions.DESC) => estates.OrderByDescending(e => e.Bathrooms),
				(nameof(Estate.Sqft), SortedOrderOptions.ASC) => estates.OrderBy(e => e.Sqft),
				(nameof(Estate.Sqft), SortedOrderOptions.DESC) => estates.OrderByDescending(e => e.Sqft),
				_ => estates
			};
			return await Task.FromResult(sortedEstates);

		}
	}
}
