using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiplomMark.Classes
{
    abstract public class Figure
    {

        public double Coord_X { get; set; }
        public double Coord_Y { get; set; }
      
        public string NameFigure { get; set; }

        public string ToFileName { get; set; }
       
        public Brush ColorFill { get; set; }
        public string TypeFigure { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Shape ShapeFigure { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
        public double FigureOpacity { get; set; }
        public Brush StrokeFill { get; set; }
        
    }
}
