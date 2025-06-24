using AutoMapper;
using Microsoft.AspNetCore.Http;
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
	public class EstatesDeleterService : IEstatesDeleterService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public EstatesDeleterService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<EstateResponseDTO> DeleteEstate(Guid? EstateId)
		{

			Estate estate = await _unitOfWork.EstateRepository.Get(e => e.Id == EstateId);


			await _unitOfWork.EstateRepository.Remove(estate);
			return _mapper.Map<EstateResponseDTO>(estate);

		}

	}

}
