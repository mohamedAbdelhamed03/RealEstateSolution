using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Core.Domain.Entities;
using RealEstate.Core.DTOs;
using RealEstate.Core.ServiceContracts.Categories;

namespace RealEstate.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoriesAdderService _categoriesAdderService;
		private readonly ICategoriesDeleterService _categoriesDeleterService;
		private readonly ICategoriesGetterService _categoriesGetterService;
		private readonly ICategoriesUpdaterService _categoriesUpdaterService;
		private readonly IMapper _mapper;
		public CategoryController(ICategoriesAdderService categoriesAdderService, ICategoriesDeleterService categoriesDeleterService, ICategoriesGetterService categoriesGetterService, ICategoriesUpdaterService categoriesUpdaterService, IMapper mapper)
		{
			_categoriesAdderService = categoriesAdderService;
			_categoriesDeleterService = categoriesDeleterService;
			_categoriesGetterService = categoriesGetterService;
			_categoriesUpdaterService = categoriesUpdaterService;
			_mapper = mapper;

		}
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<ActionResult<IEnumerable<CategoryResponseDTO>>> GetAllCategories()
		{
			try
			{
				IEnumerable<CategoryResponseDTO> categories = await _categoriesGetterService.GetAllCategories();
				return Ok(categories);


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
		public async Task<ActionResult<CategoryResponseDTO>> GetCategoryById(Guid id)
		{
			try
			{
				CategoryResponseDTO? category = await _categoriesGetterService.GetCategoryById(id);
				if (category == null)
				{
					return NotFound($"Category with ID {id} not found.");
				}
				return Ok(category);
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
		public async Task<ActionResult<Category>> CreateCategory([FromBody] CategoryCreateDTO categoryCreateDTO)
		{
			if (categoryCreateDTO == null)
			{
				return BadRequest("Category object is null.");
			}
			try
			{
				CategoryResponseDTO createdCategory = await _categoriesAdderService.AddCategoryAsync(categoryCreateDTO);
				return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
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
		public async Task<ActionResult<CategoryResponseDTO>> DeleteCategory(Guid id)
		{
			try
			{
				Category? category = _mapper.Map<Category>(await _categoriesGetterService.GetCategoryById(id));
				if (category == null)
				{
					return NotFound($"Category with ID {id} not found.");
				}
				CategoryResponseDTO DeletedCategory = await _categoriesDeleterService.DeleteCategory(id);

				return Ok(DeletedCategory);
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
		public async Task<ActionResult<CategoryResponseDTO>> UpdateCategory(Guid id, [FromBody] CategoryUpdateDTO categoryUpdateDTO)
		{
			if (categoryUpdateDTO == null || categoryUpdateDTO.Id != id)
			{
				return BadRequest("Category object is null or ID mismatch.");
			}
			try
			{
				CategoryResponseDTO category = await _categoriesUpdaterService.UpdateCategory(categoryUpdateDTO);

				if (category == null)
				{
					return NotFound($"Category with ID {id} not found.");
				}

				return Ok(category);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
		[HttpPatch("{id:guid}")]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<CategoryResponseDTO>> PatchCategory(Guid id, [FromBody] JsonPatchDocument<CategoryUpdateDTO> patchDoc)
		{
			if (patchDoc == null)
			{
				return BadRequest("Patch document is null.");
			}
			try
			{
				Category? category = _mapper.Map<Category>(await _categoriesGetterService.GetCategoryById(id));
				if (category == null)
				{
					return NotFound($"Category with ID {id} not found.");
				}
				CategoryUpdateDTO categoryUpdateDTO = _mapper.Map<CategoryUpdateDTO>(category);
				patchDoc.ApplyTo(categoryUpdateDTO, ModelState);
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				categoryUpdateDTO.Id = id;
				Category updatedCategory = _mapper.Map<Category>(categoryUpdateDTO);
				await _categoriesUpdaterService.UpdateCategory(categoryUpdateDTO);
				var patchedCategory = await _categoriesGetterService.GetCategoryById(updatedCategory.Id);
				return Ok(_mapper.Map<CategoryResponseDTO>(patchedCategory));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
	}
}
