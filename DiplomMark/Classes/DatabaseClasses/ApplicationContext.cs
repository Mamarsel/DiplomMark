using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomMark.Classes.DatabaseClasses
{
    public class ApplicationContext : DbContext
    {
        public DbSet<RecentProjects> RecentProject { get; set; } = null!;
        public DbSet<SettingsUser> SettingsUser { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=helloapp.db");
        }
    }
}
