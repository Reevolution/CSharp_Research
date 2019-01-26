using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.Win32;
using System.ComponentModel;
using System.Net;
using TestUrl;
using System.IO;
using System.Threading;
using System.Windows.Threading;

namespace TestUrlUi
{
    public enum MainWindowState
    {
        Init,
        Idle,
        Work  
    }

    public class MainWindowViewModel : IDisposable, INotifyPropertyChanged
    {
        public ObservableCollection<BaseUrlInfo> UrlData { get; set; }

        public BaseCommand AnalyzeUrlCommand { get; set; }
        public BaseCommand OpenFileCommand { get; set; }
        public BaseCommand CloseAppCommand { get; set; }

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(FilePath)));
                }
            }
        }

        private int _urlCount;
        public int UrlCount
        {
            get { return _urlCount; }
            set
            {
                if (_urlCount != value)
                {
                    _urlCount = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(UrlCount)));
                }
            }
        }

        private int _currentUrlState;
        public int CurrentUrlState
        {
            get { return _currentUrlState; }
            set
            {
                if (_currentUrlState != value)
                {
                    _currentUrlState = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(CurrentUrlState)));
                }
            }
        }

        private MainWindowState _state = MainWindowState.Init;
        public MainWindowState State
        {
            get { return _state; }
            private set
            {
                if (_state != value)
                {
                    _state = value;

                    AnalyzeUrlCommand.IsEnabledState = _state == MainWindowState.Idle;
                    CloseAppCommand.IsEnabledState = _state == MainWindowState.Idle;
                }
            }
        }

        private LinkExtractor _linkExtractor;
        private CancellationTokenSource _cancellationTokenSource;

        public MainWindowViewModel()
        {
            _linkExtractor = new LinkExtractor();
            _cancellationTokenSource = new CancellationTokenSource();

            UrlData = new ObservableCollection<BaseUrlInfo>();

            AnalyzeUrlCommand = new BaseCommand(AnalyzeUrl);
            OpenFileCommand = new BaseCommand(OpenFile);
            CloseAppCommand = new BaseCommand(CloseApp);
            State = MainWindowState.Idle;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        private async void AnalyzeUrl(object parameter)
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                return;
            }

            State = MainWindowState.Work;
            UrlData.Clear();

            try
            {
                UrlCount = File.ReadAllLines(_filePath).Count();
                var urls = File.ReadAllLines(_filePath).ToList();

                await Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < urls.Count; i++)
                    {
                        try
                        {
                            BaseUrlInfo baseUrlInfo;
                            if (_linkExtractor.TryGetUrl(out baseUrlInfo, urls[i], 1000, _cancellationTokenSource.Token))
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    UrlData.Add(baseUrlInfo);
                                    CurrentUrlState = i + 1;
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            var exc = ex;
                        }
                    }
                }, _cancellationTokenSource.Token);
            }
            catch 
            {
            }

            State = MainWindowState.Idle;
        }

        private void OpenFile(object parameter)
        {
            State = MainWindowState.Work;
            var openFileDlg = new OpenFileDialog();
            if (openFileDlg.ShowDialog() == true)
            {
                FilePath = openFileDlg.FileName;
            }
            State = MainWindowState.Idle;
        }

        private void CloseApp(object parameter)
        {
            Application.Current.MainWindow.Close();
        }

        public void Dispose()
        {         
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _linkExtractor?.Dispose();
        }
    }
}
