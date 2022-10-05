using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using HTTP.sys.ViewModels;
namespace HTTP.sys
{
    public partial class HttpSysPage : ContentPage
    {
        public HttpSysViewModel ViewModel { get; set; }
        public HttpSysPage()
        {
            InitializeComponent();
            IconImageSource = "httpsys.png";
            ViewModel = new HttpSysViewModel();
            this.BindingContext = ViewModel;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                await App.DisposeManager.DisposeAsync();
            }
            catch
            {

            }
            finally
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }

        }
    }
}
