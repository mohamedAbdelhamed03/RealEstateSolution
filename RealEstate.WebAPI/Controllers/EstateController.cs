using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.DTOs;
using RealEstate.Core.Enums;
using RealEstate.Core.ServiceContracts.Estates;

namespace RealEstate.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class EstateController : ControllerBase
	{
		private readonly IEstatesAdderService _estatesAdderService;
		private readonly IEstatesDeleterService _estatesDeleterService;
		private readonly IEstatesGetterService _estatesGetterService;
		private readonly IEstatesUpdaterService _estatesUpdaterService;
		private readonly IEstatesSorterService _estatesSorterService;
		private readonly IMapper _mapper;
		public EstateController(IEstatesAdderService estatesAdderService, IEstatesDeleterService estatesDeleterService, IEstatesGetterService estatesGetterService, IEstatesUpdaterService estatesUpdaterService, IEstatesSorterService estatesSorterService, IMapper mapper)
		{
			_mapper = mapper;
			_estatesAdderService = estatesAdderService;
			_estatesDeleterService = estatesDeleterService;
			_estatesGetterService = estatesGetterService;
			_estatesUpdaterService = estatesUpdaterService;
			_estatesSorterService = estatesSorterService;
		}
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<EstateResponseDTO>>> GetAllEstates()
		{
			try
			{
				IEnumerable<EstateResponseDTO> estates = await _estatesGetterService.GetAllEstates();

				return Ok(estates);


			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
		[HttpGet("filter")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<EstateResponseDTO>>> GetFilteredEstates(string searchBy, string searchString)
		{
			IEnumerable<EstateResponseDTO> estates = await _estatesGetterService.GetAllEstates();
			if (searchBy == null || searchString == null)
			{
				return Ok(estates);
			}
			try
			{

				IEnumerable<EstateResponseDTO> filteredEstates = await _estatesGetterService.GetFilterdEstate(searchBy, searchString);
				if (filteredEstates == null || !filteredEstates.Any())
				{
					return NotFound("No estates found matching the filter criteria.");
				}
				return Ok(filteredEstates);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
		[HttpGet("sort")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<EstateResponseDTO>>> GetSortedEstates(string sortBy, SortedOrderOptions sortOrder)
		{
			try
			{

				IEnumerable<EstateResponseDTO> sortedEstates = await _estatesSorterService.SortEstatesAsync(sortBy, sortOrder);
				return Ok(sortedEstates);
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
		public async Task<ActionResult<EstateResponseDTO>> GetEstateById(Guid id)
		{
			try
			{
				EstateResponseDTO estate = await _estatesGetterService.GetEstateById(id);
				if (estate == null)
				{
					return NotFound($"Estate with ID {id} not found.");
				}
				return Ok(estate);
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
				var createdEstate = await _estatesAdderService.AddEstateAsync(estateCreateDTO);
				return CreatedAtAction(nameof(GetEstateById), new { id = createdEstate.Id }, _mapper.Map<EstateResponseDTO>(createdEstate));
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
				Estate? estate = _mapper.Map<Estate>(await _estatesGetterService.GetEstateById(id));
				if (estate == null)
				{
					return NotFound($"Estate with ID {id} not found.");
				}
				EstateResponseDTO DeletedEstet = await _estatesDeleterService.DeleteEstate(id);

				return Ok(DeletedEstet);
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
		public async Task<ActionResult<EstateResponseDTO>> UpdateEstate(Guid id, [FromForm] EstateUpdateDTO estateUpdateDTO)
		{
			if (estateUpdateDTO == null || estateUpdateDTO.Id != id)
			{
				return BadRequest("Estate object is null or ID mismatch.");
			}
			try
			{
				EstateResponseDTO estate = await _estatesUpdaterService.UpdateEstate(estateUpdateDTO);

				if (estate == null)
				{
					return NotFound($"Estate with ID {id} not found.");
				}

				return Ok(estate);
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
				Estate? estate = _mapper.Map<Estate>(await _estatesGetterService.GetEstateById(id));
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
				estateUpdateDTO.Id = id;
				Estate updatedEstate = _mapper.Map<Estate>(estateUpdateDTO);

				await _estatesUpdaterService.UpdateEstate(estateUpdateDTO);
				var patchedEstate = await _estatesGetterService.GetEstateById(updatedEstate.Id);
				return Ok(_mapper.Map<EstateResponseDTO>(patchedEstate));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
	}
}
