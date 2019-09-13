using System;
using NUnit.Framework;
using Xamarin.UITest;

namespace AssetMan.UITest
{
    public abstract class TestPageBase
    {
        protected abstract PlatformQuery Trait { get; }
        protected IApp App => AppManager.App;
        protected bool OnAndroid => AppManager.Platform == Platform.Android;
        protected bool OnIos => AppManager.Platform == Platform.iOS;

        protected TestPageBase()
        {
            AssertOnPage(TimeSpan.FromSeconds(30));
            App.Screenshot("On " + GetType()
                               .Name);
        }

        /// <summary>
        /// Verifies that the trait is still present. Defaults to no wait.
        /// </summary>
        /// <param name="timeout">Time to wait before the assertion fails</param>
        protected void AssertOnPage(TimeSpan? timeout = default)
        {
            var message = "Unable to verify on page: " + GetType()
                              .Name;

            if (timeout == null)
                Assert.IsNotEmpty(App.Query(Trait.Current), message);
            else
                Assert.DoesNotThrow(() => App.WaitForElement(Trait.Current, timeout: timeout), message);
        }

        /// <summary>
        /// Verifies that the trait is no longer present. Defaults to a 5 second wait.
        /// </summary>
        /// <param name="timeout">Time to wait before the assertion fails</param>
        protected void WaitForPageToLeave(TimeSpan? timeout = default)
        {
            timeout = timeout ?? TimeSpan.FromSeconds(5);
            var message = "Unable to verify *not* on page: " + GetType()
                              .Name;

            Assert.DoesNotThrow(() => App.WaitForNoElement(Trait.Current, timeout: timeout), message);
        }

        // You can edit this file to define functionality that is common across many or all pages in your app.
        // For example, you could add a method here to open a side menu that is accesible from all pages.
        // To keep things more organized, consider subclassing BasePage and including common page actions there.
        // For some examples check out https://github.com/xamarin-automation-service/uitest-pop-example/wiki
    }
}
