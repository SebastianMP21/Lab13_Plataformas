using System;
using System.Collections.Generic;
using System.Text;
using Lab13.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab13.DataContext
{
    public class AppDbContext : DbContext
    {
        string DbPath = string.Empty;

        public AppDbContext(string dbPath)
        {
            this.DbPath = dbPath;
        }

        public DbSet<Artista> Artistas { get; set; }
        public DbSet<Album> Albumes { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={DbPath}");

        }
    }

}
