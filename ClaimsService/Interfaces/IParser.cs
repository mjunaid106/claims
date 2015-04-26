using System.Collections.Generic;

namespace ClaimsService.Interfaces
{
    public interface IParser
    {
        IDataSource DataSource { get; set; }
        
        IDataFormatter DataFormatter { get; set; }

        IDataReadResult DataReadResult { get; set; }

        IDataReadResult ReadDataSource();

        void PopulateData(IEnumerable<IProduct> products, int firstYear, int lastYear);

        IEnumerable<string> GetDataForOutput(IEnumerable<IProduct> products, IDataFormatter dataFormatter);
    }
}
