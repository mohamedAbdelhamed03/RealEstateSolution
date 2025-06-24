using AutoMapper;
using Microsoft.AspNetCore.Http;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Core.DTOs;
using RealEstate.Core.ServiceContracts.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Services.Companies
{
	public class CompaniesUpdaterService : ICompaniesUpdaterService
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		public CompaniesUpdaterService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}

		public async Task<CompanyResponseDTO> UpdateCompany(CompanyUpdateDTO companyUpdateDTO)
		{
			Company company = _mapper.Map<Company>(companyUpdateDTO);
			company.UpdatedAt = DateTime.UtcNow;

			await _unitOfWork.CompanyRepository.Update(company);

			return _mapper.Map<CompanyResponseDTO>(company);
		}


	}
}
