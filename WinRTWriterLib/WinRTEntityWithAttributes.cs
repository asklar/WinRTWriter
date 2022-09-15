namespace WinRTWriterLib
{
    public class WinRTEntityWithAttributes : WinRTEntity
    {
        public List<WinRTAttributeUsage> Attributes { get; private set; } = new();
    }
}