namespace WinRTWriterLib
{
    public class WinRTStruct : WinRTType
    {
        public List<StructField> Fields {get;private set;} = new();
    }
}