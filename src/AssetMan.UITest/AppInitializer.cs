using System;
using Xamarin.UITest;

namespace AssetMan.UITest
{
    public static class AppManager
    {
        private const string AndroidAppId = "com.companyname.assetman.sample";
        private const string ApkPath = "../../../AssetMan.Sample/AssetMan.Sample.Android/bin/Debug/com.companyname.assetman.sample.apk";
        private const string AppPath = "../../../AssetMan.Sample/AssetMan.Sample.iOS/bin/Debug/AssetMan.Sample.iOS.app";
        private const string IosAppId = "com.companyname.AssetMan.Sample";

        private static IApp _app;
        private static AppLaunchMode? _appLaunchMode;
        private static Platform? _platform;

        public static IApp App
        {
            get
            {
                if (_app == null)
                    throw new NullReferenceException("'AppManager.App' not set. Call 'AppManager.StartApp()' before trying to access it.");
                return _app;
            }
        }

        public static AppLaunchMode AppLaunchMode
        {
            get
            {
                if (_appLaunchMode is AppLaunchMode appLaunchMode)
                    return appLaunchMode;

                if (Enum.TryParse(Environment.GetEnvironmentVariable("UITEST_APP_LAUNCH_MODE"), out appLaunchMode))
                    return appLaunchMode;

                return AppLaunchMode.UseInstalledApp;
            }

            set => _appLaunchMode = value;
        }

        public static Platform Platform
        {
            get
            {
                if (_platform == null)
                    throw new NullReferenceException("'AppManager.Platform' not set.");
                return _platform.Value;
            }

            set => _platform = value;
        }

        public static void StartApp()
        {
            if (Platform == Platform.Android)
            {
                var androidAppConfigurator = ConfigureApp.Android;

                switch (AppLaunchMode)
                {
                    case AppLaunchMode.InstallAppFirst:
                        androidAppConfigurator = androidAppConfigurator.ApkFile(ApkPath);
                        break;
                    case AppLaunchMode.UseInstalledApp:
                        androidAppConfigurator = androidAppConfigurator.InstalledApp(AndroidAppId);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _app = androidAppConfigurator.StartApp();
            }

            if (Platform == Platform.iOS)
            {
                var iosAppConfigurator = ConfigureApp.iOS;

                switch (AppLaunchMode)
                {
                    case AppLaunchMode.InstallAppFirst:
                        iosAppConfigurator = iosAppConfigurator.AppBundle(AppPath);
                        break;
                    case AppLaunchMode.UseInstalledApp:
                        iosAppConfigurator = iosAppConfigurator.InstalledApp(IosAppId);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _app = iosAppConfigurator.StartApp();
            }
        }
    }
}
