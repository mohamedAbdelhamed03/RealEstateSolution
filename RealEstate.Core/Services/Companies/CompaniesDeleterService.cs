using AutoMapper;
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
	public class CompaniesDeleterService : ICompaniesDeleterService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CompaniesDeleterService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<CompanyResponseDTO> DeleteCompany(Guid? companyId)
		{

			Company company = await _unitOfWork.CompanyRepository.Get(e => e.Id == companyId);


			await _unitOfWork.CompanyRepository.Remove(company);
			return _mapper.Map<CompanyResponseDTO>(company);

		}
	}
}
