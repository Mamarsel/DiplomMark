

namespace UnitTests
{
    internal class UnitTests
    {
        //public int TestAIPhoto(string path)
        //{
        //    if (path == null) return 0;
           
        //    using var yolo = new Classes.Yolo.Models.PredictiorHelper(Directory.GetCurrentDirectory() + @"\Assets\Weights\yolov8m.onnx");
        //    Image img = Image.FromFile(path);
        //    var predictions = yolo.Predict(img);
        //    return predictions.Count();
        //}
        //public int CountPhotosInDirectory(string path)
        //{
        //    if (path == null) return 0; 
            
        //    string[] FilesJpg = Directory.GetFiles(path, "*.jpg");
        //    string[] FilesPng = Directory.GetFiles(path, "*.png");
        //    return FilesJpg.Concat(FilesPng).ToArray().Count();
        //}
        //public bool AddTags(string NameTag, Color colorTag)
        //{
        //    if(NameTag == null && colorTag == null) return false;

        //    TagClass tag = new TagClass { TagColor = new SolidColorBrush(colorTag), TagName = NameTag };
        //    GlobalVars.Tags.Add(tag);
        //    if (GlobalVars.Tags.Contains(tag)) return true;
            
        //    return false;
        //}
    }
}
