namespace WinRTWriterLib
{
    public class WinRTProperty : WinRTEntityWithAttributes
    {
        public WinRTType Type { get; private set; }
        public WinRTVisibility Visibility { get; private set; }
        public bool IsReadOnly { get; set; }
        public WinRTProperty(string name, WinRTType type, WinRTVisibility visilibity = WinRTVisibility.Public)
        {
            Name = name;
            Type = type;
            Visibility = visilibity;
        }
    }
}