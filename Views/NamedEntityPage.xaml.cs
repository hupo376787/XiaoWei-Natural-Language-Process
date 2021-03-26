using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using XiaoweiNLP.Helpers;
using XiaoweiNLP.Models;

namespace XiaoweiNLP.Views
{
    public sealed partial class NamedEntityPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<NLPWord> SampleItems { get; private set; } = new ObservableCollection<NLPWord>();

        private BosonNLP.Contract.NamedEntityModel resBoson = null;

        public NamedEntityPage()
        {
            InitializeComponent();
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

        private async void OnExportJson(object sender, RoutedEventArgs e)
        {
            if (resBoson == null)
                return;

            FileSavePicker saveFile = new FileSavePicker();
            saveFile.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            saveFile.FileTypeChoices.Add("json", new List<string> { ".json" });
            saveFile.SuggestedFileName = DateTime.Now.ToFileTime().ToString();
            StorageFile sFile = await saveFile.PickSaveFileAsync();
            if (sFile != null)
            {
                string strContent = JsonConvert.SerializeObject(resBoson);
                await FileIO.WriteTextAsync(sFile, strContent);
            }
        }

        private void OnOpenLegend(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void OnClearContent(object sender, RoutedEventArgs e)
        {
            textInput.Text = "";
            SampleItems.Clear();
            resBoson = null;
        }

        private async void OnBosonAINamedEntityRecognize(object sender, RoutedEventArgs e)
        {
            animationView.Visibility = Visibility.Visible;
            try
            {
                resBoson = await BosonAIHelper.NamedEntityRecognize(textInput.Text.Trim());
                if (resBoson.entity.Count > 0)
                {
                    SampleItems.Clear();
                    for (int i = 0; i <= resBoson.entity.Count - 1; i++)
                    {
                        int nStart = Convert.ToInt32(resBoson.entity[i][0]);
                        int nEnd = Convert.ToInt32(resBoson.entity[i][1]);
                        List<string> list = resBoson.word.Skip(nStart).Take(nEnd - nStart).ToList();
                        string strEntity = "";
                        foreach(var item in list)
                        {
                            strEntity += item;
                        }

                        NLPWord nlp = new NLPWord
                        {
                            word = strEntity,
                            width = strEntity.Length * 20,
                            bgcolor = NameEntityHelper.GetNameEntityColor_Boson(resBoson.entity[i][2])
                        };
                        SampleItems.Add(nlp);
                    }
                }
            }
            catch { }
            animationView.Visibility = Visibility.Collapsed;
        }
    }
}
