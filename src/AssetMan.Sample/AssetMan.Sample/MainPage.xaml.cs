using System.ComponentModel;
using FFImageLoading.Forms;
using Xamarin.Forms;

namespace AssetMan.Sample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage
    {
        public static readonly BindableProperty AutoIncludedLogoLoadedSuccessfullyProperty =
            BindableProperty.Create(nameof(AutoIncludedLogoLoadedSuccessfully), typeof(bool), typeof(MainPage), default(bool));

        public static readonly BindableProperty ManuallyIncludedLogoLoadedSuccessfullyProperty =
            BindableProperty.Create(nameof(ManuallyIncludedLogoLoadedSuccessfully), typeof(bool), typeof(MainPage), default(bool));

        public bool AutoIncludedLogoLoadedSuccessfully
        {
            get => (bool)GetValue(AutoIncludedLogoLoadedSuccessfullyProperty);
            set => SetValue(AutoIncludedLogoLoadedSuccessfullyProperty, value);
        }

        public bool ManuallyIncludedLogoLoadedSuccessfully
        {
            get => (bool)GetValue(ManuallyIncludedLogoLoadedSuccessfullyProperty);
            set => SetValue(ManuallyIncludedLogoLoadedSuccessfullyProperty, value);
        }

        public MainPage()
        {
            InitializeComponent();
        }

        private void AutoIncludedLogo_OnError(object sender, CachedImageEvents.ErrorEventArgs e)
        {
            AutoIncludedLogoLoadedSuccessfully = false;
        }

        private void AutoIncludedLogo_OnSuccess(object sender, CachedImageEvents.SuccessEventArgs e)
        {
            AutoIncludedLogoLoadedSuccessfully = true;
        }

        private void ManuallyIncludedLogo_OnError(object sender, CachedImageEvents.ErrorEventArgs e)
        {
            ManuallyIncludedLogoLoadedSuccessfully = false;
        }

        private void ManuallyIncludedLogo_OnSuccess(object sender, CachedImageEvents.SuccessEventArgs e)
        {
            ManuallyIncludedLogoLoadedSuccessfully = true;
        }
    }
}
