namespace WinRTWriterLib
{
    public class WinRTEntityWithAttributes : WinRTEntity
    {
        public List<WinRTAttribute> Attributes { get; private set; } = new();
    }
}