/*
          __ _/| _/. _  ._/__ /
        _\/_// /_///_// / /_|/
                   _/
        copyright (c) sof digital 2021
        written by michael rinderle <michael@sofdigital.net>
*/

using Headtrip.Data;
using Headtrip.Providers;
using Xamarin.Forms;

namespace Headtrip
{
    public partial class App : Application
    {

        public App()
        {
            DependencyProvider.Init();

            InitializeComponent();
            InitializeDatabase();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Constants.SyncfusionLicense);

            MainPage = new AppShell();
        }

        private void InitializeDatabase()
        {
            using (var ctx = new SqliteContext())
            {
                ctx.Database.EnsureCreated();
            }

        }

        protected override void OnStart() { }

        protected override void OnSleep() { }

        protected override void OnResume() { }
    }
}