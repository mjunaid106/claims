using ClaimsService.Interfaces;

namespace ClaimsService.Entities
{
    public class CsvDataFormatter : IDataFormatter
    {
        public char Delimiter { get; set; }

        public bool IsHeaderRowPresent { get; set; }

        public CsvDataFormatter(char delimiter, bool isHeaderRowPresent)
        {
            Delimiter = delimiter;
            IsHeaderRowPresent = isHeaderRowPresent;
        }
    }
}
