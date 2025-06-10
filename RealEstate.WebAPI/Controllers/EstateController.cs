using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Core.DTO;
using RealEstate.Core.Enums;
using RealEstate.Infrastructure.DbContext;

namespace RealEstate.WebAPI.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class EstateController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public EstateController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;

		}
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<EstateResponseDTO>>> GetAllEstates()
		{
			try
			{
				IEnumerable<Estate> estates = await _unitOfWork.EstateRepository.GetAll(null, ["Category"]);
				return Ok(_mapper.Map<IEnumerable<EstateResponseDTO>>(estates));


			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<EstateResponseDTO>> GetEstate(Guid id)
		{
			try
			{
				Estate? estate = await _unitOfWork.EstateRepository.Get(e => e.Id == id, ["Category"]);
				if (estate == null)
				{
					return NotFound($"Estate with ID {id} not found.");
				}
				return Ok(_mapper.Map<EstateResponseDTO>(estate));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<EstateResponseDTO>> CreateEstate([FromForm] EstateCreateDTO estateCreateDTO)
		{
			if (estateCreateDTO == null)
			{
				return BadRequest("Estate object is null.");
			}
			try
			{
				Estate estate = _mapper.Map<Estate>(estateCreateDTO);
				estate.Id = Guid.NewGuid();
				estate.CreatedAt = DateTime.UtcNow;
				estate.UpdatedAt = DateTime.UtcNow;
				// Ensure a new ID is generated for the estate
				//Estate estate = new()
				//{
				//	Name = estateCreateDTO.Name,
				//	Details = estateCreateDTO.Details,
				//	Rate = estateCreateDTO.Rate,
				//	Sqft = estateCreateDTO.Sqft,
				//	Occupancy = estateCreateDTO.Occupancy,
				//	ImageUrl = estateCreateDTO.ImageUrl,
				//	Amenity = estateCreateDTO.Amenity,
				//	CategoryId = estateCreateDTO.CategoryId,
				//	CreatedAt = DateTime.UtcNow,
				//	UpdatedAt = DateTime.UtcNow
				//};
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

					var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
					estate.ImageUrl = baseUrl + "/Images/" + fileName;
					estate.ImageLocalPath = filePath;

				}
				else
				{
					estate.ImageUrl = "https://placehold.co/600x400";
				}

				await _unitOfWork.EstateRepository.Update(estate);


				var createdEstate = await _unitOfWork.EstateRepository.Get(
			e => e.Id == estate.Id, ["Category"]);


				return CreatedAtAction(nameof(GetEstate), new { id = createdEstate.Id }, _mapper.Map<EstateResponseDTO>(createdEstate));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<ActionResult<EstateResponseDTO>> DeleteEstate(Guid id)
		{
			try
			{
				Estate? estate = await _unitOfWork.EstateRepository.Get(e => e.Id == id, ["Category"]);
				if (estate == null)
				{
					return NotFound($"Estate with ID {id} not found.");
				}
				if (!string.IsNullOrEmpty(estate.ImageLocalPath))
				{
					var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), estate.ImageLocalPath);
					FileInfo file = new FileInfo(oldFilePathDirectory);

					if (file.Exists)
					{
						file.Delete();
					}
				}
				bool isDeleted = await _unitOfWork.EstateRepository.Remove(estate);
				if (!isDeleted)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting estate.");
				}
				return Ok(_mapper.Map<EstateResponseDTO>(estate));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<EstateResponseDTO>> UpdateEstate(Guid id, [FromBody] EstateUpdateDTO estateUpdateDTO)
		{
			if (estateUpdateDTO == null || estateUpdateDTO.Id != id)
			{
				return BadRequest("Estate object is null or ID mismatch.");
			}
			try
			{
				Estate estate = _mapper.Map<Estate>(estateUpdateDTO);
				estate.UpdatedAt = DateTime.UtcNow;
				//estate.CreatedAt = estateUpdateDTO.CreatedAt;
				//Estate? estate = new Estate
				//{
				//	Id = estateUpdateDTO.Id,
				//	Name = estateUpdateDTO.Name,
				//	Details = estateUpdateDTO.Details,
				//	Rate = estateUpdateDTO.Rate,
				//	Sqft = estateUpdateDTO.Sqft,
				//	Occupancy = estateUpdateDTO.Occupancy,
				//	ImageUrl = estateUpdateDTO.ImageUrl,
				//	Amenity = estateUpdateDTO.Amenity,
				//	CategoryId = estateUpdateDTO.CategoryId,
				//	CreatedAt = estateUpdateDTO.CreatedAt,
				//	UpdatedAt = DateTime.UtcNow
				//};
				if (estate == null)
				{
					return NotFound($"Estate with ID {id} not found.");
				}
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

					var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
					estate.ImageUrl = baseUrl + "/Images/" + fileName;
					estate.ImageLocalPath = filePath;

				}
				else
				{
					estate.ImageUrl = "https://placehold.co/600x400";
				}

				await _unitOfWork.EstateRepository.Update(estate);

				var updatedEstate = await _unitOfWork.EstateRepository.Get(
			e => e.Id == estate.Id, ["Category"]);
				return Ok(_mapper.Map<EstateResponseDTO>(updatedEstate));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
		[HttpPatch("{id:guid}")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<EstateResponseDTO>> PatchEstate(Guid id, [FromBody] JsonPatchDocument<EstateUpdateDTO> patchDoc)
		{
			if (patchDoc == null)
			{
				return BadRequest("Patch document is null.");
			}
			try
			{
				Estate? estate = await _unitOfWork.EstateRepository.Get(e => e.Id == id, ["Category"], true);
				if (estate == null)
				{
					return NotFound($"Estate with ID {id} not found.");
				}
				EstateUpdateDTO estateUpdateDTO = _mapper.Map<EstateUpdateDTO>(estate);
				patchDoc.ApplyTo(estateUpdateDTO, ModelState);
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				Estate updatedEstate = _mapper.Map<Estate>(estateUpdateDTO);
				updatedEstate.Id = id;
				await _unitOfWork.EstateRepository.Update(updatedEstate);
				var patchedEstate = await _unitOfWork.EstateRepository.Get(
			e => e.Id == updatedEstate.Id, ["Category"]);
				return Ok(_mapper.Map<EstateResponseDTO>(patchedEstate));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
	}
}
