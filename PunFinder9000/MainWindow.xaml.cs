using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.Threading.Tasks;

namespace PunFinder9000
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Model = new MainWindowModel(this);
            _updateProgressHandler = delegate(object s, ProgressEventArgs ea)
            {
                SetStatusMessage(ea.Info, ea.IsGreek);
            };
            _setResultsHandler = delegate(object s, ListResultEventArgs ea)
            {
                SetResults(ea.Results);
            };
            _showDocHandler = delegate(object s, DocResultsEventArgs ea)
            {
                ShowDoc(ea.Data, ea.OriginalText);
            };
        }

        public MainWindowModel Model
        {
            get { return (MainWindowModel) DataContext; }
            set { DataContext = value; }
        }

        private EventHandler<ProgressEventArgs> _updateProgressHandler;
        private EventHandler<ListResultEventArgs> _setResultsHandler;
        private EventHandler<DocResultsEventArgs> _showDocHandler;

        private CancellationTokenSource _tokenSource = null;
        private CancellationToken _token;
        private bool _isProcessing = false;

        public void SetStatusMessage(string message, bool isGreek)
        {
            if (isGreek)
                Application.Current.Dispatcher.Invoke(() => { uxStatusMessage2.Text = message; });
            else
                Application.Current.Dispatcher.Invoke(() => { uxStatusMessage1.Text = message; });
        }

        private void SetResults(List<ListResult> results)
        {
            Application.Current.Dispatcher.Invoke(() => { uxResultGrid.ItemsSource = results; });
        }

        private void ShowDoc(List<Tuple<int, int, string, string, string>> data, string originalText)
        {
            ResultText newDocView = null;
            Application.Current.Dispatcher.Invoke(() => { newDocView = new ResultText(); });
            Application.Current.Dispatcher.Invoke(() => { newDocView.GenerateResultDoc(data, originalText); });
            Application.Current.Dispatcher.Invoke(() => { newDocView.Show(); });
        }

        private void OriginalFileBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text Files (*.txt)|*.txt";
            dialog.Multiselect = false;

            var result = dialog.ShowDialog();

            if (!result.HasValue || !result.Value)
                return;

            uxOriginalText.Text = dialog.FileName;
        }

        private void ReferenceTextsBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text Files (*.txt)|*.txt";
            dialog.Multiselect = true;

            var result = dialog.ShowDialog();

            if (!result.HasValue || !result.Value)
                return;

            foreach (var item in dialog.FileNames)
                uxReferenceTexts.Items.Add(item);
        }

        private void uxReferenceTexts_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete && e.Key != Key.Back)
                return;

            if (!(sender is ListBox) || ((ListBox)sender).SelectedItems == null || ((ListBox)sender).SelectedItems.Count <= 0)
                return;

            var toDelete = new List<object>();

            foreach (var item in ((ListBox)sender).SelectedItems)
                toDelete.Add(item);

            foreach (var item in toDelete)
                uxReferenceTexts.Items.Remove(item);
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            if (_isProcessing)
            {
                _tokenSource.Cancel();
            }
            else
            {
                var oText = uxOriginalText.Text;
                var rTexts = uxReferenceTexts.Items.Cast<string>().ToList();
                var length = uxMinWordLengthTextBox.Text;
                var suflength = uxSuffixTextBox.Text;
                int minLength = 4;
                if (!Int32.TryParse(length, out minLength) || minLength < 3)
                    minLength = 4;
                int suffixLength = 0;
                if (!Int32.TryParse(suflength, out suffixLength) || suffixLength < 0 || suffixLength > 2)
                    suffixLength = 0;
                var greekRef = greekRadio == null || greekRadio.IsChecked.Value;
                var isReverse = reverseOriginalText == null || reverseOriginalText.IsChecked.Value;
                var modelCopy = Model;
                modelCopy.ProgressUpdate -= _updateProgressHandler;
                modelCopy.ProgressUpdate += _updateProgressHandler;
                modelCopy.ReturnResultList -= _setResultsHandler;
                modelCopy.ReturnResultList += _setResultsHandler;
                modelCopy.PublishResultsForDoc -= _showDocHandler;
                modelCopy.PublishResultsForDoc += _showDocHandler;
                _tokenSource = new CancellationTokenSource();
                _token = _tokenSource.Token;
                _isProcessing = true;
                uxStartButton.Content = "Cancel";
                await Task.Run(() => modelCopy.StartProcessing(oText, rTexts, minLength, suffixLength, greekRef, isReverse, _token), _token);
                uxStartButton.Content = "Start";
                _isProcessing = false;
            }
        }

        private void Greek_Checked(object sender, RoutedEventArgs e)
        {
            if (uxRefTextLabel == null)
                return;
            uxRefTextLabel.Text = "Greek Reference Texts";
        }

        private void Latin_Checked(object sender, RoutedEventArgs e)
        {
            if (uxRefTextLabel == null)
                return;
            uxRefTextLabel.Text = "Latin Reference Texts";
        }
    }
}
