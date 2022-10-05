using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTTP.sys.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HTTP.sys
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebViewPage : ContentPage
    {
        public WebViewModel ViewModel { get; set; }
        public WebViewPage()
        {
            InitializeComponent();
            IconImageSource = "webview.png";
            ViewModel = new WebViewModel();
            this.BindingContext = ViewModel;
        }
    }
}