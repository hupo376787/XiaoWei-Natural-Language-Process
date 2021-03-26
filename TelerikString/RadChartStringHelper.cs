using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Core;
using Windows.UI.Xaml;

namespace XiaoweiNLP.TelerikString
{
    //How to localize
    //ChartLocalizationManager.Instance.StringLoader = new RadChartStringHelper();

    public class RadChartStringHelper : IStringResourceLoader
    {
        public string GetString(string key)
        {
            if (!(Application.Current as App).strCurrentLanguage.ToLower().Equals("zh-cn"))
                return null;

            switch (key)
            {
                case "NoData":
                    return "无数据";
                case "NoHorizontalAxis":
                    return "缺少横坐标轴";
                case "NoVerticalAxis":
                    return "缺少纵坐标轴";
                case "NoAngleAxis":
                    return "缺少角度坐标轴";
                case "NoPolarAxis":
                    return "缺少极坐标轴";
                case "NoSeries":
                    return "缺少图例";
                default:
                    return null;
            }
        }
    }
}
