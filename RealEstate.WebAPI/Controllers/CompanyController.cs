using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.DTOs;
using RealEstate.Core.Enums;
using RealEstate.Core.ServiceContracts.Companies;

namespace RealEstate.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class CompanyController : ControllerBase
	{
		private readonly ICompaniesAdderService _companiesAdderService;
		private readonly ICompaniesDeleterService _companiesDeleterService;
		private readonly ICompaniesGetterService _companiesGetterService;
		private readonly ICompaniesUpdaterService _companiesUpdaterService;
		private readonly ICompaniesSorterService _companiesSorterService;
		private readonly IMapper _mapper;
		public CompanyController(ICompaniesAdderService companiesAdderService, ICompaniesDeleterService companiesDeleterService, ICompaniesGetterService companiesGetterService, ICompaniesUpdaterService companiesUpdaterService, ICompaniesSorterService companiesSorterService, IMapper mapper)
		{
			_companiesAdderService = companiesAdderService;
			_companiesDeleterService = companiesDeleterService;
			_companiesGetterService = companiesGetterService;
			_companiesUpdaterService = companiesUpdaterService;
			_companiesSorterService = companiesSorterService;
			_mapper = mapper;

		}
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<CompanyResponseDTO>>> GetAllCompanies()
		{
			try
			{
				IEnumerable<CompanyResponseDTO> companies = await _companiesGetterService.GetAllCompanies();
				return Ok(companies);


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
		public async Task<ActionResult<IEnumerable<CompanyResponseDTO>>> GetFilteredCompanies(string searchBy, string searchString)
		{
			IEnumerable<CompanyResponseDTO> companies = await _companiesGetterService.GetAllCompanies();
			if (searchBy == null || searchString == null)
			{
				return Ok(companies);
			}
			try
			{

				IEnumerable<CompanyResponseDTO> filteredCompanies = await _companiesGetterService.GetFilterdCompany(searchBy, searchString);
				if (filteredCompanies == null || !filteredCompanies.Any())
				{
					return NotFound("No companies found matching the filter criteria.");
				}
				return Ok(filteredCompanies);
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
		public async Task<ActionResult<IEnumerable<CompanyResponseDTO>>> GetSortedCompanies(string sortBy, SortedOrderOptions sortOrder)
		{
			try
			{
				IEnumerable<CompanyResponseDTO> sortedcompanies = await _companiesSorterService.SortCompaniesAsync(sortBy, sortOrder);
				return Ok(sortedcompanies);
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
		public async Task<ActionResult<CompanyResponseDTO>> GetCompanyById(Guid id)
		{
			try
			{
				CompanyResponseDTO? company = await _companiesGetterService.GetCompanyById(id);
				if (company == null)
				{
					return NotFound($"Company with ID {id} not found.");
				}
				return Ok(company);
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
				CompanyResponseDTO createdCompany = await _companiesAdderService.AddCompanyAsync(CompanyCreateDTO);
				return CreatedAtAction(nameof(GetCompanyById), new { id = createdCompany.Id }, createdCompany);
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
				Company? Company = _mapper.Map<Company>(await _companiesGetterService.GetCompanyById(id));
				if (Company == null)
				{
					return NotFound($"Company with ID {id} not found.");
				}
				CompanyResponseDTO DeletedCompany = await _companiesDeleterService.DeleteCompany(id);

				return Ok(DeletedCompany);
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
				CompanyResponseDTO Company = await _companiesUpdaterService.UpdateCompany(CompanyUpdateDTO);

				if (Company == null)
				{
					return NotFound($"Company with ID {id} not found.");
				}

				return Ok(Company);
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
				Company? Company = _mapper.Map<Company>(await _companiesGetterService.GetCompanyById(id));
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
				CompanyUpdateDTO.Id = id;
				Company updatedCompany = _mapper.Map<Company>(CompanyUpdateDTO);
				await _companiesUpdaterService.UpdateCompany(CompanyUpdateDTO);
				var patchedCompany = await _companiesGetterService.GetCompanyById(updatedCompany.Id);
				return Ok(_mapper.Map<CompanyResponseDTO>(patchedCompany));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
	}
}
