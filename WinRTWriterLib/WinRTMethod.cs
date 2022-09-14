namespace WinRTWriterLib
{
    public class WinRTMethod : WinRTEntityWithAttributes
    {
        public WinRTVisibility Visibility { get; private set; }
        public WinRTMethod(string name, WinRTVisibility visibility)
        {
            Name = name;
            Visibility = visibility;
        }
    }
}