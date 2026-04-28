using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
	public class TodoItem
	{
		public int Id { get; set; }
		[Required]
		[StringLength(100)]
		public string? Title { get; set; }
		public bool IsCompleted { get; set; }
		public int? CategoryId { get; set; }

		public Category? Category { get; set; }
	}
}
