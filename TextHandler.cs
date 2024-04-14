using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace BelovTextHandlerApp
{
    public class TextHandler
    {
        public async Task ProcessFileAsync(string inputFilePath, string outputFilePath, int minWordLength, bool removePunctuation)
        {
            try
            {
                string text = await ReadTextFromFileAsync(inputFilePath);

                if (removePunctuation)
                {
                    text = RemovePunctuation(text);
                }

                if (minWordLength > 0)
                {
                    text = RemoveShortWords(text, minWordLength);
                }

                await WriteTextToFileAsync(outputFilePath, text);

                Trace.WriteLine("Файл успешно обработан.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Ошибка обработки файла: " + ex.Message);
            }
        }

        public async Task ProcessFileListAsync(
            List<string> inputFilePaths, List<string> outputFilePaths,
            int minWordLength, bool removePunctuation)
        {
            for (int i = 0; (i < inputFilePaths.Count) && (i < outputFilePaths.Count); i++)
            {
                await ProcessFileAsync(inputFilePaths[i], outputFilePaths[i], minWordLength, removePunctuation);
            }
        }

        // Make all files process concurrently, mwahaha
        // RIP CPU (or not tbh)
        public async Task ProcessFilesConcurrentlyAsync(List<string> inputFilePaths, List<string> outputFilePaths, int minWordLength, bool removePunctuation)
        {
            await Task.WhenAll(inputFilePaths.Select((inputFilePath, index) =>
                ProcessFileAsync(inputFilePath, outputFilePaths[index], minWordLength, removePunctuation)));
        }

        private async Task<string> ReadTextFromFileAsync(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private async Task WriteTextToFileAsync(string filePath, string text)
        {
            using (FileStream fileStream = 
                new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    await writer.WriteAsync(text);
                }
            }
        }

        private string RemovePunctuation(string text)
        {
            // Removing punctuation using
            // regular expression - Regex
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