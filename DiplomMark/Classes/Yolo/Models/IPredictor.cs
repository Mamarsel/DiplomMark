using System;
using System.Drawing;

namespace DiplomMark.Classes.Yolo.Models
{
    public interface IPredictor
        : IDisposable
    {
        string? InputColumnName { get; }
        string? OutputColumnName { get; }

        int ModelInputHeight { get; }
        int ModelInputWidth { get; }

        int ModelOutputDimensions { get; }

        Prediction[] Predict(Image img);
    }
}
