using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.Domain.RepositoryContracts;
using RealEstate.Core.DTO;
using RealEstate.Infrastructure.DbContext;

namespace RealEstate.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class CompanyController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CompanyController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;

		}
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<CompanyResponseDTO>>> GetAllCategories()
		{
			try
			{
				IEnumerable<Company> categories = await _unitOfWork.CompanyRepository.GetAll();
				return Ok(_mapper.Map<IEnumerable<CompanyResponseDTO>>(categories));


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
		public async Task<ActionResult<CompanyResponseDTO>> GetCompany(Guid id)
		{
			try
			{
				Company? Company = await _unitOfWork.CompanyRepository.Get(e => e.Id == id);
				if (Company == null)
				{
					return NotFound($"Company with ID {id} not found.");
				}
				return Ok(_mapper.Map<CompanyResponseDTO>(Company));
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
		public async Task<ActionResult<Company>> CreateCompany([FromBody] CompanyCreateDTO CompanyCreateDTO)
		{
			if (CompanyCreateDTO == null)
			{
				return BadRequest("Company object is null.");
			}
			try
			{
				Company Company = _mapper.Map<Company>(CompanyCreateDTO);
				Company.Id = Guid.NewGuid();
				Company.CreatedAt = DateTime.UtcNow;
				Company.UpdatedAt = DateTime.UtcNow;
				await _unitOfWork.CompanyRepository.Add(Company);

				var createdCompany = await _unitOfWork.CompanyRepository.Get(
			e => e.Id == Company.Id);


				return CreatedAtAction(nameof(GetCompany), new { id = createdCompany.Id }, _mapper.Map<CompanyResponseDTO>(createdCompany));
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
		public async Task<ActionResult<CompanyResponseDTO>> DeleteCompany(Guid id)
		{
			try
			{
				Company? Company = await _unitOfWork.CompanyRepository.Get(e => e.Id == id);
				if (Company == null)
				{
					return NotFound($"Company with ID {id} not found.");
				}
				bool isDeleted = await _unitOfWork.CompanyRepository.Remove(Company);
				if (!isDeleted)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting Company.");
				}
				return Ok(_mapper.Map<CompanyResponseDTO>(Company));
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
		public async Task<ActionResult<CompanyResponseDTO>> UpdateCompany(Guid id, [FromBody] CompanyUpdateDTO CompanyUpdateDTO)
		{
			if (CompanyUpdateDTO == null || CompanyUpdateDTO.Id != id)
			{
				return BadRequest("Company object is null or ID mismatch.");
			}
			try
			{
				Company Company = _mapper.Map<Company>(CompanyUpdateDTO);
				Company.UpdatedAt = DateTime.UtcNow;
				if (Company == null)
				{
					return NotFound($"Company with ID {id} not found.");
				}
				await _unitOfWork.CompanyRepository.Update(Company);

				var updatedCompany = await _unitOfWork.CompanyRepository.Get(
			e => e.Id == Company.Id);
				return Ok(_mapper.Map<CompanyResponseDTO>(updatedCompany));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
		[HttpPatch("{id:guid}")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<CompanyResponseDTO>> PatchCompany(Guid id, [FromBody] JsonPatchDocument<CompanyUpdateDTO> patchDoc)
		{
			if (patchDoc == null)
			{
				return BadRequest("Patch document is null.");
			}
			try
			{
				Company? Company = await _unitOfWork.CompanyRepository.Get(e => e.Id == id, noTracking: true);
				if (Company == null)
				{
					return NotFound($"Company with ID {id} not found.");
				}
				CompanyUpdateDTO CompanyUpdateDTO = _mapper.Map<CompanyUpdateDTO>(Company);
				patchDoc.ApplyTo(CompanyUpdateDTO, ModelState);
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				Company updatedCompany = _mapper.Map<Company>(CompanyUpdateDTO);
				updatedCompany.Id = id;
				await _unitOfWork.CompanyRepository.Update(updatedCompany);
				var patchedCompany = await _unitOfWork.CompanyRepository.Get(
			e => e.Id == updatedCompany.Id);
				return Ok(_mapper.Map<CompanyResponseDTO>(patchedCompany));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
	}
}
