namespace TodoApi.Models.DTOs
{
	public class TodoItemDto
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public bool IsCompleted { get; set; }
		public int? CategoryId { get; set; }
		public string? CategoryName { get; set; }
	}
}
