using WinRTWriterLib;

namespace WinRTWriter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var writer = new Writer();
            var assembly = writer.DefineAssembly("Contoso.UI");

            WinRTEnum @enum = assembly.DefineEnum("MyEnum");
            @enum.Fields.Add(new WinRTEnumValue(@enum, "Value1", 0));
            @enum.Fields.Add(new WinRTEnumValue(@enum, "Value2", 0));

            WinRTRuntimeClass button = assembly.DefineRuntimeClass("Button");
            button.Properties.Add(new WinRTProperty("Text", WinRTBasicType.String));
            button.Properties.Add(new WinRTProperty("Width", WinRTBasicType.Int32) { IsReadOnly = true });
            button.Attributes.Add(new WinRTAttribute("SomeAttribute", new object[] { 42, "foo" }));
            writer.Write();
        }
    }
}