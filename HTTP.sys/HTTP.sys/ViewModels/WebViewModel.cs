using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
namespace HTTP.sys.ViewModels
{
    public sealed class WebViewModel : NotifyViewModel
    {
        private string _raw;
        private bool _isHtml;
        public string Raw { get => _raw; set {
                _raw = value;
                OnPropertyChanged();
            } }
        public bool IsHtml { get => _isHtml; set {
                _isHtml = value;
                OnPropertyChanged();
            } }
        public Command SendRequest { get; set; }
        public WebViewModel()
        {
            SendRequest = new Command((obj) =>
            {
                if (IsHtml)
                {
                    (obj as WebView).Source = new HtmlWebViewSource() { Html = Raw };
                }
                else
                {
                    (obj as WebView).Source = new UrlWebViewSource() { Url = Raw };
                }
            }, (obj) => !string.IsNullOrWhiteSpace(Raw));
        }
        public override void ChangeCanExecute()
        {
            SendRequest.ChangeCanExecute();
        }
    }
}
