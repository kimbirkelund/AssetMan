using System;
using NUnit.Framework;
using Xamarin.UITest;

namespace AssetMan.UITest
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public abstract class TestFixtureBase
    {
        protected IApp App => AppManager.App;
        protected bool OnAndroid => AppManager.Platform == Platform.Android;
        protected bool OnIos => AppManager.Platform == Platform.iOS;

        protected TestFixtureBase(Platform platform)
        {
            if (Environment.GetEnvironmentVariable("ANDROID_HOME") is null)
                Environment.SetEnvironmentVariable("ANDROID_HOME", @"C:\Program Files (x86)\Android\android-sdk\");

            AppManager.Platform = platform;
        }

        [SetUp]
        public virtual void BeforeEachTest()
        {
            AppManager.StartApp();
        }
    }
}
