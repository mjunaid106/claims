using System.Collections.Generic;

namespace ClaimsService.Interfaces
{
    public interface IDataSource
    {
        IEnumerable<string> Data { get; set; }

        IEnumerable<string> Read();

        void Write(IEnumerable<string> data, int firstYear, int numberOfYears);
    }
}