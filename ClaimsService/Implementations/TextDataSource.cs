using System;
using System.Collections.Generic;
using System.IO;
using ClaimsService.Interfaces;

namespace ClaimsService.Implementations
{
    public class TextDataSource : IDataSource
    {
        public TextDataSource(string inputFilePath, string outputFilePath)
        {
            InputFilePath = inputFilePath;
            OutputFilePath = outputFilePath;
        }

        public string InputFilePath { get; set; }

        public string OutputFilePath { get; set; }

        public IEnumerable<string> Data { get; set; }

        public IEnumerable<string> Read()
        {
            return File.ReadLines(InputFilePath);
        }

        public void Write(IEnumerable<string> data, int firstYear, int numberOfYears)
        {
            File.WriteAllText(OutputFilePath, string.Format("{0}, {1}{2}", firstYear, numberOfYears, Environment.NewLine));
            File.AppendAllLines(OutputFilePath, data);
        }
    }
}
