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
	public class EstatesAdderService : IEstatesAdderService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IMapper _mapper;
		public EstatesAdderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_httpContextAccessor = httpContextAccessor;
			_mapper = mapper;
		}
		public async Task<EstateResponseDTO> AddEstateAsync(EstateCreateDTO estateCreateDTO)
		{
			Estate estate = _mapper.Map<Estate>(estateCreateDTO);
			estate.Id = Guid.NewGuid();
			estate.CreatedAt = DateTime.UtcNow;
			estate.UpdatedAt = DateTime.UtcNow;
			await _unitOfWork.EstateRepository.Add(estate);
			if (estateCreateDTO.Image != null)
			{
				string fileName = estate.Id + Path.GetExtension(estateCreateDTO.Image.FileName);
				string filePath = @"wwwroot\Images\" + fileName;

				var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

				FileInfo file = new FileInfo(directoryLocation);

				if (file.Exists)
				{
					file.Delete();
				}

				using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
				{
					estateCreateDTO.Image.CopyTo(fileStream);
				}

				var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}{_httpContextAccessor.HttpContext.Request.PathBase.Value}";
				estate.ImageUrl = baseUrl + "/Images/" + fileName;
				estate.ImageLocalPath = filePath;

			}
			else
			{
				estate.ImageUrl = "https://placehold.co/600x400";
			}

			await _unitOfWork.EstateRepository.Update(estate);
			estate = await _unitOfWork.EstateRepository.Get(e => e.Id == estate.Id, ["Category", "Company"]);
			return _mapper.Map<EstateResponseDTO>(estate);
		}
	}
}
