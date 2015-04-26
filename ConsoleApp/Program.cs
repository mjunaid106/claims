using System;
using System.Collections.Generic;
using System.IO;
using ClaimsService.Implementations;
using ClaimsService.Interfaces;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = string.Format("{0}\\..\\..\\Files\\", Environment.CurrentDirectory);
            string inputFile = string.Format("{0}input.csv", path);
            string outputFile = string.Format("{0}output.csv", path);
            IDataSource textDataSource = new TextDataSource(inputFile, outputFile);

            try
            {
                Console.WriteLine("Input Data:");
                Console.WriteLine("===========");
                Console.WriteLine(File.ReadAllText(inputFile));

                IEnumerable<string> inputData = textDataSource.Read();
                textDataSource.Data = inputData;

                IDataFormatter csvDataFormatter = new CsvDataFormatter(',', true);
                IDataReadResult dataReadResult = new DataReadResult();

                IParser triangleParser = new TriangleParser(textDataSource, csvDataFormatter, dataReadResult);
                dataReadResult = triangleParser.ReadDataSource();
                triangleParser.PopulateData(dataReadResult.Products, dataReadResult.FirstYear, dataReadResult.LastYear);

                IEnumerable<string> dataToWrite = triangleParser.GetDataForOutput(dataReadResult.Products, csvDataFormatter);

                textDataSource.Write(dataToWrite, dataReadResult.FirstYear, dataReadResult.NumberOfYears);

                Console.WriteLine();
                Console.WriteLine("File processed");
                Console.WriteLine();
                Console.WriteLine("Output Data:");
                Console.WriteLine("===========");
                Console.WriteLine(File.ReadAllText(outputFile));
            }
            catch (Exception ex)
            {
                Console.Write("Error processing file");
            }

            Console.ReadLine();
        }
    }
}
