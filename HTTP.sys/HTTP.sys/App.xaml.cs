using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HTTP.sys.Services;
namespace HTTP.sys
{
    public partial class App : Application
    {
        public static readonly IDisposeManager DisposeManager;
        static App()
        {
            DisposeManager = new DisposeManager();
        }
        public App()
        {
            InitializeComponent();
            MainPage = new MenuPage();
        }

        protected override void OnStart()
        {
        }

        protected override async void OnSleep()
        {

        }

        protected override void OnResume()
        {
        }
    }
}
