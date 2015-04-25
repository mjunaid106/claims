using System.Collections.Generic;

namespace ClaimsService.Interfaces
{
    public interface IDataReadResult
    {
        int FirstYear { get; set; }

        int LastYear { get; set; }

        int NumberOfYears { get; }

        IEnumerable<IProduct> Products { get; set; }

        
    }
}
