using XiaoweiNLP.Controls;
using XiaoweiNLP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace XiaoweiNLP.Services
{
    
    public class TipServices
    {
        private static NotifyPopup notifyPopup;
        private static string strCurrentLanguage = (Application.Current as App).strCurrentLanguage;
        
        public static void TipRecognizeSucceed()
        {
            if (notifyPopup != null)
                notifyPopup.Hide();
            if (strCurrentLanguage.ToLower().Equals("zh-cn"))
                notifyPopup = new NotifyPopup(LanguageHelper.strRecognizeSucceed_zhcn);
            else
                notifyPopup = new NotifyPopup(LanguageHelper.strRecognizeSucceed_en);
            notifyPopup.Show();
        }

        public static void TipRecognizeFailed()
        {
            if (notifyPopup != null)
                notifyPopup.Hide();
            if (strCurrentLanguage.ToLower().Equals("zh-cn"))
                notifyPopup = new NotifyPopup(LanguageHelper.strRecognizeFailed_zhcn);
            else
                notifyPopup = new NotifyPopup(LanguageHelper.strRecognizeFailed_en);
            notifyPopup.Show();
        }

        public static void TipNoInternet()
        {
            if (notifyPopup != null)
                notifyPopup.Hide();
            if (strCurrentLanguage.ToLower().Equals("zh-cn"))
                notifyPopup = new NotifyPopup(LanguageHelper.strNoInternet_zhcn);
            else
                notifyPopup = new NotifyPopup(LanguageHelper.strNoInternet_en);
            notifyPopup.Show();
        }

        public static void TipSystemError()
        {
            if (notifyPopup != null)
                notifyPopup.Hide();
            if (strCurrentLanguage.ToLower().Equals("zh-cn"))
                notifyPopup = new NotifyPopup(LanguageHelper.strSystemError_zhcn);
            else
                notifyPopup = new NotifyPopup(LanguageHelper.strSystemError_en);
            notifyPopup.Show();
        }

        public static void TipNotFlowersOrGrasses()
        {
            if (notifyPopup != null)
                notifyPopup.Hide();
            if (strCurrentLanguage.ToLower().Equals("zh-cn"))
                notifyPopup = new NotifyPopup(LanguageHelper.strNotFlowersOrGrasses_zhcn);
            else
                notifyPopup = new NotifyPopup(LanguageHelper.strNotFlowersOrGrasses_en);
            notifyPopup.Show();
        }

        public static void TipRecognizeResultNull()
        {
            if (notifyPopup != null)
                notifyPopup.Hide();
            if (strCurrentLanguage.ToLower().Equals("zh-cn"))
                notifyPopup = new NotifyPopup(LanguageHelper.strRecognizeResultNull_zhcn);
            else
                notifyPopup = new NotifyPopup(LanguageHelper.strRecognizeResultNull_en);
            notifyPopup.Show();
        }

    }
}
