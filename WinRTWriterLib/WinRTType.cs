namespace WinRTWriterLib
{
    public class WinRTType : WinRTEntityWithAttributes
    {
        
    }

    public class WinRTBasicType : WinRTType
    {
        internal WinRTBasicType(string name)
        {
            Name = name;
        }
        public static WinRTType String { get; } = new WinRTBasicType("String");
        public static WinRTType Int32 { get; } = new WinRTBasicType("Int32");
    }

    public class WinRTTypeWithMethodsAndProperties : WinRTType
    {

        public List<WinRTMethod> Methods { get; private set; } = new();
        public List<WinRTProperty> Properties { get; private set; } = new();
    }
}