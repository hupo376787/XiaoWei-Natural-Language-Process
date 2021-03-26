using Microsoft.Toolkit.Uwp.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XiaoweiNLP.Models;
using XiaoweiNLP.Services;

namespace XiaoweiNLP.Helpers
{
    public class BosonAIHelper
    {
        public async static Task<BosonNLP.Contract.SegTagModel> WordSegAndTag(string input)
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
                return await BosonNLP.Single.SegTag.WordSegAndTag(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }

        public async static Task<BosonNLP.Contract.NamedEntityModel> NamedEntityRecognize(string input)
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
                return await BosonNLP.Single.NamedEntity.NamedEntityRecognize(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }

        public async static Task<BosonNLP.Contract.EmotionModel> EmotionAnalysis(string input)
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
                return await BosonNLP.Single.Emotion.EmotionAnalysis(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }

        public async static Task<BosonNLP.Contract.DependencyGrammarModel> DependencyGrammar(string input)
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
                return await BosonNLP.Single.DependencyGrammar.DependencyGrammarAnalysis(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }

        public async static Task<BosonNLP.Contract.ClassifyModel> ClassifyNews(string input)
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
                return await BosonNLP.Single.Classify.ClassifyNews(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }

        public async static Task<List<string>> GetKeywords(string input)
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
                return await BosonNLP.Single.Keywords.GetKeywords(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }

        public async static Task<List<string>> GetSuggest(string input)
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
                return await BosonNLP.Single.Suggest.GetSuggest(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }

        public async static Task<string> SummaryAnalysis(string input)
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
                return await BosonNLP.Single.Summary.SummaryAnalysis(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }

        public async static Task<BosonNLP.Contract.TimeModel> TimeConvertor(string input)
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
                return await BosonNLP.Single.Time.TimeConvertor(input);
            }
            catch
            {
                TipServices.TipSystemError();
                return null;
            }
        }
    }
}
