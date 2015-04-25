namespace ClaimsService.Interfaces
{
    public interface IDataFormatter
    {
        char Delimiter { get; set; }

        bool IsHeaderRowPresent { get; set; }
    }
}
