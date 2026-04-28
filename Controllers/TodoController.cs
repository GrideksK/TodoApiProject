using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Models.DTOs;
using TodoApi.Services;

namespace TodoApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TodoController : ControllerBase
	{
		private readonly ITodoService _todoService;

		public TodoController(ITodoService todoService)
		{
			_todoService = todoService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetAll()
		{
			var items = await _todoService.GetAllTasksAsync();
			var dtos = items.Select(item => new TodoItemDto
			{
				Id = item.Id,
				Title = item.Title,
				IsCompleted = item.IsCompleted,
				CategoryId = item.CategoryId,
				CategoryName = item.Category?.Name
			});
			
			return Ok(dtos);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<TodoItemDto>> GetById(int id)
		{
			var item = await _todoService.GetTaskByIdAsync(id);

			if (item == null) return NotFound();

			var dto = new TodoItemDto
			{
				Id = item.Id,
				Title = item.Title,
				IsCompleted = item.IsCompleted
			};

			return Ok(dto);
		}

		[HttpPost]
		public async Task<IActionResult> Add([FromBody] CreateTodoItemDto item)
		{
			var result = await _todoService.AddTaskAsync(item);
			if (result == null) return BadRequest();

			var outputDto = new TodoItemDto
			{
				Id = result.Id,
				Title = result.Title,
				IsCompleted = result.IsCompleted,
				CategoryId = result.CategoryId,
			};

			return CreatedAtAction(nameof(GetById), new { id = outputDto.Id }, outputDto);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] TodoItemDto dto)
		{
			var success = await _todoService.UpdateTaskAsync(id, dto);

			if (!success)
			{
				return NotFound(); 
			}

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var result = await _todoService.DeleteTaskAsync(id);

			if (!result) return NotFound();

			return NoContent();
		}
	}
}
