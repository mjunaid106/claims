using ClaimsService.Interfaces;

namespace ClaimsService.Entities
{
    public class CsvDataFormatter : IDataFormatter
    {
        public CsvDataFormatter(char delimiter)
        {
            Delimiter = delimiter;
        }

        public char Delimiter { get; set; }

        public bool IsHeaderRowPresent { get; set; }
    }
}
