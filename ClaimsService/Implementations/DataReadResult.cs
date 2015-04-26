using System.Collections.Generic;
using ClaimsService.Interfaces;

namespace ClaimsService.Implementations
{
    public class DataReadResult : IDataReadResult
    {
        public int FirstYear { get; set; }

        public int LastYear { get; set; }

        public int NumberOfYears { get { return (LastYear - FirstYear) + 1; } }

        public IEnumerable<IProduct> Products { get; set; }

        public bool IsSuccess { get; set; }

        public void Reset()
        {
            FirstYear = 0;
            LastYear = 0;
            Products = null;
            IsSuccess = false;
        }

        public DataReadResult()
        {
            IsSuccess = false;
        }
    }
}
