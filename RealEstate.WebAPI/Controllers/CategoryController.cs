using AutoMapper;
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
	public class CategoryController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
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
				IEnumerable<Category> categories = await _unitOfWork.CategoryRepository.GetAll();
				return Ok(_mapper.Map<IEnumerable<CategoryResponseDTO>>(categories));


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
		public async Task<ActionResult<CategoryResponseDTO>> GetCategory(Guid id)
		{
			try
			{
				Category? category = await _unitOfWork.CategoryRepository.Get(e => e.Id == id);
				if (category == null)
				{
					return NotFound($"Category with ID {id} not found.");
				}
				return Ok(_mapper.Map<CategoryResponseDTO>(category));
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
				Category category = _mapper.Map<Category>(categoryCreateDTO);
				category.Id = Guid.NewGuid();
				category.CreatedAt = DateTime.UtcNow;
				category.UpdatedAt = DateTime.UtcNow;
				// Ensure a new ID is generated for the category
				//Category category = new()
				//{
				//	Name = categoryCreateDTO.Name,
				//	CreatedAt = DateTime.UtcNow,
				//	UpdatedAt = DateTime.UtcNow
				//};
				await _unitOfWork.CategoryRepository.Add(category);

				var createdCategory = await _unitOfWork.CategoryRepository.Get(
			e => e.Id == category.Id);


				return CreatedAtAction(nameof(GetCategory), new { id = createdCategory.Id }, _mapper.Map<CategoryResponseDTO>(createdCategory));
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
				Category? category = await _unitOfWork.CategoryRepository.Get(e => e.Id == id);
				if (category == null)
				{
					return NotFound($"Category with ID {id} not found.");
				}
				bool isDeleted = await _unitOfWork.CategoryRepository.Remove(category);
				if (!isDeleted)
				{
					return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting category.");
				}
				return Ok(_mapper.Map<CategoryResponseDTO>(category));
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
				Category category = _mapper.Map<Category>(categoryUpdateDTO);
				category.UpdatedAt = DateTime.UtcNow;
				//category.CreatedAt = categoryUpdateDTO.CreatedAt;
				//Category? category = new Category
				//{
				//	Id = categoryUpdateDTO.Id,
				//	Name = categoryUpdateDTO.Name,
				//	CreatedAt = categoryUpdateDTO.CreatedAt,
				//	UpdatedAt = DateTime.UtcNow
				//};
				if (category == null)
				{
					return NotFound($"Category with ID {id} not found.");
				}
				await _unitOfWork.CategoryRepository.Update(category);

				var updatedCategory = await _unitOfWork.CategoryRepository.Get(
			e => e.Id == category.Id);
				return Ok(_mapper.Map<CategoryResponseDTO>(updatedCategory));
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
				Category? category = await _unitOfWork.CategoryRepository.Get(e => e.Id == id, noTracking: true);
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
				Category updatedCategory = _mapper.Map<Category>(categoryUpdateDTO);
				updatedCategory.Id = id;
				await _unitOfWork.CategoryRepository.Update(updatedCategory);
				var patchedCategory = await _unitOfWork.CategoryRepository.Get(
			e => e.Id == updatedCategory.Id);
				return Ok(_mapper.Map<CategoryResponseDTO>(patchedCategory));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
			}
		}
	}
}
