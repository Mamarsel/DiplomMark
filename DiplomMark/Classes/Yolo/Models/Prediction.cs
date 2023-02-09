using System.Drawing;

namespace DiplomMark.Classes.Yolo.Models
{
    public class Prediction
    {
        public Label? Label { get; init; }
        public RectangleF Rectangle { get; init; }
        public float Score { get; init; }
    }
}
