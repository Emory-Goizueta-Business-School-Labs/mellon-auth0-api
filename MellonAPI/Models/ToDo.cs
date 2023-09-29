using System;
namespace MellonAPI.Models
{
	public class ToDo
	{
		public int Id { get; set; }
		public string Text { get; set; } = "";
		public bool IsDone { get; set; } = false;

		public ToDo()
		{
		}
	}
}

