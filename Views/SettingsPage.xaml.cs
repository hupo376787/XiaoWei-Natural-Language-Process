using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Services.Store;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using XiaoweiNLP.Controls;
using XiaoweiNLP.Helpers;
using XiaoweiNLP.Services;

namespace XiaoweiNLP.Views
{
    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings-codebehind.md
    // TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
    public sealed partial class SettingsPage : Page, INotifyPropertyChanged
    {
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        public SettingsPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            toogleNeedCropImage.IsOn = (Application.Current as App).bNeedCropImage;

            comboBoxLanguage.Items.Add("auto Auto");
            comboBoxLanguage.Items.Add("en-us English");
            comboBoxLanguage.Items.Add("zh-cn 简体中文");

            comboBoxSegTagEngine.Items.Add("Boson AI");
            comboBoxSegTagEngine.Items.Add("Tencent AI");

            string strCurrentLanguage = (Application.Current as App).strCurrentLanguage;
            string strTempAutoLang = strCurrentLanguage;
            if (strCurrentLanguage.ToLower().Equals("auto"))
                strCurrentLanguage = LanguageHelper.GetCurLanguage();

            comboBoxLanguage.SelectedIndex = 0;
            if (!strTempAutoLang.Equals("auto"))
            {
                for (int i = 0; i < comboBoxLanguage.Items.Count; i++)
                {
                    string temp = comboBoxLanguage.Items[i].ToString();
                    string[] tempArr = temp.Split(' ');
                    if (strCurrentLanguage == tempArr[0])
                        comboBoxLanguage.SelectedIndex = i;
                }
            }

            if((Application.Current as App).strSegTagEngine == "")
                comboBoxSegTagEngine.SelectedIndex = 0;
            else
            {
                for (int i = 0; i < comboBoxSegTagEngine.Items.Count; i++)
                {
                    string temp = comboBoxSegTagEngine.Items[i].ToString();
                    if ((Application.Current as App).strSegTagEngine == temp)
                        comboBoxSegTagEngine.SelectedIndex = i;
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Initialize();
        }

        private async void Initialize()
        {
            VersionDescription = GetVersionDescription();

            if (Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported())
            {
                FeedbackLink.Visibility = Visibility.Visible;
            }

            await ReadAttachment();
        }

        private string GetVersionDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async void ThemeChanged_CheckedAsync(object sender, RoutedEventArgs e)
        {
            var param = (sender as RadioButton)?.CommandParameter;

            if (param != null)
            {
                await ThemeSelectorService.SetThemeAsync((ElementTheme)param);
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

        private async void FeedbackLink_Click(object sender, RoutedEventArgs e)
        {
            // This launcher is part of the Store Services SDK https://docs.microsoft.com/en-us/windows/uwp/monetize/microsoft-store-services-sdk
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }

        private void toogleNeedCropImage_Toggled(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["NeedCropImage"] = (sender as ToggleSwitch).IsOn.ToString();
            (Application.Current as App).bNeedCropImage = (sender as ToggleSwitch).IsOn;
        }

        private void comboBoxLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string temp = comboBoxLanguage.SelectedItem.ToString();
            string[] tempArr = temp.Split(' ');
            ApplicationData.Current.LocalSettings.Values["strCurrentLanguage"] = tempArr[0];
        }

        private void SegTagEngine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["SegTagEngine"] = comboBoxSegTagEngine.SelectedItem.ToString();
            (Application.Current as App).strSegTagEngine = comboBoxSegTagEngine.SelectedItem.ToString();
        }

        private async void MarkdownText_LinkClicked(object sender, Microsoft.Toolkit.Uwp.UI.Controls.LinkClickedEventArgs e)
        {
            if (e.Link.StartsWith("//"))
                await Launcher.LaunchUriAsync(new Uri(e.Link.Replace("//", "")));
            else
                await Launcher.LaunchUriAsync(new Uri(e.Link));
        }

        private async Task ReadAttachment()
        {
            StorageFile sf1;
            StorageFile sf2;
            if ((Application.Current as App).strCurrentLanguage.ToLower().Equals("zh-cn"))
            {
                sf1 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Attachment/NLP_zhcn.txt"));
                sf2 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Attachment/UpdateLog_zhcn.txt"));
            }
            else
            {
                sf1 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Attachment/NLP_en.txt"));
                sf2 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Attachment/UpdateLog_en.txt"));
            }

            using (Stream sr = await sf1.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(sr))
                {
                    textWhatsNLP.Text = await read.ReadToEndAsync();
                }
            }

            using (Stream sr = await sf2.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(sr))
                {
                    textUpdateLog.Text = await read.ReadToEndAsync();
                }
            }
        }

        private async void MoreApps_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("ms-windows-store://publisher/?name=小冰科技"));
        }

        private void Image_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DonateHelper.GetAddOnInfoAndShowDonateWindow();
        }
        
        private void Border_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            border.Width = border.Height = 66;
        }

        private void Border_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            border.Width = border.Height = 68;
        }

        private void Donate_Click(object sender, RoutedEventArgs e)
        {
            DonateHelper.GetAddOnInfoAndShowDonateWindow();
        }

    }
}
