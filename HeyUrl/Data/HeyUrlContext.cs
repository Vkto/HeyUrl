using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HeyUrl.Models;

namespace HeyUrl.Data
{
    public class HeyUrlContext : DbContext
    {
        public HeyUrlContext (DbContextOptions<HeyUrlContext> options): base(options)
        {
        }

        public DbSet<Url> Url { get; set; }

        public DbSet<Click> Click { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
  }

}
