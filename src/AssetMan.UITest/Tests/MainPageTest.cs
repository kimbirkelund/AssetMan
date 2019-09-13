using System.Linq;
using AssetMan.UITest.Pages;
using NUnit.Framework;
using Xamarin.UITest;

namespace AssetMan.UITest.Tests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class MainPageTest : TestFixtureBase
    {
        public MainPageTest(Platform platform)
            : base(platform) { }

        [Test]
        public void AutoIncludedLogoShouldShowTheActualLogo()
        {
            var mainTestPage = new MainTestPage();

            App.Screenshot("Main page.");

            var results = mainTestPage.GetAutoIncludedLogoLoadedSuccessfully();
            Assert.IsTrue(results.Any(r => bool.TryParse(r.Text, out var result) && result));
        }

        [Test]
        public void ManuallyIncludedLogoShouldShowTheActualLogo()
        {
            var mainTestPage = new MainTestPage();

            App.Screenshot("Main page.");

            var results = mainTestPage.GetManuallyIncludedLogoLoadedSuccessfully();
            Assert.IsTrue(results.Any(r => bool.TryParse(r.Text, out var result) && result));
        }
    }
}
