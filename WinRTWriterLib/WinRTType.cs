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
        public static WinRTType Void { get; } = new WinRTBasicType("void");
        public static WinRTType String { get; } = new WinRTBasicType("String");
        public static WinRTType Int32 { get; } = new WinRTBasicType("Int32");
        public static WinRTType Single { get; } = new WinRTBasicType("Single");
        public static WinRTType Double { get; } = new WinRTBasicType("Double");
        public static WinRTType Null { get; } = new WinRTBasicType("<<null_type>>");
    }

    public class WinRTTypeWithMethodsAndProperties : WinRTType
    {

        public List<WinRTMethod> Methods { get; private set; } = new();
        public List<WinRTProperty> Properties { get; private set; } = new();
        public List<WinRTEvent> Events { get; private set; } = new();

        public List<WinRTInterface> Interfaces { get; private set; } = new();
    }
}