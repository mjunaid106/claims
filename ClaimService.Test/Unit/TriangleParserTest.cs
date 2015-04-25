using System;
using System.Collections.Generic;
using System.Linq;
using ClaimsService.Entities;
using ClaimsService.Implementations;
using ClaimsService.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClaimService.Test.Unit
{
    [TestClass]
    public class TriangleParserTest
    {
        [TestClass]
        public class ProcessorTest
        {
            private List<string> _goodData, _goodDataWithFourProducts, _invalidFormatData, _invalidDataWithNoComma, _missingColumn;
            private IDataSource _dataSource;
            private IDataFormatter _dataFormatter;
            private IDataReadResult _dataReadResult;
            private IParser _parser;

            private List<Product> _products;

            [TestInitialize]
            public void Initialise()
            {
                _dataSource = new TextDataSource("input.csv", "output.csv");
                _dataFormatter = new CsvDataFormatter(',');
                _dataReadResult = new DataReadResult();

                _goodData = new List<string>
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

                _goodDataWithFourProducts = new List<string>
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
                    "Non-Comp, 1993, 1993, 100.0",
                    "First-Prod, 1990, 1992, 55.0",
                    "Second-Prod, 1991, 1991, 78.0",
                    "Second-Prod, 1991, 1992, 79.0",
                    "Second-Prod, 1993, 1993, 65.0"
                };

                _invalidFormatData = new List<string>
                {
                    "Product, Origin Year, Development Year, Incremental Value",
                    "Comp, 1A992, 1992, 11A0.0",
                    "Comp, 1992, 1993, 170.0",
                    "Comp, 1993, 1993, 200.0",
                    "Non-Comp, 1990, 1990, 45.2",
                    "Non-Comp, 1990, 1991, 64.8",
                    "Non-Comp, 1990, 1993, 37.0"
                };

                _invalidDataWithNoComma = new List<string>
                {
                    "Product, Origin Year, Development Year, Incremental Value",
                    "Comp 1992 1992 110.0",
                    "Comp, 1992, 1993, 170.0",
                    "Comp, 1993, 1993, 200.0",
                    "Non-Comp, 1990, 1990, 45.2",
                    "Non-Comp, 1990, 1991, 64.8",
                    "Non-Comp, 1990, 1993, 37.0"
                };

                _missingColumn = new List<string>
                {
                    "Product, Origin Year, Development Year, Incremental Value",
                    "Comp, 1992, 1992"
                };


                _dataReadResult = new DataReadResult
                {
                    FirstYear = 1990,
                    LastYear = 1993,
                    Products = new List<Product>
                    {
                        new Product("Comp")
                        {
                            Rows = new SortedList<int, SortedList<int, double>>
                            {
                                {1992, new SortedList<int, double> {{1992, 110}, {1993, 280}}},
                                {1993, new SortedList<int, double> {{1993, 200}}}
                            }
                        },
                        new Product("Non-Comp")
                        {
                            Rows = new SortedList<int, SortedList<int, double>>
                            {
                                {1990, new SortedList<int, double> {{1990, 45.2}, {1991, 64.8}, {1993, 37}}},
                                {1991, new SortedList<int, double> {{1991, 50}, {1992, 75}, {1993, 25}}},
                                {1992, new SortedList<int, double> {{1992, 55}, {1993, 85}}},
                                {1993, new SortedList<int, double> {{1993, 100}}}
                            }
                        }
                    }
                };
                _products = new List<Product>
                { 
                    new Product("Comp") 
                    { 
                        Rows= new SortedList<int,SortedList<int,double>> 
                        {
                                {1990, new SortedList<int, double>{{1990,0},{1991,0},{1992,0},{1993,0}}},
                                {1991, new SortedList<int, double>{{1991,0},{1992,0},{1993,0}}},
                                {1992, new SortedList<int, double>{{1992,110},{1993,280}}},
                                {1993, new SortedList<int, double>{{1993,200}}}
                        } 
                    },
                    new Product("Non-Comp") 
                    { 
                        Rows= new SortedList<int,SortedList<int,double>> 
                        {
                                {1990, new SortedList<int, double>{{1990,45.2},{1991,110},{1992,110},{1993,147}}},
                                {1991, new SortedList<int, double>{{1991,50},{1992,125},{1993,150}}},
                                {1992, new SortedList<int, double>{{1992,55},{1993,140}}},
                                {1993, new SortedList<int, double>{{1993,100}}}
                        } 
                    }
                };
            }

            [TestMethod]
            public void ReadDataSource_GoodData_TwoProducts_Success()
            {
                _dataSource.Data = _goodData;

                _parser = new TriangleParser(_dataSource, _dataFormatter, _dataReadResult);
                _dataReadResult = _parser.ReadDataSource();
                Assert.AreEqual(1990, _dataReadResult.FirstYear);
                Assert.AreEqual(1993, _dataReadResult.LastYear);
                Assert.AreEqual(4, _dataReadResult.NumberOfYears);
                Assert.AreEqual(2, _dataReadResult.Products.Count());

                var firstProduct = _dataReadResult.Products.ElementAt(0);
                Assert.AreEqual("Comp", firstProduct.Name);
                Assert.AreEqual(2, firstProduct.Rows.Count);

                var secondProduct = _dataReadResult.Products.ElementAt(1);
                Assert.AreEqual("Non-Comp", secondProduct.Name);
                Assert.AreEqual(4, secondProduct.Rows.Count);
            }

            [TestMethod]
            public void ReadDataSource_GoodData_FourProducts_Success()
            {
                _dataSource.Data = _goodDataWithFourProducts;

                _parser = new TriangleParser(_dataSource, _dataFormatter, _dataReadResult);
                _dataReadResult = _parser.ReadDataSource();
                Assert.AreEqual(1990, _dataReadResult.FirstYear);
                Assert.AreEqual(1993, _dataReadResult.LastYear);
                Assert.AreEqual(4, _dataReadResult.NumberOfYears);
                Assert.AreEqual(4, _dataReadResult.Products.Count());

                var firstProduct = _dataReadResult.Products.ElementAt(0);
                Assert.AreEqual("Comp", firstProduct.Name);
                Assert.AreEqual(2, firstProduct.Rows.Count);

                var secondProduct = _dataReadResult.Products.ElementAt(1);
                Assert.AreEqual("Non-Comp", secondProduct.Name);
                Assert.AreEqual(4, secondProduct.Rows.Count);

                var thirdProduct = _dataReadResult.Products.ElementAt(2);
                Assert.AreEqual("First-Prod", thirdProduct.Name);
                Assert.AreEqual(1, thirdProduct.Rows.Count);

                var fourthProduct = _dataReadResult.Products.ElementAt(3);
                Assert.AreEqual("Second-Prod", fourthProduct.Name);
                Assert.AreEqual(2, fourthProduct.Rows.Count);
            }

            [TestMethod]
            [ExpectedException(typeof(FormatException))]
            public void ReadDataSource_WithInvalidData_Exception()
            {
                _dataSource.Data = _invalidFormatData;

                _parser = new TriangleParser(_dataSource, _dataFormatter, _dataReadResult);
                _dataReadResult = _parser.ReadDataSource();
            }

            [TestMethod]
            [ExpectedException(typeof(IndexOutOfRangeException))]
            public void ReadDataSource_WithNoCommaData_Exception()
            {
                _dataSource.Data = _invalidDataWithNoComma;

                _parser = new TriangleParser(_dataSource, _dataFormatter, _dataReadResult);
                _dataReadResult = _parser.ReadDataSource();
            }

            [TestMethod]
            [ExpectedException(typeof(IndexOutOfRangeException))]
            public void ReadDataSource_WithMissingColumn_Exception()
            {
                _dataSource.Data = _missingColumn;

                _parser = new TriangleParser(_dataSource, _dataFormatter, _dataReadResult);
                _dataReadResult = _parser.ReadDataSource();
            }

            [TestMethod]
            public void PopulateMissingData_TwoProducts_Success()
            {
                _dataSource.Data = _goodData;

                _parser = new TriangleParser(_dataSource, _dataFormatter, _dataReadResult);

                _parser.PopulateMissingData(_dataReadResult.Products, _dataReadResult.FirstYear, _dataReadResult.LastYear);

                Assert.AreEqual(1990, _dataReadResult.FirstYear);
                Assert.AreEqual(1993, _dataReadResult.LastYear);
                Assert.AreEqual(4, _dataReadResult.NumberOfYears);
                Assert.AreEqual(2, _dataReadResult.Products.Count());

                var firstProduct = _dataReadResult.Products.ElementAt(0);
                Assert.AreEqual("Comp", firstProduct.Name);
                Assert.AreEqual(4, firstProduct.Rows.Count);
                Assert.AreEqual(1990, firstProduct.Rows.ElementAt(0).Key);
                Assert.AreEqual(4, firstProduct.Rows.ElementAt(0).Value.Count);

                var secondProduct = _dataReadResult.Products.ElementAt(1);
                Assert.AreEqual("Non-Comp", secondProduct.Name);
                Assert.AreEqual(4, secondProduct.Rows.Count);
                Assert.AreEqual(1990, secondProduct.Rows.ElementAt(0).Key);
                Assert.AreEqual(4, firstProduct.Rows.ElementAt(0).Value.Count);
            }

            [TestMethod]
            public void GetDataForOutput_Success()
            {
                /*
                 1990, 4
                 Comp, 0, 0, 0, 0, 0, 0, 0, 110, 280, 200
                 Non-Comp, 45.2, 110, 110, 147, 50, 125, 150, 55, 140, 100
                */

                _parser = new TriangleParser(_dataSource, _dataFormatter, _dataReadResult);

                IEnumerable<string> lines = _parser.GetDataForOutput(_products, _dataFormatter);
                Assert.AreEqual(2, lines.Count());
            }
        }
    }
}
