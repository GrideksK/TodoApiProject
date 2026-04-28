using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Models.DTOs;

namespace TodoApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly TodoContext _todoContext;

		public CategoryController(TodoContext todoContext)
		{
			_todoContext = todoContext;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
		{
			var categories = await _todoContext.Categories
				.Include(c => c.Items)
				.ToListAsync();
			var result = categories.Select(category => new CategoryDto
			{
				Id = category.Id,
				Name = category.Name,
				TasksCount = category.Items.Count
			});

			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<CategoryDto>> GetById(int id)
		{
			var category = await _todoContext.Categories
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.Id == id);
			if (category == null) return NotFound();
			var result = new CategoryDto
			{
				Id = category.Id,
				Name = category.Name,
				TasksCount = category.Items.Count
			};
			return Ok(result);
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromBody] CreateCategoryDto dto)
		{
			if (string.IsNullOrEmpty(dto.Name))
			{
				return BadRequest("Название категории не может быть пустым");
			}

			var category = new Category
			{
				Name = dto.Name
			};

			_todoContext.Categories.Add(category);
			await _todoContext.SaveChangesAsync();

			var resultDto = new CategoryDto
			{
				Id = category.Id,
				Name = category.Name
			};

			return CreatedAtAction(nameof(GetAll), new { id = resultDto.Id }, resultDto);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] CreateCategoryDto dto)
		{
			var category = await _todoContext.Categories.FindAsync(id);
			if (category == null) return NotFound();
			if (string.IsNullOrEmpty(dto.Name))
			{
				return BadRequest("Название категории не может быть пустым");
			}
			category.Name = dto.Name;
			await _todoContext.SaveChangesAsync();
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id, [FromQuery] bool deleteAllTasks = false)
		{
			var category = await _todoContext.Categories
				.Include(c => c.Items)
				.FirstOrDefaultAsync(c => c.Id == id);

			if (category == null) return NotFound();

			if (deleteAllTasks)
			{
				_todoContext.TodoItems.RemoveRange(category.Items);
			}
			else
			{
				foreach (var item in category.Items)
				{
					item.CategoryId = null;
				}
			}

			_todoContext.Categories.Remove(category);
			await _todoContext.SaveChangesAsync();

			return NoContent();
		}
	}
}
