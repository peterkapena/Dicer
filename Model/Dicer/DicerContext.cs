using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dicer.Model.Dicer
{
    public class DicerContext : DbContext
    {
        public DicerContext(DbContextOptions<DicerContext> options)
               : base(options)
        {
        }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<TestData>().ToTable("TestData");
        }
    }
}
