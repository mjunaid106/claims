using System.Collections.Generic;
using ClaimsService.Interfaces;

namespace ClaimsService.Entities
{
    public class DataReadResult : IDataReadResult
    {
        public int FirstYear { get; set; }

        public int LastYear { get; set; }

        public int NumberOfYears { get { return (LastYear - FirstYear) + 1; } }

        public IEnumerable<IProduct> Products { get; set; }
    }
}
