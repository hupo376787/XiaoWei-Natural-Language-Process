using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using XiaoweiNLP.Helpers;
using XiaoweiNLP.Services;

namespace XiaoweiNLP
{
    public sealed partial class App : Application
    {
        public string strCurrentLanguage = "en-us";
        public string strSegTagEngine = "";
        public bool bNeedCropImage = false;
        public Popup gPopup;
        public string jsonPosTag_Boson = "";
        public string jsonNamedEntityTag_Boson = "";
        public string jsonPosTag_Tencent = "";
        public string jsonNamedEntityTag_Tencent = "";

        private Lazy<ActivationService> _activationService;
        private ApplicationDataContainer appLocalSettings = ApplicationData.Current.LocalSettings;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {
            if (ApplicationData.Current.LocalSettings.Values["strCurrentLanguage"] != null)
            {
                strCurrentLanguage = ApplicationData.Current.LocalSettings.Values["strCurrentLanguage"].ToString();
                if (strCurrentLanguage == "auto")
                {
                    ApplicationLanguages.PrimaryLanguageOverride = LanguageHelper.GetCurLanguage();
                }
                else
                    ApplicationLanguages.PrimaryLanguageOverride = strCurrentLanguage;
            }
            else
            {
                ApplicationLanguages.PrimaryLanguageOverride = strCurrentLanguage = LanguageHelper.GetCurLanguage();
                //ApplicationLanguages.PrimaryLanguageOverride = strCurrentLanguage = "en-us";
            }

            InitializeComponent();

            gPopup = new Popup();

            if (appLocalSettings.Values["SegTagEngine"] != null)
                strSegTagEngine = appLocalSettings.Values["SegTagEngine"].ToString();
            else
                strSegTagEngine = "Boson AI";
            if (appLocalSettings.Values["NeedCropImage"] != null)
                bNeedCropImage = Convert.ToBoolean(appLocalSettings.Values["NeedCropImage"].ToString());
            else
                bNeedCropImage = false;

            LoadJson();

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(Views.MainPage), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            return new Views.ShellPage();
        }

        private async Task LoadJson()
        {
            StorageFile sf = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Models/PosTag_Boson.json"));
            using (Stream sr = await sf.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(sr))
                {
                    jsonPosTag_Boson = await read.ReadToEndAsync();
                }
            }

            sf = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Models/NameEntity_Boson.json"));
            using (Stream sr = await sf.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(sr))
                {
                    jsonNamedEntityTag_Boson = await read.ReadToEndAsync();
                }
            }

            sf = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Models/PosTag_Tencent.json"));
            using (Stream sr = await sf.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(sr))
                {
                    jsonPosTag_Tencent = await read.ReadToEndAsync();
                }
            }

            sf = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Models/NameEntity_Tencent.json"));
            using (Stream sr = await sf.OpenStreamForReadAsync())
            {
                using (StreamReader read = new StreamReader(sr))
                {
                    jsonNamedEntityTag_Tencent = await read.ReadToEndAsync();
                }
            }
        }
    }
}
