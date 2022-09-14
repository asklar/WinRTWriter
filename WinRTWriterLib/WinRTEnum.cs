namespace WinRTWriterLib
{
    public class WinRTEnum : WinRTType
    {
        public List<WinRTEnumValue> Fields { get; private set; } = new();
    }
}