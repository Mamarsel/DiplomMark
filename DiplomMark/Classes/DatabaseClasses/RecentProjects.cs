using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomMark.Classes.DatabaseClasses
{
    public class RecentProjects
    {
        [Key]
        public string FolderName { get; set; }
        public string? FullPath { get; set; }
        public DateTime LastModify { get; set; }
    }
}
