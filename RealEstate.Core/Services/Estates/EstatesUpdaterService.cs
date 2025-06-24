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
	public class EstatesUpdaterService : IEstatesUpdaterService
	{

		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public EstatesUpdaterService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<EstateResponseDTO> UpdateEstate(EstateUpdateDTO estateUpdateDTO)
		{
			Estate estate = _mapper.Map<Estate>(estateUpdateDTO);
			estate.UpdatedAt = DateTime.UtcNow;
			if (estateUpdateDTO.Image != null)
			{
				if (!string.IsNullOrEmpty(estate.ImageLocalPath))
				{
					var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), estate.ImageLocalPath);
					FileInfo file = new FileInfo(oldFilePathDirectory);

					if (file.Exists)
					{
						file.Delete();
					}
				}

				string fileName = estateUpdateDTO.Id + Path.GetExtension(estateUpdateDTO.Image.FileName);
				string filePath = @"wwwroot\Images\" + fileName;

				var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

				using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
				{
					estateUpdateDTO.Image.CopyTo(fileStream);
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
