using LogCommon;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GUI.utils
{
    public class SQL : DbContext
    {
        private static string CONNECTION_STRING = "Server=localhost; User ID=root; Password=asd123; Database=ServerHandlerGUI";

        public SQL() : base(new DbContextOptionsBuilder
            ().UseMySql(CONNECTION_STRING, ServerVersion.AutoDetect(CONNECTION_STRING)).Options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { }

        public DbSet<LogEntry> logEntries { get; set; }
    }
}
