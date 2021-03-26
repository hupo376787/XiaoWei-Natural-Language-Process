using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using XiaoweiNLP.Models;

namespace XiaoweiNLP.Helpers
{
    public class NameEntityHelper
    {
        public static string GetNameEntityColor_Boson(string entityEn)
        {
            NameEntity ne = JsonConvert.DeserializeObject<NameEntity>((Application.Current as App).jsonNamedEntityTag_Boson);
            for (int i = 0; i <= ne.en.Count - 1; i++)
            {
                if (entityEn == ne.en[i])
                    return ne.bgcolor[i];
            }

            return "";
        }

        public static string GetNameEntityColor_Tencent(string entityEn)
        {
            NameEntity ne = JsonConvert.DeserializeObject<NameEntity>((Application.Current as App).jsonNamedEntityTag_Tencent);
            for (int i = 0; i <= ne.en.Count - 1; i++)
            {
                if (entityEn == ne.en[i])
                    return ne.bgcolor[i];
            }

            return "";
        }
    }
}
