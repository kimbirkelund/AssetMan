using Xamarin.UITest.Queries;

namespace AssetMan.UITest.Pages
{
    public class MainTestPage : TestPageBase
    {
        protected override PlatformQuery Trait { get; } = new PlatformQuery
                                                          {
                                                              Android = x => x.Marked("MainPage"),
                                                              iOS = x => x.Marked("MainPage")
                                                          };

        public AppResult[] GetAutoIncludedLogoLoadedSuccessfully()
        {
            return App.Query(x => x.Marked("AutoIncludedLogoLoadedSuccessfully"));
        }

        public AppResult[] GetManuallyIncludedLogoLoadedSuccessfully()
        {
            return App.Query(x => x.Marked("ManuallyIncludedLogoLoadedSuccessfully"));
        }
    }
}
