using System;
using System.Collections.Generic;
using ClaimsService.Entities;
using ClaimsService.Implementations;
using ClaimsService.Interfaces;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const string inputFile = "Files\\input.csv";
            const string outputFile = "Files\\output.csv";
            IDataSource textDataSource = new TextDataSource(inputFile, outputFile);

            try
            {
                IEnumerable<string> inputData = textDataSource.Read();
                textDataSource.Data = inputData;

                IDataFormatter csvDataFormatter = new CsvDataFormatter(',');
                IDataReadResult dataReadResult = new DataReadResult();

                IParser triangleParser = new TriangleParser(textDataSource, csvDataFormatter, dataReadResult);
                dataReadResult = triangleParser.ReadDataSource();
                triangleParser.PopulateMissingData(dataReadResult.Products, dataReadResult.FirstYear, dataReadResult.LastYear);

                IEnumerable<string> dataToWrite = triangleParser.GetDataForOutput(dataReadResult.Products, csvDataFormatter);

                textDataSource.Write(dataToWrite, dataReadResult.FirstYear, dataReadResult.NumberOfYears);

                Console.Write("File processed");
            }
            catch (Exception ex)
            {
                Console.Write("Error processing file");
            }

            Console.ReadLine();
        }
    }
}
