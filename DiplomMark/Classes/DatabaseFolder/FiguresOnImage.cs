using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomMark.Classes.DatabaseFolder
{
    public class FiguresOnImage
    {
        public int Id { get; set; }
        public double Coord_X { get; set; }
        public double Coord_Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string? URLToImage { get; set; }
        public string? NameFigure { get; set; }

    }
}
