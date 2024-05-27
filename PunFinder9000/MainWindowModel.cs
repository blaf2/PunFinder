using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Threading;
using System.Threading;
using System.Text.RegularExpressions;

namespace PunFinder9000
{
    public class MainWindowModel
    {
        private readonly MainWindow _view;
        private readonly Dictionary<string, List<char>> _greekDictionary = new Dictionary<string, List<char>>();
        private Dictionary<string, List<string>> _dipthongDictionary = new Dictionary<string, List<string>>();
        private HashSet<string> _latinDipthongs = new HashSet<string>();
        private readonly Dictionary<char, List<string>> _greekLatinDictionary = new Dictionary<char, List<string>>();

        //private const int MinGreekLetters = 4;

        public MainWindowModel(MainWindow view)
        {
            _view = view;
            _greekDictionary.Add("α", new List<char> { 'α', 'Α', 'ἀ', 'ἁ', 'ἂ', 'ἃ', 'ἄ', 'ἅ', 'ἆ', 'ἇ', 'Ἀ', 'Ἁ', 'Ἂ', 'Ἃ', 'Ἄ', 'Ἅ', 'Ἆ', 'Ἇ', 'ὰ', 'ᾀ', 'ᾁ', 'ᾂ', 'ᾃ', 'ᾄ', 'ᾅ', 'ᾆ', 'ᾇ', 'ᾈ', 'ᾉ', 'ᾊ', 'ᾋ', 'ᾌ', 'ᾍ', 'ᾎ', 'ᾏ', 'ᾰ', 'ᾱ', 'ᾲ', 'ᾳ', 'ᾴ', 'ᾶ', 'ᾷ', 'Ᾰ', 'Ᾱ', 'Ὰ', 'Ά', 'ᾼ', 'ά', 'ά' });
            _greekDictionary.Add("β", new List<char> { 'Β', 'β' });
            _greekDictionary.Add("κ", new List<char> { 'κ', 'Κ' });
            _greekDictionary.Add("χ", new List<char> { 'χ', 'Χ' });
            _greekDictionary.Add("δ", new List<char> { 'δ', 'Δ' });
            _greekDictionary.Add("ε", new List<char> { 'ε', 'Ε', 'ἐ', 'ἑ', 'ἒ', 'ἓ', 'ἔ', 'ἕ', 'Ἐ', 'Ἑ', 'Ἒ', 'Ἓ', 'Ἔ', 'Ἕ', 'ὲ', 'έ', 'Ὲ', 'Έ', 'έ' });
            _greekDictionary.Add("η", new List<char> { 'η', 'Η', 'ἠ', 'ἡ', 'ἢ', 'ἣ', 'ἤ', 'ἥ', 'ἦ', 'ἧ', 'Ἠ', 'Ἡ', 'Ἢ', 'Ἣ', 'Ἤ', 'Ἥ', 'Ἦ', 'Ἧ', 'ὴ', 'ή', 'ᾐ', 'ᾑ', 'ᾒ', 'ᾓ', 'ᾔ', 'ᾕ', 'ᾖ', 'ᾗ', 'ῂ', 'ῃ', 'ῄ', 'ῆ', 'ῇ', 'ᾘ', 'ᾙ', 'ᾚ', 'ᾛ', 'ᾜ', 'ᾝ', 'ᾞ', 'ᾟ', 'Ή', 'ῌ', 'ή' });
            _greekDictionary.Add("φ", new List<char> { 'φ', 'Φ' });
            _greekDictionary.Add("γ", new List<char> { 'γ', 'Γ' });
            _greekDictionary.Add("ι", new List<char> { 'ι', 'Ι', 'ἰ', 'ἱ', 'ἲ', 'ἳ', 'ἴ', 'ἵ', 'ἶ', 'ἷ', 'Ἰ', 'Ἱ', 'Ἲ', 'Ἳ', 'Ἴ', 'Ἵ', 'Ἶ', 'Ἷ', 'ὶ', 'ί', 'ῐ', 'ῑ', 'ῒ', 'ΐ', 'ῖ', 'ῗ', 'Ῐ', 'Ῑ', 'Ὶ', 'Ί', 'ί', 'ΐ', 'ϊ' });
            _greekDictionary.Add("λ", new List<char> { 'λ', 'Λ' });
            _greekDictionary.Add("μ", new List<char> { 'μ', 'Μ' });
            _greekDictionary.Add("ν", new List<char> { 'ν', 'Ν' });
            _greekDictionary.Add("ο", new List<char> { 'ο', 'Ο', 'ὀ', 'ὁ', 'ὂ', 'ὃ', 'ὄ', 'ὅ', 'Ὀ', 'Ὁ', 'Ὂ', 'Ὃ', 'Ὄ', 'Ὅ', 'ὸ', 'ό', 'Ὸ', 'Ό', 'ό' });
            _greekDictionary.Add("π", new List<char> { 'π', 'Π' });
            _greekDictionary.Add("ρ", new List<char> { 'ρ', 'Ρ', 'ῤ', 'ῥ', 'Ῥ' });
            _greekDictionary.Add("ζ", new List<char> { 'ζ', 'Ζ' });
            _greekDictionary.Add("ξ", new List<char> { 'ξ', 'Ξ' });
            _greekDictionary.Add("σ", new List<char> { 'ς', 'σ', 'Σ' });
            _greekDictionary.Add("τ", new List<char> { 'τ', 'Τ' });
            _greekDictionary.Add("υ", new List<char> { 'υ', 'Υ', 'ὐ', 'ὑ', 'ὒ', 'ὓ', 'ὔ', 'ὕ', 'ὖ', 'ὗ', 'Ὑ', 'Ὓ', 'Ὕ', 'Ὗ', 'ὺ', 'ύ', 'ῠ', 'ῡ', 'ῢ', 'ΰ', 'ῦ', 'ῧ', 'Ῠ', 'Ῡ', 'Ὺ', 'Ύ', 'ύ', 'ύ', 'ΰ', 'ϋ' });
            _greekDictionary.Add("ψ", new List<char> { 'ψ', 'Ψ'});
            _greekDictionary.Add("ω", new List<char> { 'ω', 'Ω', 'ὠ', 'ὡ', 'ὢ', 'ὣ', 'ὤ', 'ὥ', 'ὦ', 'ὧ', 'Ὠ', 'Ὡ', 'Ὢ', 'Ὣ', 'Ὤ', 'Ὥ', 'Ὦ', 'Ὧ', 'ὼ', 'ώ', 'ᾠ', 'ᾡ', 'ᾢ', 'ᾣ', 'ᾤ', 'ᾦ', 'ᾥ', 'ᾧ', 'Ὼ', 'Ώ', 'ῼ', 'ᾨ', 'ᾩ', 'ᾪ', 'ᾫ', 'ᾬ', 'ᾭ', 'ᾮ', 'ᾯ', 'ῷ', 'ῶ', 'ῳ', 'ῴ', 'ώ', 'ῲ' });
            _greekDictionary.Add("θ", new List<char> { 'θ', 'Θ' });
            _greekDictionary.Add(" ", new List<char> { ' ' });
            _greekDictionary.Add("\n", new List<char> { '\n' });
            _greekDictionary.Add("\r", new List<char> { '\r' });
            _greekDictionary.Add("\t", new List<char> { '\t' });

            _dipthongDictionary.Add("αι", new List<string> { "ae", "ai" });
            _dipthongDictionary.Add("ει", new List<string> { "ei" });
            _dipthongDictionary.Add("ευ", new List<string> { "eu" });
            _dipthongDictionary.Add("ηυ", new List<string> { "eu" });
            _dipthongDictionary.Add("οι", new List<string> { "oe", "oi" });
            _dipthongDictionary.Add("αυ", new List<string> { "au" });
            _dipthongDictionary.Add("υι", new List<string> { "ui" });
            _dipthongDictionary.Add("ου", new List<string> { "ou" });

            _latinDipthongs.Add("ae");
            _latinDipthongs.Add("ei");
            _latinDipthongs.Add("eu");
            _latinDipthongs.Add("oe");
            _latinDipthongs.Add("au");
            _latinDipthongs.Add("ui");

            _greekLatinDictionary.Add('α', new List<string> { "a" });
            _greekLatinDictionary.Add('β', new List<string> { "b" });
            _greekLatinDictionary.Add('κ', new List<string> { "c", "k", "qu", "g" });
            _greekLatinDictionary.Add('χ', new List<string> { "c", "ch", "qu" });
            _greekLatinDictionary.Add('δ', new List<string> { "d" });
            _greekLatinDictionary.Add('ε', new List<string> { "e" });
            _greekLatinDictionary.Add('η', new List<string> { "e" });
            _greekLatinDictionary.Add('φ', new List<string> { "f", "ph", "p" });
            _greekLatinDictionary.Add('γ', new List<string> { "g" });
            _greekLatinDictionary.Add('ι', new List<string> { "i" });
            _greekLatinDictionary.Add('λ', new List<string> { "l" });
            _greekLatinDictionary.Add('μ', new List<string> { "m" });
            _greekLatinDictionary.Add('ν', new List<string> { "n" });
            _greekLatinDictionary.Add('ο', new List<string> { "o" });
            _greekLatinDictionary.Add('ω', new List<string> { "o" });
            _greekLatinDictionary.Add('π', new List<string> { "p" });
            _greekLatinDictionary.Add('ψ', new List<string> { "ps", "bs" });
            _greekLatinDictionary.Add('ρ', new List<string> { "r", "rh" });
            _greekLatinDictionary.Add('σ', new List<string> { "s", "ns" });
            _greekLatinDictionary.Add('ς', new List<string> { "s", "ns" });
            _greekLatinDictionary.Add('τ', new List<string> { "t" });
            _greekLatinDictionary.Add('θ', new List<string> { "th", "t" });
            _greekLatinDictionary.Add('υ', new List<string> { "u", "y" });
            _greekLatinDictionary.Add('ξ', new List<string> { "x" });
            _greekLatinDictionary.Add('ζ', new List<string> { "z" });
        }

        public event EventHandler<ProgressEventArgs> ProgressUpdate = null;
        public event EventHandler<ListResultEventArgs> ReturnResultList = null;
        public event EventHandler<DocResultsEventArgs> PublishResultsForDoc = null; 

        public async Task StartProcessing(string originalFilePath, List<string> refFilePaths, int minLength, int suffixLength, bool isGreek, bool isReverse, CancellationToken token)
        {
            string originalText = string.Empty;
            string refText = string.Empty;

            try
            {   
                using (StreamReader sr = new StreamReader(originalFilePath))
                {
                    originalText = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                _view.SetStatusMessage(ex.Message, false);
                return;
            }

            try
            {
                foreach (var file in refFilePaths)
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        refText += sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                _view.SetStatusMessage(ex.Message, false);
                return;
            }

            List<Tuple<string, string>> refWords = null;
            Tuple<string, List<int>> processedOriginalText = null; // entire processed latin text with real index of every letter
            try
            {
                var task1 = isGreek ? Task.Run(() => { refWords = GetGreekWords(refText, minLength, suffixLength, token).Result; }, token) : Task.Run(() => { refWords = GetLatinRefWords(refText, minLength, suffixLength, token).Result; }, token);
                var task2 = Task.Run(() => { processedOriginalText = ProcessOriginalText(originalText, isReverse, token).Result; }, token);           
                await Task.WhenAll(task1, task2);
            }
            catch (AggregateException ae)
            {
                _view.SetStatusMessage(string.Join(", ", ae.InnerExceptions.Select(x => x.Message)), false);
            }
            if (token.IsCancellationRequested)
                return;
            RaiseProgressUpdate(new ProgressEventArgs(string.Empty, true));
            RaiseProgressUpdate(new ProgressEventArgs(string.Empty, false));

            List<Tuple<int, int, string, string, string>> matches = null;
            if (isGreek)
                await Task.Run(() => { matches = FindMatches(processedOriginalText, refWords, isReverse, token); });
            else
                await Task.Run(() => { matches = FindLatinMatches(processedOriginalText, refWords, isReverse, token); });

            if (token.IsCancellationRequested)
                return;

            var values = new List<Tuple<string, string>>();
            var toDelete = new List<int>();

            RaiseProgressUpdate(new ProgressEventArgs("Processing matches...", false));

            for (var i = 0; i < matches.Count; i++)
            {
                //var toAdd = new ListResult { LatinText = originalText.Substring(matches[i].Item1, matches[i].Item2 - matches[i].Item1 + 1).Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " "), ReferenceText = matches[i].Item4 };
                var toAdd = new Tuple<string, string>(matches[i].Item5, matches[i].Item4 );
                if (matches[i].Item5.ToLower() != matches[i].Item3 && originalText.Substring(matches[i].Item1, matches[i].Item2 - matches[i].Item1 + 1).Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " ").ToLower() != matches[i].Item3)
                    values.Add(toAdd);
                if (matches[i].Item5.ToLower() == matches[i].Item3 || originalText.Substring(matches[i].Item1, matches[i].Item2 - matches[i].Item1 + 1).Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " ").ToLower() == matches[i].Item3)
                   toDelete.Add(i);
            }

            for (var i = toDelete.Count - 1; i >= 0; i--)
                matches.RemoveAt(toDelete[i]);

            var wMatches = values.GroupBy(x => x.Item1);

            var results = new List<ListResult>();

            foreach(var match in wMatches)
            {
                results.Add(new ListResult { LatinText = match.Key, MatchedWords = string.Join(", ", match.Select(x => x.Item2).Distinct()), Frequency = match.Count() });
            }

            results = results.OrderByDescending(x => x.Frequency).ToList();

            RaiseProgressUpdate(new ProgressEventArgs(string.Empty, false));

            RaiseResultUpdate(new ListResultEventArgs(results));

            RaisePublishResultsForDoc(new DocResultsEventArgs(matches, originalText));
        }

        private string GetSimiplifiedGreekLetter(char letter)
        {
            return _greekDictionary.FirstOrDefault(x => x.Value.Contains(letter)).Key;
        }

        private string GetFullWordForShortenedWord(string word, HashSet<int> omittedLetters)
        {
            var result = string.Empty;
            var isOmitted = false;

            for (var i = 0; i < word.Length; i++)
            {
                if (!isOmitted && omittedLetters.Contains(i))
                {
                    result += string.Format("({0}", word[i]);
                    isOmitted = true;
                }
                else if (isOmitted && !omittedLetters.Contains(i))
                {
                    result += string.Format("){0}", word[i]);
                    isOmitted = false;
                }
                else
                {
                    isOmitted = omittedLetters.Contains(i);
                    result += word[i];
                }
            }
            if (isOmitted)
                result += ")";

            return result;
        }

        private async Task<List<Tuple<string, string>>> GetGreekWords(string refText, int minLength, int suffixLength, CancellationToken token)
        {
            var greekWords = new List<string>();
            var word = string.Empty;

            if (string.IsNullOrWhiteSpace(refText))
                return new List<Tuple<string, string>>();

            for (var curIndex = 0; curIndex < refText.Length; curIndex++)
            {
                if (token.IsCancellationRequested)
                {
                    RaiseProgressUpdate(new ProgressEventArgs(string.Empty, true));
                    break;
                }
                if (curIndex % 10 == 0)
                    RaiseProgressUpdate(new ProgressEventArgs(string.Format("Processing Greek texts... {0}%", Math.Round(((double)curIndex / (double)refText.Length) * 100, 1)), true));
                var letter = refText[curIndex];
                var simplifiedLetter = GetSimiplifiedGreekLetter(letter);
                //if (simplifiedLetter == null && "\'\":\\.,;-_=+*’[](){}᾽·‘—ϝ1234567890/?qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM".IndexOf(letter) < 0)
                //{
                //    if (letter != (char)770 && letter != (char)776 && letter != (char)803 && letter != (char)788 && letter != (char)8128 && letter != (char)8142 && letter != (char)787)
                //    {
                //        var lol = refText[curIndex - 1];
                //    }
                //}
                if (simplifiedLetter == " " || simplifiedLetter == "\n" || simplifiedLetter == "\r" || simplifiedLetter == "\t")
                {
                    if (word != string.Empty && !greekWords.Select(x => x).Contains(word) && word.Length >= minLength)
                    {
                        greekWords.Add(word);
                    }
                    word = string.Empty;
                    continue;
                }
                word += simplifiedLetter;
                if (curIndex == refText.Length - 1 && word != string.Empty)
                    greekWords.Add(word);
            }
            var toReturnBefore = new List<Tuple<string, string, HashSet<int>>>();
            foreach (var lWord in greekWords)
            {
                toReturnBefore.AddRange(GetAdditionalWordsWithDoubleLetters(lWord));
            }

            var toAdd = new List<Tuple<string, string, HashSet<int>>>();
            for (var i = 1; i <= suffixLength; i++)
            {
                foreach (var lWord in toReturnBefore)
                {
                    if (lWord.Item1.Length < i + 4)
                        continue;
                    var set = new HashSet<int>(lWord.Item3);
                    for (var a = lWord.Item1.Length - 1; a >= lWord.Item1.Length - i; a--)
                    {
                        set.Add(a);
                    }
                    toAdd.Add(new Tuple<string, string, HashSet<int>>(lWord.Item1.Substring(0, lWord.Item1.Length - i), lWord.Item2, set));
                }
            }
            toReturnBefore.AddRange(toAdd);

            var toReturnBefore2 = new List<Tuple<string, string, HashSet<int>>>();
            foreach (var lWord in toReturnBefore)
            {
                if (toReturnBefore2.Select(x => x.Item1).Contains(lWord.Item1))
                    continue;
                toReturnBefore2.Add(lWord);
            }

            var toReturn = new List<Tuple<string, string>>();

            foreach (var lWord in toReturnBefore2)
            {
                toReturn.Add(new Tuple<string, string>(lWord.Item1, GetFullWordForShortenedWord(lWord.Item2, lWord.Item3)));
            }

            RaiseProgressUpdate(new ProgressEventArgs("Processing Greek texts... 100%", true));
            return toReturn;
        }

        private async Task<List<Tuple<string, string>>> GetLatinRefWords(string refText, int minLength, int suffixLength, CancellationToken token)
        {
            var latinWords = new List<string>();
            var word = string.Empty;

            if (string.IsNullOrWhiteSpace(refText))
                return new List<Tuple<string, string>>();

            for (var curIndex = 0; curIndex < refText.Length; curIndex++)
            {
                if (token.IsCancellationRequested)
                {
                    RaiseProgressUpdate(new ProgressEventArgs(string.Empty, true));
                    break;
                }
                if (curIndex % 10 == 0)
                    RaiseProgressUpdate(new ProgressEventArgs(string.Format("Processing Latin reference texts... {0}%", Math.Round(((double)curIndex / (double)refText.Length) * 100, 1)), true));
                var letter = refText[curIndex];
                if (letter == ' ' || letter == '\n' || letter == '\r' || letter == '\t')
                {
                    if (word != string.Empty && !latinWords.Contains(word) && word.Length >= minLength)
                    {
                        latinWords.Add(word);
                    }
                    word = string.Empty;
                    continue;
                }
                
                var simplifiedLetter = Char.IsLetter(letter) ? Char.ToLower(letter).ToString() : string.Empty;

                //if (simplifiedLetter == null && "\'\":\\.,;-_=+*’[](){}᾽·‘—ϝ1234567890/?qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM".IndexOf(letter) < 0)
                //{
                //    if (letter != (char)770 && letter != (char)776 && letter != (char)803 && letter != (char)788 && letter != (char)8128 && letter != (char)8142 && letter != (char)787)
                //    {
                //        var lol = refText[curIndex - 1];
                //    }
                //}
                
                word += simplifiedLetter;
                if (curIndex == refText.Length - 1 && word != string.Empty)
                    latinWords.Add(word);
            }
            var toReturnBefore = new List<Tuple<string, string, HashSet<int>>>();
            foreach (var lWord in latinWords)
            {
                toReturnBefore.AddRange(GetAdditionalWordsWithDoubleLetters(lWord));
            }

            var toAdd = new List<Tuple<string, string, HashSet<int>>>();
            for (var i = 1; i <= suffixLength; i++)
            {
                foreach (var lWord in toReturnBefore)
                {
                    if (lWord.Item1.Length < i + 4)
                        continue;
                    var set = new HashSet<int>(lWord.Item3);
                    for (var a = lWord.Item1.Length - 1; a >= lWord.Item1.Length - i; a--)
                    {
                        set.Add(a);
                    }
                    toAdd.Add(new Tuple<string, string, HashSet<int>>(lWord.Item1.Substring(0, lWord.Item1.Length - i), lWord.Item2, set));
                }
            }
            toReturnBefore.AddRange(toAdd);

            var toReturnBefore2 = new List<Tuple<string, string, HashSet<int>>>();
            foreach (var lWord in toReturnBefore)
            {
                if (toReturnBefore2.Select(x => x.Item1).Contains(lWord.Item1))
                    continue;
                toReturnBefore2.Add(lWord);
            }

            var toReturn = new List<Tuple<string, string>>();

            foreach (var lWord in toReturnBefore2)
            {
                toReturn.Add(new Tuple<string,string>(lWord.Item1, GetFullWordForShortenedWord(lWord.Item2, lWord.Item3)));
            }

            RaiseProgressUpdate(new ProgressEventArgs("Processing Latin reference texts... 100%", true));
            return toReturn;
        }

        private List<Tuple<string, string, HashSet<int>>> GetAdditionalWordsWithDoubleLetters(string word)
        {
            var data = new List<List<string>>();
            var wordPart = string.Empty;
            for (var i = 0; i< word.Length; i++)
            {
                if (i == 0)
                {
                    wordPart += word[i];
                    continue;
                }
                if (i < word.Length - 1 && word[i] == word[i + 1])
                {
                    if (wordPart.Length > 0)
                        data.Add(new List<string>{wordPart});
                    wordPart = string.Empty;
                    data.Add(new List<string> { word[i].ToString(), word[i].ToString() + word[i].ToString() });
                    i++;
                    continue;
                }
                wordPart += word[i];
                if (i == word.Length - 1)
                    data.Add(new List<string> { wordPart });
            }
            var results = Combine(data);
            var toReturn = new List<Tuple<string, string, HashSet<int>>>();
            foreach (var item in results)
            {
                if (item.Length < 4)
                    continue;
                var toAdd = new HashSet<int>();
                var origI = 0;
                for(var i = 0; i < word.Length; i++)
                {
                    if (origI >= item.Length)
                    {
                        toAdd.Add(i);
                        continue;
                    }
                    if (word[i] == item[origI])
                        origI++;
                    else
                        toAdd.Add(i);
                }
                toReturn.Add(new Tuple<string, string, HashSet<int>>(item, word, toAdd));
            }
            return toReturn;
        }

        private List<string> GetAdditionalWordsWithSuffix(string word)
        {
            var data = new List<List<string>>();
            var wordPart = string.Empty;
            for (var i = 0; i < word.Length; i++)
            {
                if (i == 0)
                {
                    wordPart += word[i];
                    continue;
                }
                if (i < word.Length - 1 && word[i] == word[i + 1])
                {
                    if (wordPart.Length > 0)
                        data.Add(new List<string> { wordPart });
                    wordPart = string.Empty;
                    data.Add(new List<string> { word[i].ToString(), word[i].ToString() + word[i].ToString() });
                    i++;
                    continue;
                }
                wordPart += word[i];
                if (i == word.Length - 1)
                    data.Add(new List<string> { wordPart });
            }
            return Combine(data);
        }

        private bool IsDipthong(string letters)
        {
            return _latinDipthongs.Contains(letters);
        }

        private bool IsVowel(char letter)
        {
            return "aeiou".IndexOf(letter) >= 0;
        }

        private async Task<Tuple<string, List<int>>> ProcessOriginalText(string originalText, bool isReverse, CancellationToken token)
        {
            var elidedText = string.Empty;
            var finalLocations = new List<int>();
            var wordLocations = new List<int>(); // first letter index
            var words = new List<string>();
            var elidedWords = new List<string>();
            var itr = 0;

            var word = string.Empty;
            int? wordLocation = null;

            for (int curIndex = 0; curIndex < originalText.Length; curIndex++)
            {
                if (token.IsCancellationRequested)
                {
                    RaiseProgressUpdate(new ProgressEventArgs(string.Empty, false));
                    break;
                }
                var letter = originalText[curIndex];
                if ((Char.IsLetter(letter) && Char.ToLower(letter) != 'h') || letter == ' ' || letter == '\n' || letter == '\r' || letter == '\t')
                    letter = Char.ToLower(letter);
                else 
                    continue;

                if (letter == ' ' || letter == '\n' || letter == '\r' || letter == '\t')
                {
                    if (word != string.Empty)
                    {
                        words.Add(word);
                        wordLocations.Add(wordLocation.Value);
                        word = string.Empty;
                        wordLocation = null;
                    }
                    itr++;
                    continue;
                }
                if (wordLocation == null)
                    wordLocation = curIndex;
                word += letter;
            }

            if (word != string.Empty)
            {
                words.Add(word);
                wordLocations.Add(wordLocation.Value);
            }

            itr = 0;
            if (!isReverse)
            {
                foreach (var localWord in words)
                {
                    if (token.IsCancellationRequested)
                    {
                        RaiseProgressUpdate(new ProgressEventArgs(string.Empty, false));
                        break;
                    }
                    if (itr == words.Count - 1)
                    {
                        elidedWords.Add(localWord);
                        continue;
                    }

                    if (IsVowel(words[itr + 1][0]))
                    {
                        if (IsVowel(localWord[localWord.Length - 1]) && !(localWord.Length > 1 && IsDipthong(localWord.Substring(localWord.Length - 2))))
                        {
                            elidedWords.Add(localWord.Substring(0, localWord.Length - 1));
                            itr++;
                            continue;
                        }
                        if (localWord[localWord.Length - 1] == 'm' && IsVowel(localWord[localWord.Length - 2]) && !(localWord.Length > 2 && IsDipthong(localWord.Substring(localWord.Length - 3, 2))))
                        {
                            elidedWords.Add(localWord.Substring(0, localWord.Length - 2));
                            itr++;
                            continue;
                        }
                    }
                    elidedWords.Add(localWord);
                    itr++;
                }
            }
            else
            {
                elidedWords = words;
            }

            int innerItr;
            // for each letter of each word get real index in original text
            if (!isReverse)
            {
                for (var wordIdx = 0; wordIdx < elidedWords.Count; wordIdx++)
                {
                    if (token.IsCancellationRequested)
                    {
                        RaiseProgressUpdate(new ProgressEventArgs(string.Empty, false));
                        break;
                    }
                    if (wordIdx % 10 == 0)
                        RaiseProgressUpdate(new ProgressEventArgs(string.Format("Processing Latin text... {0}%", Math.Round(((double)wordIdx / (double)elidedWords.Count()) * 100, 1)), false));
                    innerItr = wordLocations[wordIdx];

                    for (var curIndex = 0; curIndex < elidedWords[wordIdx].Length; curIndex++)
                    {
                        elidedText += elidedWords[wordIdx][curIndex];
                        finalLocations.Add(innerItr);
                        innerItr++;
                    }
                }
            }
            else
            {
                for (var wordIdx = elidedWords.Count - 1; wordIdx >= 0; wordIdx--)
                {
                    if (token.IsCancellationRequested)
                    {
                        RaiseProgressUpdate(new ProgressEventArgs(string.Empty, false));
                        break;
                    }
                    if (wordIdx % 10 == 0)
                        RaiseProgressUpdate(new ProgressEventArgs(string.Format("Processing Latin text... {0}%", Math.Round(100 - (((double)wordIdx / (double)elidedWords.Count()) * 100), 1)), false));
                    innerItr = wordLocations[wordIdx] + elidedWords[wordIdx].Length - 1; //end index of non-reversed word

                    for (var curIndex = elidedWords[wordIdx].Length - 1; curIndex >= 0; curIndex--)
                    {
                        elidedText += elidedWords[wordIdx][curIndex];  //reversed original text per letter
                        finalLocations.Add(innerItr);
                        innerItr--;
                    }
                }
            }

            RaiseProgressUpdate(new ProgressEventArgs("Processing Latin text... 100%", true));

            return new Tuple<string, List<int>>(elidedText, finalLocations);
        }

        private void RaiseProgressUpdate(ProgressEventArgs args)
        {
            if (ProgressUpdate == null)
                return;
            var del = ProgressUpdate;
            del.BeginInvoke(this, args, null, null);
        }

        private void RaiseResultUpdate(ListResultEventArgs args)
        {
            if (ReturnResultList == null)
                return;
            var del = ReturnResultList;
            del.BeginInvoke(this, args, null, null);
        }

        private void RaisePublishResultsForDoc(DocResultsEventArgs args)
        {
            if (PublishResultsForDoc == null)
                return;
            var del = PublishResultsForDoc;
            del.BeginInvoke(this, args, null, null);
        }

        private List<string> LatinWordsForGreekWord(string greekWord)
        {
            var possibleLetters = new List<List<string>>();
            for (var curIndex = 0; curIndex < greekWord.Length - 1; curIndex++)
            {
                possibleLetters.Add(new List<string>());
                if (_dipthongDictionary.Keys.Contains(greekWord.Substring(curIndex, 2)))
                {
                    possibleLetters.Add(new List<string>());
                    foreach (var dip in _dipthongDictionary[greekWord.Substring(curIndex, 2)])
                    {
                        if (!possibleLetters[curIndex].Contains(dip.Substring(0, 1)))
                            possibleLetters[curIndex].Add(dip.Substring(0, 1));
                        
                        if (!possibleLetters[curIndex + 1].Contains(dip.Substring(1, 1)))
                            possibleLetters[curIndex + 1].Add(dip.Substring(1, 1));
                    }
                    curIndex++;
                    if (curIndex == greekWord.Length - 2)
                    {
                        curIndex++;
                        possibleLetters.Add(new List<string>());
                        if (_greekLatinDictionary.Keys.Contains(greekWord[curIndex]))
                            possibleLetters[curIndex] = _greekLatinDictionary[greekWord[curIndex]];
                    }

                    continue;
                }

                if (_greekLatinDictionary.Keys.Contains(greekWord[curIndex]))
                    possibleLetters[curIndex] = _greekLatinDictionary[greekWord[curIndex]];

                if (curIndex == greekWord.Length - 2)
                {
                    curIndex++;
                    possibleLetters.Add(new List<string>());
                    if (_greekLatinDictionary.Keys.Contains(greekWord[curIndex]))
                        possibleLetters[curIndex] = _greekLatinDictionary[greekWord[curIndex]];
                }
            }

            if (possibleLetters.Any(x => x.Count == 0))
                return new List<string>();

            return Combine(possibleLetters);
        }

        private List<string> Combine(IEnumerable<IEnumerable<string>> sequences)
        {
            IEnumerable<IEnumerable<string>> emptyProduct = new[] { Enumerable.Empty<string>() };

            var listofLists = sequences.Aggregate(
              emptyProduct,
              (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat(new[] { item }));

            return listofLists.Select(x => string.Join(string.Empty, x)).ToList();
        }

        private IEnumerable<int> GetAllIndexes(string source, string matchString)
        {
            return Regex.Matches(source, matchString).Cast<Match>().Select(m => m.Index);
        }

        private List<Tuple<int, int, string, string, string>> FindLatinMatches(Tuple<string, List<int>> originalInfo, List<Tuple<string, string>> latinWords, bool isReverse, CancellationToken token)
        {
            var matchingLocations = new List<Tuple<int, int, string, string, string>>();
            RaiseProgressUpdate(new ProgressEventArgs("Comparing texts...", false));

            foreach (var word in latinWords)
            {
                if (token.IsCancellationRequested)
                {
                    RaiseProgressUpdate(new ProgressEventArgs(string.Empty, false));
                    break;
                }
                var locations = GetAllIndexes(originalInfo.Item1, word.Item1);
                foreach (var location in locations)
                {
                    if (token.IsCancellationRequested)
                    {
                        RaiseProgressUpdate(new ProgressEventArgs(string.Empty, false));
                        break;
                    }
                    if (!isReverse)
                        matchingLocations.Add(new Tuple<int, int, string, string, string>(originalInfo.Item2[location], originalInfo.Item2[location + word.Item1.Length - 1], word.Item1, word.Item2, originalInfo.Item1.Substring(location, word.Item1.Length - 1)));
                    else
                        matchingLocations.Add(new Tuple<int, int, string, string, string>(originalInfo.Item2[location + word.Item1.Length - 1], originalInfo.Item2[location], word.Item1, word.Item2, originalInfo.Item1.Substring(location, word.Item1.Length - 1)));
                }
            }

            return matchingLocations;
        }

        private List<Tuple<int, int, string, string, string>> FindMatches(Tuple<string, List<int>> originalInfo, List<Tuple<string, string>> greekWords, bool isReverse, CancellationToken token)
        {
            var latinSounds = new List<Tuple<string, string, string>>(); //list of latin sounds and greek words with original greek word
            var matchingLocations = new List<Tuple<int, int, string, string, string>>(); //list of start index, end index, greek word, and original greek word, latin word

            RaiseProgressUpdate(new ProgressEventArgs("Comparing texts...", false));

            foreach (var word in greekWords)
            {
                if (token.IsCancellationRequested)
                {
                    RaiseProgressUpdate(new ProgressEventArgs(string.Empty, false));
                    break;
                }
                foreach (var match in LatinWordsForGreekWord(word.Item1))
                {
                    if (token.IsCancellationRequested)
                    {
                        RaiseProgressUpdate(new ProgressEventArgs(string.Empty, false));
                        break;
                    }
                    latinSounds.Add(new Tuple<string, string, string>(match, word.Item1, word.Item2));
                }
            }


            foreach (var sound in latinSounds)
            {
                if (token.IsCancellationRequested)
                {
                    RaiseProgressUpdate(new ProgressEventArgs(string.Empty, false));
                    break;
                }
                var locations = GetAllIndexes(originalInfo.Item1, sound.Item1);
                foreach (var location in locations)
                {
                    if (token.IsCancellationRequested)
                    {
                        RaiseProgressUpdate(new ProgressEventArgs(string.Empty, false));
                        break;
                    }
                    if (!isReverse)
                        matchingLocations.Add(new Tuple<int, int, string, string, string>(originalInfo.Item2[location], originalInfo.Item2[location + sound.Item1.Length - 1], sound.Item2, sound.Item3, sound.Item1));                   
                    else
                        matchingLocations.Add(new Tuple<int, int, string, string, string>(originalInfo.Item2[location + sound.Item1.Length - 1], originalInfo.Item2[location], sound.Item2, sound.Item3, sound.Item1));                   
                }
            }

            return matchingLocations;
        }
    }
}
