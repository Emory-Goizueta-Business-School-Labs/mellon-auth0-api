using System;
using MellonAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MellonAPI.Data
{
	public class MellonAPIContext : DbContext
	{
		public DbSet<Author> Authors { get; set; }
		public DbSet<Book> Books { get; set; }

		public MellonAPIContext(DbContextOptions<MellonAPIContext> options)
			: base(options)
		{
		}
	}
}

