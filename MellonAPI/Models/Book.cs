using System;
namespace MellonAPI.Models
{
	public class Book
	{
		public int Id { get; set; }
		public string Title { get; set; } = "";

		public Author Author { get; set; } = null!;

		public Book()
		{
		}
	}
}

