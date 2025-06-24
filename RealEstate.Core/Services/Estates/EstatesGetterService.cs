using AutoMapper;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Core.DTOs;
using RealEstate.Core.ServiceContracts.Estates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Core.Services.Estates
{
	public class EstatesGetterService : IEstatesGetterService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public EstatesGetterService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<IEnumerable<EstateResponseDTO>> GetAllEstates()
		{
			IEnumerable<Estate> estates = await _unitOfWork.EstateRepository.GetAll(null, ["Category", "Company"]);
			return _mapper.Map<IEnumerable<EstateResponseDTO>>(estates);
		}

		public async Task<EstateResponseDTO> GetEstateById(Guid EstateId)
		{
			Estate? estate = await _unitOfWork.EstateRepository.Get(e => e.Id == EstateId, ["Category", "Company"]);
			return _mapper.Map<EstateResponseDTO>(estate);
		}

		public async Task<IEnumerable<EstateResponseDTO>> GetFilterdEstate(string searchBy, string searchString)
		{
			IEnumerable<EstateResponseDTO> estates;
			estates = searchBy switch
			{
				nameof(Estate.Name) => _mapper.Map<IEnumerable<EstateResponseDTO>>(await _unitOfWork.EstateRepository.GetAll(e => e.Name!.Contains(searchString), ["Category", "Company"])),
				"Category" => _mapper.Map<IEnumerable<EstateResponseDTO>>(await _unitOfWork.EstateRepository.GetAll(e => e.Category!.Name!.Contains(searchString), ["Category", "Company"])),
				nameof(Estate.Company) => _mapper.Map<IEnumerable<EstateResponseDTO>>(await _unitOfWork.EstateRepository.GetAll(e => e.Company!.Name!.Contains(searchString), ["Category", "Company"])),
				nameof(Estate.EstateNumber) => _mapper.Map<IEnumerable<EstateResponseDTO>>(await _unitOfWork.EstateRepository.GetAll(e => e.EstateNumber!.Contains(searchString), ["Category", "Company"])),
				nameof(Estate.Price) => _mapper.Map<IEnumerable<EstateResponseDTO>>(await _unitOfWork.EstateRepository.GetAll(e => e.Price.ToString().Contains(searchString), ["Category", "Company"])),
				nameof(Estate.Rate) => _mapper.Map<IEnumerable<EstateResponseDTO>>(await _unitOfWork.EstateRepository.GetAll(e => e.Rate.ToString().Contains(searchString), ["Category", "Company"])),
				nameof(Estate.Bedrooms) => _mapper.Map<IEnumerable<EstateResponseDTO>>(await _unitOfWork.EstateRepository.GetAll(e => e.Bedrooms.ToString().Contains(searchString), ["Category", "Company"])),
				nameof(Estate.Bathrooms) => _mapper.Map<IEnumerable<EstateResponseDTO>>(await _unitOfWork.EstateRepository.GetAll(e => e.Bathrooms.ToString().Contains(searchString), ["Category", "Company"])),
				_ => _mapper.Map<IEnumerable<EstateResponseDTO>>(await _unitOfWork.EstateRepository.GetAll(null, ["Category", "Company"]))
			};
			return estates;
		}
	}
}
