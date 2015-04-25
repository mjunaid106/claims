using System.Collections.Generic;
using ClaimsService.Interfaces;

namespace ClaimsService.Entities
{
    public class Product : IProduct
    {
        public Product(string name)
        {
            Name = name;
            Rows = new SortedList<int, SortedList<int, double>>();
        }

        public string Name { get; set; }

        public SortedList<int, SortedList<int, double>> Rows { get; set; }
    }
}
