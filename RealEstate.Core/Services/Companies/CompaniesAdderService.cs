using AutoMapper;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Core.DTOs;
using RealEstate.Core.ServiceContracts.Categories;
using RealEstate.Core.ServiceContracts.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Services.Companies
{
	public class CompaniesAdderService : ICompaniesAdderService
	{

		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CompaniesAdderService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<CompanyResponseDTO> AddCompanyAsync(CompanyCreateDTO companyCreateDTO)
		{

			Company company = _mapper.Map<Company>(companyCreateDTO);
			company.Id = Guid.NewGuid();
			company.CreatedAt = DateTime.UtcNow;
			company.UpdatedAt = DateTime.UtcNow;

			await _unitOfWork.CompanyRepository.Add(company);
			return _mapper.Map<CompanyResponseDTO>(company);
		}
	}

}
