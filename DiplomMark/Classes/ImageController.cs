using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DiplomMark.Classes
{
    static class ImageController
    {
        /// <summary>
        /// Класс для работы с фотографией
        /// </summary>
        public static byte[] getJPGFromImageControl(BitmapImage imageC)
        {
            MemoryStream memStream = new();
            JpegBitmapEncoder encoder = new();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }
        public static BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                BitmapImage image = new();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        public static BitmapImage FromFile(string file)
        {
            FileStream fileStream = new(file, FileMode.Open, FileAccess.Read);
            BitmapImage img = new();
            img.BeginInit();
            img.StreamSource = fileStream;
            img.EndInit();
            return img;
        }

        public static byte[] GetByteFileFromExplorer()
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Image Files|*.jpg; *.jpeg; *.png;"
            };
            openFileDialog.ShowDialog();
            string file = openFileDialog.FileName;
            if (file == "")
                return null;
            byte[] data = File.ReadAllBytes(file);
            return data;

        }
        public static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null ||
                imageData.Length == 0)
                return null;

            BitmapImage image = new();
            using (MemoryStream mem = new(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption   = BitmapCacheOption.OnLoad;
                image.UriSource     = null;
                image.StreamSource  = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
        //Уменьшение DPI фото до 96-ти
        public static BitmapSource correctImage(List<string> paths, int counterImage)
        {
            try
            {
                var img = FromFile(paths[counterImage]);
                double dpi = 96;
                int width = img.PixelWidth;
                int height = img.PixelHeight;

                int stride = width * 4; 
                byte[] pixelData = new byte[stride * height];
                img.CopyPixels(pixelData, stride, 0);

                BitmapSource bmpSource = BitmapSource.Create(width, height, dpi, dpi, PixelFormats.Bgra32, null, pixelData, stride);
                return bmpSource;
            }
            catch { return null; }
        }



    }
}
