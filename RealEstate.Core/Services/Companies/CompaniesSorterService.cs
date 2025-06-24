using AutoMapper;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Core.DTOs;
using RealEstate.Core.Enums;
using RealEstate.Core.ServiceContracts.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Services.Companies
{
	public class CompaniesSorterService : ICompaniesSorterService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CompaniesSorterService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<IEnumerable<CompanyResponseDTO>> SortCompaniesAsync(string sortBy, SortedOrderOptions sortedOrder)
		{
			IEnumerable<CompanyResponseDTO> companies = _mapper.Map<IEnumerable<CompanyResponseDTO>>(await _unitOfWork.CompanyRepository.GetAll());
			if (string.IsNullOrEmpty(sortBy))
				return companies;
			IEnumerable<CompanyResponseDTO> sortedCompanies = (sortBy, sortedOrder)
				switch
			{
				(nameof(Company.Name), SortedOrderOptions.ASC) => companies.OrderBy(e => e.Name),
				(nameof(Company.Name), SortedOrderOptions.DESC) => companies.OrderByDescending(e => e.Name),
				(nameof(Company.CreatedAt), SortedOrderOptions.DESC) => companies.OrderByDescending(e => e.CreatedAt),
				(nameof(Company.UpdatedAt), SortedOrderOptions.ASC) => companies.OrderBy(e => e.UpdatedAt),
				(nameof(Company.UpdatedAt), SortedOrderOptions.DESC) => companies.OrderByDescending(e => e.UpdatedAt),
				(nameof(Company.City), SortedOrderOptions.ASC) => companies.OrderBy(e => e.City),
				(nameof(Company.City), SortedOrderOptions.DESC) => companies.OrderByDescending(e => e.City),
				_ => companies
			};
			return await Task.FromResult(sortedCompanies);

		}
	}
}
