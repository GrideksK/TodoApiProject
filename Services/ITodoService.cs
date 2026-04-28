using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Models.DTOs;

namespace TodoApi.Services
{
	public interface ITodoService 
	{
		Task<IEnumerable<TodoItem>> GetAllTasksAsync();
		Task<TodoItem> AddTaskAsync(CreateTodoItemDto item);
		Task<TodoItem?> GetTaskByIdAsync(int id);
		Task<bool> UpdateTaskAsync(int id, TodoItemDto dto);
		Task<bool> DeleteTaskAsync(int id);
	}
}
