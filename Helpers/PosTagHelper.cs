using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using XiaoweiNLP.Models;

namespace XiaoweiNLP.Helpers
{
    public class PosTagHelper
    {
        public static string GetPosColor_Boson(string posEn)
        {
            PosTag pt = JsonConvert.DeserializeObject<PosTag>((Application.Current as App).jsonPosTag_Boson);
            for(int i = 0; i <= pt.zh.Count - 1; i++)
            {
                if (posEn.StartsWith(pt.en[i]))
                    return pt.bgcolor[i];
            }

            return "";
        }

        public static string GetPosColor_Tencent(string posEn)
        {
            PosTag pt = JsonConvert.DeserializeObject<PosTag>((Application.Current as App).jsonPosTag_Tencent);
            for (int i = 0; i <= pt.zh.Count - 1; i++)
            {
                if (posEn == pt.en[i])
                    return pt.bgcolor[i];
            }

            return "";
        }
    }
}
