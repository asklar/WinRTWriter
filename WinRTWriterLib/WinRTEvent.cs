namespace WinRTWriterLib
{
    public class WinRTEvent : WinRTEntityWithAttributes
    {
        public WinRTVisibility Visibility { get; private set; }
        public WinRTType Type { get; private set; }

        public WinRTEvent(string name, WinRTType type, WinRTVisibility visibility = WinRTVisibility.Public)
        {
            Name = name;
            Type = type;
            Visibility = visibility;
        }
    }
}