using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomMark.Classes.DatabaseFolder
{
    public class ApplicationContext : DbContext
    {
        public DbSet<FiguresOnImage> figuresOnImages { get; set; } = null!;
        public DbSet<CrashReports> crashReports { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DiplomMark.db");
        }
    }
}
