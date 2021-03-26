using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaoweiNLP.Services;

namespace XiaoweiNLP.Helpers
{
    public class TencentAIHelper
    {
        public async static Task<TencentAI.Contract.NaturalLanguage.WordSegModel> WordSeg(string input)
        {
            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                TipServices.TipNoInternet();
                return null;
            }
            if (input == "")
                return null;

            try
            {
                return await TencentAI.NaturalLanguage.BaseTextAnalyze.WordSeg(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }

        public async static Task<TencentAI.Contract.NaturalLanguage.WordPositionTaggingModel> WordPositionTagging(string input)
        {
            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                TipServices.TipNoInternet();
                return null;
            }
            if (input == "")
                return null;

            try
            {
                return await TencentAI.NaturalLanguage.BaseTextAnalyze.WordPositionTagging(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }

        public async static Task<TencentAI.Contract.NaturalLanguage.WordProperNounsModel> WordProperNouns(string input)
        {
            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                TipServices.TipNoInternet();
                return null;
            }
            if (input == "")
                return null;

            try
            {
                return await TencentAI.NaturalLanguage.BaseTextAnalyze.WordProperNounsTagging(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }

        public async static Task<TencentAI.Contract.NaturalLanguage.WordSynonymModel> WordSynonym(string input)
        {
            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                TipServices.TipNoInternet();
                return null;
            }
            if (input == "")
                return null;

            try
            {
                return await TencentAI.NaturalLanguage.BaseTextAnalyze.WordSynonymAnalyzing(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }

        public async static Task<TencentAI.Contract.NaturalLanguage.WordComponentModel> WordComponent(string input)
        {
            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                TipServices.TipNoInternet();
                return null;
            }
            if (input == "")
                return null;

            try
            {
                return await TencentAI.NaturalLanguage.SemanticAnalyze.IntentionComponentIdentification(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }


        public async static Task<TencentAI.Contract.NaturalLanguage.WordEmotionPolarModel> WordEmotionPolar(string input)
        {
            if (!NetworkHelper.Instance.ConnectionInformation.IsInternetAvailable)
            {
                TipServices.TipNoInternet();
                return null;
            }
            if (input == "")
                return null;

            try
            {
                return await TencentAI.NaturalLanguage.EmotionAnalyze.EmotionPolarAnalyze(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }

    }
}
