namespace WinRTWriterLib
{
    public class WinRTEnumValue : WinRTEntityWithAttributes
    {
        public int Value { get; set; }
        public WinRTEnum EnumType { get; private set; }
        public WinRTEnumValue(WinRTEnum enumType, string name, int? value = null)
        {
            EnumType = enumType;
            Name = name;
            Value = value ?? EnumType.Fields.Max(_ => _.Value) + 1;
        }
    }
}