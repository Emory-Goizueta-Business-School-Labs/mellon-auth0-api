using System;
using MellonAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MellonAPI.Data
{
	public class MellonAPIContext : DbContext
	{
		public DbSet<ToDo> ToDos { get; set; }

        public MellonAPIContext(DbContextOptions<MellonAPIContext> options)
			: base(options)
		{
		}
	}
}

