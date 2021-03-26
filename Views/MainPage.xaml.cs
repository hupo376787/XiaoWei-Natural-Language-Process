using TencentAI;
using BosonNLP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using XiaoweiNLP.Helpers;
using XiaoweiNLP.Models;
using XiaoweiNLP.Services;
using Windows.Services.Store;
using XiaoweiNLP.Controls;
using Newtonsoft.Json;
using Microsoft.Toolkit.Uwp.Connectivity;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI;
using System.Reflection;
using Windows.UI.Xaml.Media;
using Telerik.UI.Xaml.Controls.Chart;
using XiaoweiNLP.TelerikString;

namespace XiaoweiNLP.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<NLPWord> SegTagItems { get; private set; } = new ObservableCollection<NLPWord>();
        public ObservableCollection<NLPWord> NamedEntityItems { get; private set; } = new ObservableCollection<NLPWord>();
        public ObservableCollection<NLPWord> KeywordsItems { get; private set; } = new ObservableCollection<NLPWord>();
        public ObservableCollection<NLPWord> SuggestItems { get; private set; } = new ObservableCollection<NLPWord>();


        private List<TencentAI.Contract.NaturalLanguage.BaseTokensp> listBaseTokens = new List<TencentAI.Contract.NaturalLanguage.BaseTokensp>();
        private List<TencentAI.Contract.NaturalLanguage.MixTokensp> listMixTokens = new List<TencentAI.Contract.NaturalLanguage.MixTokensp>();
        private TencentAI.Contract.NaturalLanguage.WordSegModel resTencentSeg = new TencentAI.Contract.NaturalLanguage.WordSegModel();
        private TencentAI.Contract.NaturalLanguage.WordPositionTaggingModel resTencentPosTag = new TencentAI.Contract.NaturalLanguage.WordPositionTaggingModel();
        private TencentAI.Contract.NaturalLanguage.WordProperNounsModel resTencentProperNouns = new TencentAI.Contract.NaturalLanguage.WordProperNounsModel();
        private TencentAI.Contract.NaturalLanguage.WordSynonymModel resTencentSynonym = new TencentAI.Contract.NaturalLanguage.WordSynonymModel();
        private TencentAI.Contract.NaturalLanguage.WordComponentModel resTencentComponent = new TencentAI.Contract.NaturalLanguage.WordComponentModel();
        private TencentAI.Contract.NaturalLanguage.WordEmotionPolarModel resTencentEmotionPolar = new TencentAI.Contract.NaturalLanguage.WordEmotionPolarModel();

        private BosonNLP.Contract.SegTagModel resBosonSegTag = null;
        private BosonNLP.Contract.NamedEntityModel resBosonNamedEntity = null;
        private BosonNLP.Contract.EmotionModel resBosonEmotion = null;
        private BosonNLP.Contract.ClassifyModel resBosonClassify = null;
        private BosonNLP.Contract.SummaryModel resBosonSummary = null;

        List<ChartDataModel> chartItems = new List<ChartDataModel>();

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            ChartLocalizationManager.Instance.StringLoader = new RadChartStringHelper();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame.BackStack.Clear();
            if ((Application.Current as App).strSegTagEngine == "Tencent AI")
            {
                ButtonTencentAIBaseTokens.Visibility = Visibility.Visible;
                ButtonTencentAIMixTokens.Visibility = Visibility.Visible;
                ButtonTencentAISegLegend.Visibility = Visibility.Visible;
                ButtonTencentAIEntityLegend.Visibility = Visibility.Visible;
                tencentClassify.Visibility = Visibility.Visible;

                ButtonBosonAIGo.Visibility = Visibility.Collapsed;
                ButtonBosonAISegLegend.Visibility = Visibility.Collapsed;
                ButtonBosonAIEntityLegend.Visibility = Visibility.Collapsed;
                bosonClassify.Visibility = Visibility.Collapsed;
            }
            else
            {
                ButtonTencentAIBaseTokens.Visibility = Visibility.Collapsed;
                ButtonTencentAIMixTokens.Visibility = Visibility.Collapsed;
                ButtonTencentAISegLegend.Visibility = Visibility.Collapsed;
                ButtonTencentAIEntityLegend.Visibility = Visibility.Collapsed;
                tencentClassify.Visibility = Visibility.Collapsed;

                ButtonBosonAIGo.Visibility = Visibility.Visible;
                ButtonBosonAISegLegend.Visibility = Visibility.Visible;
                ButtonBosonAIEntityLegend.Visibility = Visibility.Visible;
                bosonClassify.Visibility = Visibility.Visible;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));




        private async Task GetTencentAIResponse()
        {
            try
            {
                resTencentPosTag = await TencentAIHelper.WordPositionTagging(textInput.Text.Trim());
                if (resTencentPosTag != null && resTencentPosTag.ret == 0 && resTencentPosTag.msg == "ok")
                {
                    SegTagItems.Clear();
                    listBaseTokens = resTencentPosTag.data.base_tokens;
                    listMixTokens = resTencentPosTag.data.mix_tokens;
                }

                resTencentProperNouns = await TencentAIHelper.WordProperNouns(textInput.Text.Trim());
                if(resTencentProperNouns != null && resTencentProperNouns.ret == 0 && resTencentProperNouns.msg == "ok")
                {
                    NamedEntityItems.Clear();
                    foreach (var item in resTencentProperNouns.data.ner_tokens)
                    {
                        NLPWord nlp = new NLPWord
                        {
                            word = item.word,
                            width = item.length * 20,
                            bgcolor = NameEntityHelper.GetNameEntityColor_Tencent(item.types[0].ToString())
                        };
                        NamedEntityItems.Add(nlp);
                    }
                }

                resTencentSynonym = await TencentAIHelper.WordSynonym(textInput.Text.Trim());
                if (resTencentSynonym != null && resTencentSynonym.ret == 0 && resTencentSynonym.msg == "ok")
                {
                    SuggestItems.Clear();
                    foreach (var item in resTencentSynonym.data.syn_tokens)
                    {
                        NLPWord nlp = new NLPWord
                        {
                            word = item.ori_word.word,
                            width = item.ori_word.length * 20,
                            bgcolor = "#9acd32"
                        };
                        SuggestItems.Add(nlp);

                        foreach (var iitem in item.syn_words)
                        {
                            string strWeight = (Convert.ToInt32(iitem.weight * 100)).ToString();
                            NLPWord inlp = new NLPWord
                            {
                                word = iitem.word + "(" + strWeight + ")",
                                //width = item.ori_word.length * 20,
                                bgcolor = "#99CC67"
                            };
                            SuggestItems.Add(inlp);
                        }
                    }
                }

                resTencentComponent = await TencentAIHelper.WordComponent(textInput.Text.Trim());
                if (resTencentComponent != null && resTencentComponent.ret == 0 && resTencentComponent.msg == "ok")
                {
                    sliderTencentClassify.Value = resTencentComponent.data.intent + 0.5;
                }
                else if(resTencentComponent != null && resTencentComponent.ret == 16393)
                {
                    sliderTencentClassify.Value = 0.5;
                }

                resTencentEmotionPolar = await TencentAIHelper.WordEmotionPolar(textInput.Text.Trim());
                if (resTencentEmotionPolar != null && resTencentEmotionPolar.ret == 0 && resTencentEmotionPolar.msg == "ok")
                {
                    double positive = 0.0, negtive = 0.0; 
                    if (resTencentEmotionPolar.data.polar == 1)
                    {
                        positive = 1;
                        negtive = 0;
                    }
                    else if (resTencentEmotionPolar.data.polar == -1)
                    {
                        positive = 0;
                        negtive = 1;
                    }
                    else if (resTencentEmotionPolar.data.polar == 0)
                    {
                        positive = 1;
                        negtive = 1;
                    }

                    if ((Application.Current as App).strCurrentLanguage.ToLower().Equals("zh-cn"))
                    {
                        // Create a new chart data point for each value you want in the PieSeries
                        var sliceOne = new ChartDataModel { Value = positive, Title = "正面" };
                        var sliceTwo = new ChartDataModel { Value = negtive, Title = "负面" };

                        // Add those items to the list
                        chartItems.Add(sliceOne);
                        chartItems.Add(sliceTwo);
                    }
                    else
                    {
                        // Create a new chart data point for each value you want in the PieSeries
                        var sliceOne = new ChartDataModel { Value = positive, Title = "Positive" };
                        var sliceTwo = new ChartDataModel { Value = negtive, Title = "Negtive" };

                        // Add those items to the list
                        chartItems.Add(sliceOne);
                        chartItems.Add(sliceTwo);
                    }

                    MyDoughnutSeries.ItemsSource = chartItems;
                }

                TipServices.TipRecognizeSucceed();
            }
            catch
            {
                if (resTencentPosTag.ret < 0)
                    TipServices.TipSystemError();
                else
                    TipServices.TipRecognizeFailed();
            }
        }

        private async void OnBaseTokens(object sender, RoutedEventArgs e)
        {
            animationView.Visibility = Visibility.Visible;
            ResetAll();
            await GetTencentAIResponse();

            listBaseTokens.ForEach((x) =>
            {
                NLPWord nlp = new NLPWord
                {
                    word = x.word,
                    width = x.length * 20,
                    bgcolor = PosTagHelper.GetPosColor_Tencent(x.pos_code.ToString())
                };
                SegTagItems.Add(nlp);
            });
            animationView.Visibility = Visibility.Collapsed;
        }

        private async void OnMixTokens(object sender, RoutedEventArgs e)
        {
            animationView.Visibility = Visibility.Visible;
            ResetAll();
            await GetTencentAIResponse();

            listMixTokens.ForEach((x) =>
            {
                NLPWord nlp = new NLPWord
                {
                    word = x.word,
                    width = x.length * 20,
                    bgcolor = PosTagHelper.GetPosColor_Tencent(x.pos_code.ToString())
                };
                SegTagItems.Add(nlp);
            });
            animationView.Visibility = Visibility.Collapsed;
        }



        private void ResetAll()
        {
            SegTagItems.Clear();
            NamedEntityItems.Clear();
            chartItems.Clear();
            sliderClassify.Value = sliderTencentClassify .Value = 0;
            textSummary.Text = "";
            KeywordsItems.Clear();
            SuggestItems.Clear();
        }

        private void OnClearContent(object sender, RoutedEventArgs e)
        {
            textInput.Text = "";
            SegTagItems.Clear();
            listBaseTokens.Clear();
            listMixTokens.Clear();
            resTencentSeg = null;
            resBosonSegTag = null;
        }

        private async void OnExportJson(object sender, RoutedEventArgs e)
        {
            if((Application.Current as App).strSegTagEngine == "Tencent AI")
            {
                if (resTencentSeg == null)
                    return;

                FileSavePicker saveFile = new FileSavePicker();
                saveFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                saveFile.FileTypeChoices.Add("json", new List<string> { ".json" });
                saveFile.SuggestedFileName = DateTime.Now.ToFileTime().ToString();
                StorageFile sFile = await saveFile.PickSaveFileAsync();
                if (sFile != null)
                {
                    string strContent = JsonConvert.SerializeObject(resTencentSeg);
                    await FileIO.WriteTextAsync(sFile, strContent);
                }
            }
            else
            {
                if (resBosonSegTag == null)
                    return;

                FileSavePicker saveFile = new FileSavePicker();
                saveFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                saveFile.FileTypeChoices.Add("json", new List<string> { ".json" });
                saveFile.SuggestedFileName = DateTime.Now.ToFileTime().ToString();
                StorageFile sFile = await saveFile.PickSaveFileAsync();
                if (sFile != null)
                {
                    string strContent = JsonConvert.SerializeObject(resBosonSegTag);
                    await FileIO.WriteTextAsync(sFile, strContent);
                }
            }
        }

        private void OnOpenLegend(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void Image_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DonateHelper.GetAddOnInfoAndShowDonateWindow();
        }

        private void Border_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            border.Width = border.Height = 48;
        }

        private void Border_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            border.Width = border.Height = 50;
        }




        private async void OnBosonAIGo(object sender, RoutedEventArgs e)
        {
            animationView.Visibility = Visibility.Visible;
            ResetAll();

            try
            {
                await OnSegTag();
                await OnNamedEntityRecognize();
                await OnEmotionAnalysis();
                await OnClassify();
                await OnSummary();
                await OnKeywords();
                await OnGetSuggest();
            }
            catch
            {
                TipServices.TipRecognizeFailed();
            }

            animationView.Visibility = Visibility.Collapsed;
            TipServices.TipRecognizeSucceed();
        }

        private async Task OnSegTag()
        {
            resBosonSegTag = await BosonAIHelper.WordSegAndTag(textInput.Text.Trim());
            if (resBosonSegTag != null && resBosonSegTag.tag.Count > 0)
            {
                for (int i = 0; i <= resBosonSegTag.tag.Count - 1; i++)
                {
                    NLPWord nlp = new NLPWord
                    {
                        word = resBosonSegTag.word[i],
                        width = resBosonSegTag.word[i].Length * 20,
                        bgcolor = PosTagHelper.GetPosColor_Boson(resBosonSegTag.tag[i])
                    };
                    SegTagItems.Add(nlp);
                }
            }
        }

        private async Task OnNamedEntityRecognize()
        {
            try
            {
                resBosonNamedEntity = await BosonAIHelper.NamedEntityRecognize(textInput.Text.Trim());
                if (resBosonNamedEntity.entity.Count > 0)
                {
                    for (int i = 0; i <= resBosonNamedEntity.entity.Count - 1; i++)
                    {
                        int nStart = Convert.ToInt32(resBosonNamedEntity.entity[i][0]);
                        int nEnd = Convert.ToInt32(resBosonNamedEntity.entity[i][1]);
                        List<string> list = resBosonNamedEntity.word.Skip(nStart).Take(nEnd - nStart).ToList();
                        string strEntity = "";
                        foreach (var item in list)
                        {
                            strEntity += item;
                        }

                        NLPWord nlp = new NLPWord
                        {
                            word = strEntity,
                            width = strEntity.Length * 20,
                            bgcolor = NameEntityHelper.GetNameEntityColor_Boson(resBosonNamedEntity.entity[i][2])
                        };
                        NamedEntityItems.Add(nlp);
                    }
                }
            }
            catch { }
        }

        private async Task OnEmotionAnalysis()
        {
            resBosonEmotion = await BosonAIHelper.EmotionAnalysis(textInput.Text.Trim());
            if (resBosonEmotion != null)
            {
                if ((Application.Current as App).strCurrentLanguage.ToLower().Equals("zh-cn"))
                {
                    // Create a new chart data point for each value you want in the PieSeries
                    var sliceOne = new ChartDataModel { Value = resBosonEmotion.positive, Title = "正面" };
                    var sliceTwo = new ChartDataModel { Value = resBosonEmotion.negtive, Title = "负面" };

                    // Add those items to the list
                    chartItems.Add(sliceOne);
                    chartItems.Add(sliceTwo);
                }
                else
                {
                    // Create a new chart data point for each value you want in the PieSeries
                    var sliceOne = new ChartDataModel { Value = resBosonEmotion.positive, Title = "Positive" };
                    var sliceTwo = new ChartDataModel { Value = resBosonEmotion.negtive, Title = "Negtive" };

                    // Add those items to the list
                    chartItems.Add(sliceOne);
                    chartItems.Add(sliceTwo);
                }

                MyDoughnutSeries.ItemsSource = chartItems;
            }
        }

        private async Task OnClassify()
        {
            resBosonClassify = await BosonAIHelper.ClassifyNews(textInput.Text.Trim());
            if (resBosonClassify != null)
            {
                sliderClassify.Value = resBosonClassify.area + 0.5;
            }
        }

        private async Task OnSummary()
        {
            string res = await BosonAIHelper.SummaryAnalysis(textInput.Text.Trim());
            if(res != null)
            {
                textSummary.Text = res;
            }
        }

        private async Task OnKeywords()
        {
            List<string> list = await BosonAIHelper.GetKeywords(textInput.Text.Trim());
            if (list != null && list.Count > 0)
            {
                var brush = (SolidColorBrush)Application.Current.Resources["SystemControlHighlightListAccentLowBrush"];

                for (int i = 0; i <= list.Count - 1; i++)
                {
                    string[] arr = list[i].Split(',');
                    int weight = Convert.ToInt32(Convert.ToDouble(arr[0]) * 100);
                    
                    NLPWord nlp = new NLPWord
                    {
                        word = arr[1] + " (" + weight + ")",
                        //width = resBosonSegTag.word[i].Length * 20,
                        bgcolor = "#FFCB99"//brush.Color.ToString()
                    };
                    KeywordsItems.Add(nlp);
                }
            }
        }

        private async Task OnGetSuggest()
        {
            List<string> list = await BosonAIHelper.GetSuggest(textInput.Text.Trim());
            if (list != null && list.Count > 0)
            {
                var brush = (SolidColorBrush)Application.Current.Resources["SystemControlHighlightListAccentLowBrush"];

                for (int i = 0; i <= list.Count - 1; i++)
                {
                    string[] arr = list[i].Split(',');
                    int relative = Convert.ToInt32(Convert.ToDouble(arr[0]) * 100);

                    NLPWord nlp = new NLPWord
                    {
                        word = arr[1] + " (" + relative + ")",
                        //width = resBosonSegTag.word[i].Length * 20,
                        bgcolor = "#99CC67"//brush.Color.ToString()
                    };
                    SuggestItems.Add(nlp);
                }
            }
        }

    }
}
