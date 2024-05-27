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
using System.Windows.Shapes;
using System.Diagnostics;

namespace PunFinder9000
{
    /// <summary>
    /// Interaction logic for ResultText.xaml
    /// </summary>
    public partial class ResultText : Window
    {
        //private const int WindowSize = 30;

        public ResultText()
        {
            InitializeComponent();
        }


        public void GenerateResultDoc(List<Tuple<int, int, string, string, string>> data, string originalText)
        {
            var par = new Paragraph();
            var itr = 0;
            var line = string.Empty;
            var firstIndex = 0;
            var foundIndexes = new List<FoundIndex>();
            int windowSize = Math.Max(Math.Min(data.Count / 700, 100), 30);

            //var foundStop = new Stopwatch();
            //var foundStop2 = new Stopwatch();
            //var foundStop3 = new Stopwatch();
            //var foundStop4 = new Stopwatch();
            //var foundStop5 = new Stopwatch();
            //var foundStop6 = new Stopwatch();
            //var foundStop7 = new Stopwatch();
            //var foundStop8 = new Stopwatch();
            //var foundStop9 = new Stopwatch();

            var greekWords = new List<string>();
            var greekWordsToAdd = string.Empty;
            TextPointer endPos;
            TextPointer startPos;
            TextPointer startOffset;
            TextPointer endOffset;
            TextRange foundRange;
            IEnumerable<Tuple<int, int, string, string, string>> matches;
            data = data.OrderBy(x => x.Item1).ToList();
            List<Tuple<int, int, string, string, string>> dataSubset = data.Take(windowSize).ToList();
            var localGreedWords = new List<string>();
            string localGreedToAdd = string.Empty;

            //foundStop4.Start();
            for( var curIndex = 0; curIndex <= originalText.Length; curIndex++)
            {               
                if (curIndex == originalText.Length || originalText[curIndex] == '\r' || originalText[curIndex] == '\n')
                {
                    //foundStop6.Start();
                    par.Inlines.Add(new Run(line));
                    //foundStop6.Stop();
                    greekWords = new List<string>();
                    greekWordsToAdd = string.Empty;
                    foreach (var foundIndex in foundIndexes.GroupBy(x => new { x.OriginalStart, x.OriginalEnd }))
                    {
                        localGreedWords = new List<string>();
                        foreach (var fIndex in foundIndex)
                        {
                            //foundStop.Start();
                            endPos = par.ContentEnd;
                            startPos = endPos.GetPositionAtOffset(line.Length * -1);

                            startOffset = startPos.GetPositionAtOffset(fIndex.StartIndex == null ? -1 : fIndex.StartIndex.Value - 1);
                            endOffset = fIndex.EndIndex == null ? endPos : startPos.GetPositionAtOffset(fIndex.EndIndex.Value);
                            foundRange = new TextRange(startOffset, endOffset);
                            foundRange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(new Color { A = 120, R = 255, G = 255, B = 0 }));
                            if (!string.IsNullOrWhiteSpace(fIndex.GreekWord))
                            {                             
                                localGreedWords.Add(fIndex.GreekWord);
                            }
                            //greekWords.Add(fIndex.GreekWord);
                        }
                        if (localGreedWords.Count > 1)
                            greekWords.Add(string.Format("[{0}]", string.Join(", ", localGreedWords)));
                        else if (localGreedWords.Count == 1)
                            greekWords.Add(string.Join(", ", localGreedWords));
                        //foundStop.Stop();
                    }

                    if (greekWords.Count > 0)
                        greekWordsToAdd = "     (" + string.Join(", ", greekWords) + ")";

                    //foundStop5.Start();
                    par.Inlines.Add(new Run(greekWordsToAdd + "\r\n"));
                    //foundStop5.Stop();
                    firstIndex = curIndex + 1;
                    line = string.Empty;
                    itr = 0;
                    foundIndexes = new List<FoundIndex>();
                    dataSubset = data.Take(windowSize).ToList(); 
                    continue;
                }

                //foundStop9.Start();               
                //foundStop7.Start();
                matches = dataSubset.Where(x => x.Item1 == curIndex);
                //foundStop7.Stop();
                foreach (var match in matches)
                {
                    //foundStop2.Start();
                    foundIndexes.Add(new FoundIndex { StartIndex = itr, OriginalStart = match.Item1, OriginalEnd = match.Item2, GreekWord = match.Item4});
                    //foundStop2.Stop();
                }
                //foundStop8.Start();
                matches = dataSubset.Where(x => x.Item2 == curIndex);
                foreach(var match in matches)
                {
                    data.Remove(match);
                }
                //foundStop8.Stop();
                foreach (var match in matches)
                {
                    //foundStop3.Start();
                    var inLines = foundIndexes.Where(x => x.OriginalStart == match.Item1 && x.OriginalEnd == match.Item2);
                    foreach (var inLine in inLines)
                    {
                        inLine.EndIndex = itr;
                    }
                    if (inLines == null || inLines.Count() < 1)
                    {
                        foundIndexes.Add(new FoundIndex { EndIndex = itr, OriginalStart = match.Item1, OriginalEnd = match.Item2 });
                    }
                    //foundStop3.Stop();
                }

                line += originalText[curIndex];
                itr++;
                //foundStop9.Stop();
            }
            //foundStop4.Stop();
            uxDoc.Blocks.Add(par);
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            //var dialog = new PrintDialog();
            ////var docToPrint = uxDoc.;
            //IDocumentPaginatorSource source = docToPrint;
            
            //dialog.PrintDocument(source.DocumentPaginator, "Results");
            //var result = dialog.ShowDialog();
            //if (result.HasValue && result.Value)
            //{
            //    dialog.PrintDocument(source.DocumentPaginator, "Results");
            //}
            Print(uxDoc);

        }

        private void Print(FlowDocument document)
        {
            System.IO.MemoryStream s = new System.IO.MemoryStream();
            TextRange source = new TextRange(document.ContentStart, document.ContentEnd);
            source.Save(s, DataFormats.Xaml);
            FlowDocument copy = new FlowDocument();
            TextRange dest = new TextRange(copy.ContentStart, copy.ContentEnd);
            dest.Load(s, DataFormats.Xaml);

            var dialog = new PrintDialog();

            copy.ColumnWidth = dialog.PrintableAreaWidth;
            copy.PagePadding = new Thickness(50);
            IDocumentPaginatorSource sourceCopy = copy;
            
            //dialog.PrintDocument(sourceCopy.DocumentPaginator, "Results");
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                dialog.PrintDocument(sourceCopy.DocumentPaginator, "Results");
            }
        }


    }
}
