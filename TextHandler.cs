using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace BelovTextHandlerApp
{
    public class TextHandler
    {
        public void ProcessFile(string inputFilePath, string outputFilePath, int minWordLength, bool removePunctuation)
        {
            try
            {
                // Чтение текста из входного файла
                string text = File.ReadAllText(inputFilePath);

                // Обработка текста
                if (removePunctuation)
                {
                    text = RemovePunctuation(text);
                }

                if (minWordLength > 0)
                {
                    text = RemoveShortWords(text, minWordLength);
                }

                // Запись обработанного текста в выходной файл
                File.WriteAllText(outputFilePath, text);

                Trace.WriteLine("Файл успешно обработан.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Ошибка обработки файла: " + ex.Message);
            }
        }

        public void ProcessFileList(List<string> inputFilePaths, List<string> outputFilePaths, int minWordLength, bool removePunctuation)
        {
            for (int i = 0; (i < inputFilePaths.Count) && (i < outputFilePaths.Count); i++)
            {
                ProcessFile(inputFilePaths[i], outputFilePaths[i], minWordLength, removePunctuation);
            }
        }

        private string RemovePunctuation(string text)
        {
            // Удаление знаков препинания с использованием регулярного выражения
            return Regex.Replace(text, @"[\p{P}\p{S}]", "");
        }

        private string RemoveShortWords(string text, int minWordLength)
        {
            string[] words = text.Split(new char[] { ' ', '\n', '\r', '\t' },
                StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length < minWordLength)
                {
                    words[i] = string.Empty;
                }
            }
            return string.Join(" ", words);
        }
    }
}
