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

        public double coord_x { get; set; }
        public double coord_y { get; set; }
        public int Id { get; set; }
        public string name { get; set; }

        public string toFileName { get; set; }
       
        public Brush colorFill { get; set; }
        public string TypeFigure { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Shape shape { get; set; }

        public double width { get; set; }
        public double height { get; set; }
        public double opacity { get; set; }
        public Brush StrokeFill { get; set; }
        
    }
}
