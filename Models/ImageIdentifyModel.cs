using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace XiaoweiNLP.Models
{
    public class ImageIdentifyModel
    {
        public DateTime RecognizeTime { get; set; }
        public string Name { get; set; }
        public double Confidence { get; set; }
        public string ConfidenceDescription { get; set; }
        public BitmapImage Image { get; set; }
    }
}
