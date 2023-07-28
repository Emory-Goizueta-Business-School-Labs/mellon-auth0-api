using System;
using MellonAPI.Data;
namespace MellonAPI.Models
{
	public class Author
	{
		public int Id { get; set; }
		public string Name { get; set; } = "";
		public List<Book> Books { get; set; } = new();

		public Author()
		{
		}
	}
}
