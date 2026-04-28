using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using TodoApi.Models.DTOs;

namespace TodoApi.Services
{
	public class TodoService : ITodoService
	{
		private readonly TodoContext _context;

		public TodoService(TodoContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<TodoItem>> GetAllTasksAsync()
		{
			return await _context.TodoItems.Include(t => t.Category).ToListAsync();
		}

		public async Task<TodoItem> AddTaskAsync(CreateTodoItemDto item)
		{
			if (item.CategoryId.HasValue)
			{
				var categoryExists = await _context.Categories.AnyAsync(c => c.Id == item.CategoryId.Value);
				if (!categoryExists)
				{
					return null;
				}
			}

			var newItem = new TodoItem
			{
				Title = item.Title,
				IsCompleted = false,
				CategoryId = item.CategoryId
			};

			_context.TodoItems.Add(newItem);
			await _context.SaveChangesAsync();
			return newItem;
		}
		public async Task<TodoItem?> GetTaskByIdAsync(int id)
		{
			return await _context.TodoItems.FirstOrDefaultAsync(x => x.Id == id);
		}

		public async Task<bool> UpdateTaskAsync(int id, TodoItemDto dto)
		{
			var result = await _context.TodoItems.FindAsync(id);
			if (result == null)
			{
				return false;
			}

			result.Title = dto.Title;
			result.IsCompleted = dto.IsCompleted;
			await _context.SaveChangesAsync();
			return true;
		}

		public async Task<bool> DeleteTaskAsync(int id)
		{
			var item = await _context.TodoItems.FindAsync(id);

			if (item == null)
			{
				return false;
			}

			_context.TodoItems.Remove(item);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
