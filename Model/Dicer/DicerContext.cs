using IniParser;
using IniParser.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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

        public DbSet<Person> Person { get; set; }
        public DbSet<Device> Device { get; set; }
        public DbSet<Gamed> Gamed { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("Person");
            modelBuilder.Entity<Device>().ToTable("Device");
            modelBuilder.Entity<Gamed>().ToTable("Gamed");
        }
    }

    public class Data
    {
        private string _cnstring;
        public string CnString
        {
            get { return _cnstring; }
            set { _cnstring = value; }
        }
        public Data(IConfiguration config)
        {
            var database = config.GetSection("SQL").GetSection("DataBase").Value;
            var server = config.GetSection("SQL").GetSection("Server").Value;

            _cnstring = BuildCnString(ref database, ref server);
        }

        private string BuildCnString(ref string db, ref string srvr)
        {
            return $"Server={srvr};Database={db};Trusted_Connection=True;MultipleActiveResultSets=true";
        }
    }

}
