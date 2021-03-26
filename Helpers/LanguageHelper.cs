using System.Collections.Generic;
using Windows.System.UserProfile;

namespace XiaoweiNLP.Helpers
{
    public class LanguageHelper
    {
        //多语言
        public static string GetCurLanguage()
        {
            try
            {
                var languages = GlobalizationPreferences.Languages;
                string ss = languages[0];
                if (languages.Count == 1 && (ss.ToLower() != "en-us" || ss.ToLower() != "zh-cn"))
                    return "en-us";
                string sss = languages[1];
                if (languages.Count > 0)
                {
                    List<string> lLang = new List<string>();
                    lLang.Add("zh-cn、zh、zh-Hans、zh-hans-cn、zh-sg、zh-hans-sg");
                    lLang.Add("en-us、en、en-au、en-ca、en-gb、en-ie、en-in、en-nz、en-sg、en-za、en-bz、en-hk、en-id、en-jm、en-kz、en-mt、en-my、en-ph、en-pk、en-tt、en-vn、en-zw、en-053、en-021、en-029、en-011、en-018、en-014");
                    for (int i = 0; i < lLang.Count; i++)
                    {
                        if (lLang[i].ToLower().Contains(languages[0].ToLower()))
                        {
                            string temp = lLang[i].ToLower();
                            string[] tempArr = temp.Split('、');

                            return tempArr[0];
                        }
                        //else
                        //    continue;
                    }
                }
                return "en-us";
            }
            catch
            {
                return "en-us";
            }
        }

        public static string strRecognizeSucceed_zhcn = "处理完毕";
        public static string strRecognizeSucceed_en = "Done";

        public static string strRecognizeFailed_zhcn = "处理失败啦，放心这个几率很小";
        public static string strRecognizeFailed_en = "Recognize failed";

        public static string strNoInternet_zhcn = "没有网络，请检查网络连接";
        public static string strNoInternet_en = "No internet, check your network";

        public static string strSystemError_zhcn = "系统错误，或网络超时";
        public static string strSystemError_en = "System error, or network time out";

        public static string strNotFlowersOrGrasses_zhcn = "介个不是汽车呢";
        public static string strNotFlowersOrGrasses_en = "It seems not a car";

        public static string strRecognizeResultNull_zhcn = "我暂时还不认识呢，或许过两天就知道了";
        public static string strRecognizeResultNull_en = "I can't recognize it currently, maybe two days later";

        public static string strStorePurchaseStatusSucceeded_zhcn = "购买成功";
        public static string strStorePurchaseStatusSucceeded_en = "Purchase succeeded";

        public static string strStorePurchaseStatusAlreadyPurchased_zhcn = "已经购买过了，请在一天后重试";
        public static string strStorePurchaseStatusAlreadyPurchased_en = "Already purchased, please try again one day later";

        public static string strStorePurchaseStatusNotPurchased_zhcn = "您取消了购买";
        public static string strStorePurchaseStatusNotPurchased_en = "You canceled the purchase";

        public static string strStorePurchaseStatusServerError_zhcn = "对不起，微软服务器掉链子了";
        public static string strStorePurchaseStatusServerError_en = "We are sorry, there's something wrong with Microsoft server";

        public static string strStorePurchaseStatusNetworkError_zhcn = "貌似网络不通畅";
        public static string strStorePurchaseStatusNetworkError_en = "It seems an error with your network";
    }
}
