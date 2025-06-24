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
	public class CompaniesGetterService : ICompaniesGetterService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CompaniesGetterService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}


		public async Task<IEnumerable<CompanyResponseDTO>> GetAllCompanies()
		{
			IEnumerable<Company> companies = await _unitOfWork.CompanyRepository.GetAll();
			return _mapper.Map<IEnumerable<CompanyResponseDTO>>(companies);
		}

		public async Task<CompanyResponseDTO> GetCompanyById(Guid CompanyId)
		{
			Company? Company = await _unitOfWork.CompanyRepository.Get(e => e.Id == CompanyId);
			return _mapper.Map<CompanyResponseDTO>(Company);
		}

		public async Task<IEnumerable<CompanyResponseDTO>> GetFilterdCompany(string searchBy, string searchString)
		{
			IEnumerable<CompanyResponseDTO> companies;
			companies = searchBy switch
			{
				nameof(Company.Name) => _mapper.Map<IEnumerable<CompanyResponseDTO>>(await _unitOfWork.CompanyRepository.GetAll(e => e.Name!.Contains(searchString))),

				nameof(Company.City) => _mapper.Map<IEnumerable<CompanyResponseDTO>>(await _unitOfWork.CompanyRepository.GetAll(e => e.City!.Contains(searchString))),

				nameof(Company.Email) => _mapper.Map<IEnumerable<CompanyResponseDTO>>(await _unitOfWork.CompanyRepository.GetAll(e => e.Email!.Contains(searchString))),

				nameof(Company.State) => _mapper.Map<IEnumerable<CompanyResponseDTO>>(await _unitOfWork.CompanyRepository.GetAll(e => e.State!.Contains(searchString))),

				nameof(Company.PostalCode) => _mapper.Map<IEnumerable<CompanyResponseDTO>>(await _unitOfWork.CompanyRepository.GetAll(e => e.PostalCode!.Contains(searchString))),

				nameof(Company.PhoneNumber) => _mapper.Map<IEnumerable<CompanyResponseDTO>>(await _unitOfWork.CompanyRepository.GetAll(e => e.PhoneNumber!.Contains(searchString))),

				nameof(Company.CreatedAt) => _mapper.Map<IEnumerable<CompanyResponseDTO>>(await _unitOfWork.CompanyRepository.GetAll(e => e.CreatedAt.ToString()!.Contains(searchString))),

				nameof(Company.UpdatedAt) => _mapper.Map<IEnumerable<CompanyResponseDTO>>(await _unitOfWork.CompanyRepository.GetAll(e => e.UpdatedAt.ToString()!.Contains(searchString))),
				_ => _mapper.Map<IEnumerable<CompanyResponseDTO>>(await _unitOfWork.CompanyRepository.GetAll(null))

			};
			return companies;
		}
	}
}
