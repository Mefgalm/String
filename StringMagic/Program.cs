using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StringMagic {
    class Program {
        private const string OUTPUT_FILE_NAME = "OutPut.txt";
        private const string INPUT_FILE_NAME = "input.txt";

        static string GetAlignmentString(List<string> wordList, int wide) {
            string newStr = "";
            if( wordList.Count == 1 ) {
                return wordList[0];
            } else {
                int totalWordsLenght = 0;
                foreach(string word in wordList) {
                    totalWordsLenght += word.Length;
                }
                int ost = (wide - totalWordsLenght) % (wordList.Count - 1);
                string[] spaces = new string[wordList.Count - 1];
                int spaceSize = (wide - totalWordsLenght) / (wordList.Count - 1);
                for (int i = 0; i < spaces.Length; i++) {
                    spaces[i] = new string(' ', spaceSize);
                }
                for (int i = 0; ost != 0; i++, ost--) {
                    spaces[i] += ' ';
                }
                for(int i = 0; i < wordList.Count - 1; i++) {
                    newStr += wordList[i] + spaces[i];
                }
                newStr += wordList[wordList.Count - 1];
            }
            return newStr;
        }

        static void CutLongWords(List<string> wordList, int stringSize) {
            for(int i =0; i < wordList.Count; i++) {
                if(wordList[i].Length <= stringSize) {
                    continue;
                }
                string tmpString = wordList[i];
                List<string> cutedWord = new List<string>();

                while (tmpString.Length > stringSize) {
                    cutedWord.Add(tmpString.Substring(0, stringSize));
                    tmpString = tmpString.Substring(stringSize);
                }
                cutedWord.Add(tmpString);
                if (cutedWord.Count != 0) {
                    wordList.Remove(wordList[i]);
                    wordList.InsertRange(i, cutedWord);
                }
            }
        }

        static string RemoveSpecialChars(string input) {
            return Regex.Replace(input, @"[^0-9A-Za-z ]", string.Empty);
        }

        static void Main(string[] args) {
            string text = File.ReadAllText(INPUT_FILE_NAME);
            string[] dataStr = text.Split(new char[] { '\n', '\r' }, 2, StringSplitOptions.RemoveEmptyEntries);
            int stringSize = int.Parse(dataStr[0]);
            List<string> words = RemoveSpecialChars(dataStr[1]).Split(' ').ToList();
            CutLongWords(words, stringSize);

            StreamWriter file = new StreamWriter(OUTPUT_FILE_NAME);
            try {
                int currentLineSize = 0;
                List<string> wordList = new List<string>();
                for (int i = 0; i < words.Count; i++) {
                    if (currentLineSize + words[i].Length > stringSize) {
                        file.WriteLine(GetAlignmentString(wordList, stringSize));
                        wordList.Clear();
                        currentLineSize = 0;
                        i--;
                    } else {
                        wordList.Add(words[i]);
                        currentLineSize += words[i].Length + 1;
                    }
                }
                if (currentLineSize != 0) {
                    file.WriteLine(GetAlignmentString(wordList, stringSize));
                }
            } finally {
                file.Close();
            }
        }
    }
}
