namespace WinRTWriterLib
{
    public class WinRTMethod : WinRTEntityWithAttributes
    {
        public WinRTVisibility Visibility { get; private set; }
        public WinRTMethod(string name, WinRTVisibility visibility = WinRTVisibility.Public)
        {
            Name = name;
            Visibility = visibility;
        }
        public WinRTType ReturnType { get; set; } = WinRTBasicType.Void;
        public List<WinRTArgument> Arguments { get; private set; } = new();
    }
}