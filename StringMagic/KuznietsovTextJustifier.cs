using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StringMagic {
    class KuznietsovTextJustifier : ITextJustifier {

        private string GetAlignmentString(List<string> wordList, int wide) {
            if (wordList.Count == 1) {
                return wordList[0];
            }
            string newStr = "";
            
            int totalWordsLenght = 0;
            foreach (string word in wordList) {
                totalWordsLenght += word.Length;
            }
            int ost = (wide - totalWordsLenght) % (wordList.Count - 1);
            string[] spaces = new string[wordList.Count - 1];
            int spaceSize = (wide - totalWordsLenght) / (wordList.Count - 1);
            for (int i = 0; i < spaces.Length; i++) {
                spaces[i] = new string(' ', spaceSize);
            }
            int ostRight = ost / 2;
            int ostLeft = ost - ostRight;

            for (int i = 0; ostLeft != 0; i++, ostLeft--) {
                spaces[i] += ' ';
            }
            for (int i = spaces.Length - 1; ostRight != 0; i--, ostRight--) {
                spaces[i] += ' ';
            }
            for (int i = 0; i < wordList.Count - 1; i++) {
                newStr += wordList[i] + spaces[i];
            }
            newStr += wordList[wordList.Count - 1];
            return newStr;
        }

        private void CutLongWords(List<string> wordList, int stringSize) {
            for (int i = 0; i < wordList.Count; i++) {
                if (wordList[i].Length <= stringSize) {
                    continue;
                }
                string tmpString = wordList[i];
                List<string> cutedWord = new List<string>();

                while (tmpString.Length > stringSize) {
                    cutedWord.Add(tmpString.Substring(0, stringSize));
                    tmpString = tmpString.Substring(stringSize);
                }
                cutedWord.Add(tmpString);
                wordList.Remove(wordList[i]);
                wordList.InsertRange(i, cutedWord);
            }
        }

        //Another way to use alignment
        /*private List<string> GetWords(string text) {
            List<string> list = new List<string>();
            bool space = false;
            int characterIndex = 0;
            for (int i = 0; i < text.Length; i++) {
                if (space && text[i] != ' ') {
                    list.Add(text.Substring(characterIndex, i - characterIndex));
                    space = false;
                    characterIndex = i;
                }
                if (text[i] == ' ') {
                    space = true;
                } else {
                    space = false;
                }

            }
            list.Add(text.Substring(characterIndex, text.Length - characterIndex));
            return list;
        }*/

        public string Justify(string text, int maxLineWidth) {
            StringBuilder stringBuilder = new StringBuilder();
            string clearString = Regex.Replace(text, @"[\t\r\n]+", " ");
            clearString = Regex.Replace(clearString, @"[ ]+", " ");
            clearString = Regex.Replace(clearString, @"[^0-9A-Za-z А-Яа-я]+", string.Empty);
            List<string> words = clearString.Split(' ').ToList();
            CutLongWords(words, maxLineWidth);

            int currentLineSize = 0;
            List<string> wordList = new List<string>();
            for (int i = 0; i < words.Count; i++) {
                if (currentLineSize + words[i].Length > maxLineWidth) {
                    stringBuilder.AppendLine(GetAlignmentString(wordList, maxLineWidth));
                    wordList.Clear();
                    currentLineSize = 0;
                    i--;
                } else {
                    wordList.Add(words[i]);
                    currentLineSize += words[i].Length + 1;
                }
            }
            if (currentLineSize != 0) {
                stringBuilder.Append(GetAlignmentString(wordList, maxLineWidth));
            }
            return stringBuilder.ToString();
        }
    }
}
