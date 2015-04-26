using System;
using System.Collections.Generic;
using System.Linq;
using ClaimsService.Interfaces;

namespace ClaimsService.Implementations
{
    public class TriangleParser : IParser
    {
        public IDataSource DataSource { get; set; }

        public IDataFormatter DataFormatter { get; set; }

        public IDataReadResult DataReadResult { get; set; }

        public TriangleParser(IDataSource dataSource, IDataFormatter dataFormatter, IDataReadResult dataReadResult)
        {
            DataSource = dataSource;
            DataFormatter = dataFormatter;
            DataReadResult = dataReadResult;
        }

        public IDataReadResult ReadDataSource()
        {
            IList<IProduct> products = new List<IProduct>();
            int minYear = 0;
            int maxYear = 0;
            var sourceData = DataFormatter.IsHeaderRowPresent ? DataSource.Data.Skip(1) : DataSource.Data;
            bool isReadSuccess = false;
            DataReadResult.Products = products;

            foreach (string line in sourceData)
            {
                string name;
                int originYear;
                int developmentYear;
                double value;

                try
                {
                    string[] data = line.Split(DataFormatter.Delimiter);
                    name = data[0];
                    originYear = Convert.ToInt32(data[1]);
                    developmentYear = Convert.ToInt32(data[2]);
                    value = Convert.ToDouble(data[3]);
                    isReadSuccess = true;
                }
                catch (Exception ex)
                {
                    // Log data read exception and exit (do not process current file/data).

                    DataReadResult.Reset();
                    return DataReadResult;
                }

                minYear = Math.Min(minYear == 0 ? originYear : minYear, originYear);
                maxYear = Math.Max(maxYear, developmentYear);

                IProduct product = products.FirstOrDefault(a => a.Name == name);

                if (product == null)
                {
                    product = new Product(name);
                    products.Add(product);
                }

                if (product.Rows.ContainsKey(originYear))
                {
                    var row = product.Rows[originYear];
                    row.Add(developmentYear, value);
                }
                else
                {
                    product.Rows[originYear] = new SortedList<int, double> { { developmentYear, value } };
                }
                DataReadResult.FirstYear = minYear;
                DataReadResult.LastYear = maxYear;

            }

            DataReadResult.IsSuccess = isReadSuccess;

            return DataReadResult;
        }

        public void PopulateData(IEnumerable<IProduct> products, int firstYear, int lastYear)
        {
            foreach (IProduct product in products)
            {
                for (int oy = firstYear; oy <= lastYear; oy++)
                {
                    if (product.Rows.ContainsKey(oy))
                    {
                        for (int dy = oy; dy <= lastYear; dy++)
                        {
                            if (!product.Rows[oy].ContainsKey(dy))
                            {
                                product.Rows[oy].Add(dy, dy == oy ? 0 : product.Rows[oy][dy - 1]);
                            }
                            else
                            {
                                if (dy != oy)
                                {
                                    product.Rows[oy][dy] = product.Rows[oy][dy - 1] + product.Rows[oy][dy];
                                }
                            }
                        }
                    }
                    else
                    {
                        var row = new SortedList<int, double>();
                        for (int j = oy; j <= lastYear; j++)
                        {
                            row.Add(j, 0);
                        }
                        product.Rows.Add(oy, row);
                    }
                }
            }
        }

        public IEnumerable<string> GetDataForOutput(IEnumerable<IProduct> products, IDataFormatter dataFormatter)
        {
            var lines = products.Select(s => string.Format("{0},{1} {2}", s.Name, dataFormatter.Delimiter,
                string.Join(string.Format("{0} ", dataFormatter.Delimiter), s.Rows.SelectMany(r => r.Value.Select(d => d.Value)))));
            return lines.ToList();
        }
    }
}
