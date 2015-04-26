using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClaimsService.Implementations;
using ClaimsService.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClaimService.Test.Integration
{
    [TestClass]
    public class TextDataSourceTest
    {
        private IDataSource _dataSource;
        private const string InputFilePath = "input.csv";
        private const string OutputFilePath = "output.csv";
        private IEnumerable<string> _processedData;

        [TestInitialize]
        public void Initialise()
        {
            var data = new List<string>
            {
                "Product, Origin Year, Development Year, Incremental Value",
                "Comp, 1992, 1992, 110.0",
                "Comp, 1992, 1993, 170.0",
                "Comp, 1993, 1993, 200.0",
                "Non-Comp, 1990, 1990, 45.2",
                "Non-Comp, 1990, 1991, 64.8",
                "Non-Comp, 1990, 1993, 37.0",
                "Non-Comp, 1991, 1991, 50.0",
                "Non-Comp, 1991, 1992, 75.0",
                "Non-Comp, 1991, 1993, 25.0",
                "Non-Comp, 1992, 1992, 55.0",
                "Non-Comp, 1992, 1993, 85.0",
                "Non-Comp, 1993, 1993, 100.0"
            };

            File.WriteAllLines("input.csv", data);

            _processedData = new List<string>
            {
                "1990, 4",
                "Comp, 0, 0, 0, 0, 0, 0, 0, 110, 280, 200",
                "Non-Comp, 45.2, 110, 110, 147, 50, 125, 150, 55, 140, 100"
            };
        }

        [TestMethod]
        public void Read()
        {
            _dataSource = new TextDataSource(InputFilePath, OutputFilePath);
            IEnumerable<string> readData = _dataSource.Read();
            Assert.AreEqual(13, readData.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void Read_InvalidFilePath()
        {
            const string invalidFile = "invalid_input.csv";
            _dataSource = new TextDataSource(invalidFile, OutputFilePath);
            IEnumerable<string> readData = _dataSource.Read();
            Assert.AreEqual(13, readData.Count());
        }

        [TestMethod]
        public void Save()
        {
            _dataSource = new TextDataSource(InputFilePath, OutputFilePath);
            _dataSource.Write(_processedData, 1990, 4);
        }


        [TestCleanup]
        public void Cleanup()
        {
            File.Delete("input.csv");
            File.Delete("output.csv");
        }
    }
}
