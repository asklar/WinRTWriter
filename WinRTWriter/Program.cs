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
            @enum.Fields.Add(new WinRTEnumValue(@enum, "Value2"));

            var ibutton = assembly.DefineInterface("IButton");
            var render = new WinRTMethod("Render") { ReturnType = WinRTBasicType.Single };
            render.Arguments.Add(new WinRTArgument("width", WinRTBasicType.Int32) {  Attributes = { new WinRTAttributeUsage("optional") } });
            ibutton.Methods.Add(render);

            WinRTRuntimeClass button = assembly.DefineRuntimeClass("Button");
            button.Properties.Add(new WinRTProperty("Text", WinRTBasicType.String));
            button.Properties.Add(new WinRTProperty("Width", WinRTBasicType.Int32) { IsReadOnly = true });
            button.Attributes.Add(new WinRTAttributeUsage("SomeAttribute", AttributeUsageValue.From(42, "foo")));
            button.Interfaces.Add(ibutton);
            

            writer.Write();
        }
    }
}