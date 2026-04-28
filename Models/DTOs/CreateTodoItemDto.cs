using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.DTOs
{
	public class CreateTodoItemDto
	{
		[Required]
		[StringLength(100)]
		public string Title { get; set; } = string.Empty;
		public int? CategoryId { get; set; }
	}
}
