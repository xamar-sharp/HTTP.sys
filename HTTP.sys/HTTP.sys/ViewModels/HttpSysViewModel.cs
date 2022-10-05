using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Xamarin.Essentials;
using System.Net;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System.Collections.ObjectModel;
namespace HTTP.sys.ViewModels
{
    public sealed class HttpSysViewModel : NotifyViewModel, IDisposable
    {
        public sealed class FileViewModel : NotifyViewModel
        {
            private string _fileName;
            public string FileName { get => _fileName; set { _fileName = value; OnPropertyChanged(); } }
            public byte[] Data { get; set; }
            public long Length { get => Data.Length / 1024; set { } }
            public override void ChangeCanExecute()
            {
                throw new NotImplementedException();
            }
            public FileViewModel()
            {
                FileName = "";
                Data = new byte[1];
            }
            public FileViewModel(string fileName, byte[] data)
            {
                FileName = fileName;
                Data = data;
            }
        }
        private readonly HttpListener _listener;
        private short _statusCode;
        private FileViewModel _selectedFile;
        public ObservableCollection<string> Prefixes { get; set; }
        public short StatusCode { get { return _statusCode; } set { _statusCode = value; OnPropertyChanged(); } }
        public ObservableCollection<string> Headers { get; set; }
        public FileViewModel SelectedFile { get { return _selectedFile; } set { _selectedFile = value; OnPropertyChanged(); } }
        public Command SetPrefixes { get; set; }
        public Command SetStatusCode { get; set; }
        public Command AddHeader { get; set; }
        public Command SelectFile { get; set; }
        public Command StartListener { get; set; }
        public Command StopListener { get; set; }
        public async Task<FileViewModel> SelectFileAction()
        {
            FileResult result = await FilePicker.PickAsync();
            byte[] buffer;
            using (var stream = await result.OpenReadAsync())
            {
                buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, buffer.Length);
            }
            return new FileViewModel(result.FileName, buffer);
        }
        public void Dispose()
        {
            _listener.Close();
        }
        public override void ChangeCanExecute()
        {
            StartListener.ChangeCanExecute();
            StopListener.ChangeCanExecute();
        }
        public HttpSysViewModel()
        {
            _listener = new HttpListener();
            App.DisposeManager.Register(this);
            StatusCode = 200;
            Prefixes = new ObservableCollection<string>(new List<string>(1));
            Headers = new ObservableCollection<string>(new List<string>(1));
            SelectedFile = new FileViewModel();
            SetPrefixes = new Command((obj) =>
            {
                if (obj is Entry)
                {
                    Prefixes.Add((obj as Entry).Text);
                }
                else
                {
                    Prefixes.Remove((obj as TextCell).Text);
                }
            });
            SetStatusCode = new Command((obj) =>
            {
                StatusCode = Convert.ToInt16((obj as Entry).Text);
            });
            AddHeader = new Command((obj) =>
            {
                if (obj is Entry)
                {
                    Headers.Add((obj as Entry).Text);
                }
                else
                {
                    Headers.Remove((obj as TextCell).Text);
                }
            });
            SelectFile = new Command(async (obj) =>
            {
                var res = await SelectFileAction();
                SelectedFile.FileName = res.FileName;
                SelectedFile.Data = res.Data;
                (obj as Label).Text = res.FileName;
                ChangeCanExecute();
            });
            StartListener = new Command(async (obj) =>
            {
                if (_listener.Prefixes.Count == 0)
                {
                    foreach (var prefix in Prefixes)
                    {
                        _listener.Prefixes.Add(prefix);
                    }
                }
                _listener.Start();
                ChangeCanExecute();
                (obj as ActivityIndicator).IsVisible = true;
                (obj as ActivityIndicator).IsRunning = true;
                var ctx = await _listener.GetContextAsync();
                ctx.Response.StatusCode = StatusCode;
                ctx.Response.ContentType = "application/octate-stream";
                ctx.Response.ContentLength64 = SelectedFile.Data.Length;
                await ctx.Response.OutputStream.WriteAsync(SelectedFile.Data);
                foreach (var header in Headers)
                {
                    string[] values = header.Split('=');
                    if (values.Length == 2)
                    {
                        ctx.Response.Headers.Add(values[0], values[1]);
                    }
                }
                (obj as ActivityIndicator).IsVisible = false;
                (obj as ActivityIndicator).IsRunning = false;

            }, (obj) => SelectedFile.Data != null);
            StopListener = new Command((obj) =>
            {
                _listener.Stop();
                (obj as ActivityIndicator).IsVisible = false;
                (obj as ActivityIndicator).IsRunning = false;
                ChangeCanExecute();
            }, (obj) => _listener.IsListening);
        }
    }
}
