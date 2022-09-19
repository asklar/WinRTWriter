using WinRTWriterLib;

namespace WinRTWriter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var writer = new IDLWriter();
            var assembly = WinRTNamespace.DefineNamespace("Contoso.UI");

            WinRTEnum @enum = assembly.DefineEnum("MyEnum");
            @enum.Fields.Add(new WinRTEnumValue(@enum, "Value1", 41));
            @enum.Fields.Add(new WinRTEnumValue(@enum, "Value2"));

            var ibutton = assembly.DefineInterface("IButton");
            var render = new WinRTMethod("Render") { ReturnType = WinRTBasicType.Single };
            render.Arguments.Add(new WinRTArgument("width", WinRTBasicType.Int32) {  Attributes = { new WinRTAttributeUsage("optional") } });
            ibutton.Methods.Add(render);

            WinRTRuntimeClass button = assembly.DefineRuntimeClass("Button");
            button.Properties.Add(new ("Text", WinRTBasicType.String));
            button.Properties.Add(new ("Width", WinRTBasicType.Int32) { IsReadOnly = true });
            button.Properties.Add(new("Content", WinRTType.Object));
            button.Methods.Add(new("GetXamlButton") { ReturnType = WinRTType.Create("Windows.UI.Xaml") });
            button.Attributes.Add(new WinRTAttributeUsage("SomeAttribute", AttributeUsageValue.From(42, "foo")));
            button.Interfaces.Add(ibutton);
            button.Properties.Add(new WinRTProperty("EnumValue", @enum, WinRTVisibility.Protected) { IsReadOnly = true });

            writer.Write(new[] { assembly});

            var winmd = new WinMDWriter();
            winmd.Write(new[] { assembly });
        }
    }
}