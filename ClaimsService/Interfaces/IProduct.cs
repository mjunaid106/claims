using System.Collections.Generic;

namespace ClaimsService.Interfaces
{
    public interface IProduct
    {
        string Name { get; set; }
        SortedList<int, SortedList<int, double>> Rows { get; set; }
    }
}
