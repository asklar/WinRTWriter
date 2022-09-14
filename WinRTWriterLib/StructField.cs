namespace WinRTWriterLib
{
    public class StructField : WinRTEntityWithAttributes
    {
        // TODO: validate Type is a reference type
        WinRTType? Type { get; set; }
    }
}