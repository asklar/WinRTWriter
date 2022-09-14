using WinRTWriterLib.Templates;

namespace WinRTWriterLib
{
    namespace Templates
    {
        internal interface Transformer
        {
            public string TransformText();
        }
        public partial class AssemblyTemplate : Transformer
        {
            public AssemblyTemplate(WinRTAssembly asm, Writer writer)
            {
                Assembly = asm;
                Writer = writer;
            }
            public Writer Writer { get; }

            public WinRTAssembly Assembly { get; }
        }

        public partial class RuntimeClassTemplate : Transformer
        {
            public RuntimeClassTemplate(WinRTRuntimeClass @class, Writer writer)
            {
                Class = @class;
                Writer = writer;
            }
            public Writer Writer { get; }
            public WinRTRuntimeClass Class { get; }
        }

        public partial class PropertyTemplate : Transformer
        {
            public PropertyTemplate(WinRTProperty property, Writer writer)
            {
                Property = property;
                Writer = writer;
            }
            public Writer Writer { get; }
            public WinRTProperty Property { get; }
        }

        public partial class AttributeTemplate: Transformer
        {
            public AttributeTemplate(WinRTAttribute attribute, Writer writer)
            {
                Attribute = attribute;
                Writer = writer;
            }
            public Writer Writer { get; }
            public WinRTAttribute Attribute { get; }
        }
    }

    public class Writer
    {
        List<WinRTAssembly> assemblies = new();
        public WinRTAssembly DefineAssembly(string assemblyName)
        {
            var asm = new WinRTAssembly(assemblyName);
            assemblies.Add(asm);
            return asm;
        }

        public void Write()
        {
            foreach (var assembly in assemblies)
            {
                var output = Transform(assembly);
                Console.WriteLine(output);
            }
        }

        internal int Indent { get; set; }
        internal string Transform(WinRTEntity entity)
        {
            Transformer? transformer = GetTransformer(entity);
            var output = transformer?.TransformText() ?? string.Empty;
            var indented = output.Split(Environment.NewLine);
            var res = string.Join(Environment.NewLine, indented.Select(x => string.IsNullOrWhiteSpace(x) ? x : (Indent == 0 ? x : new string(' ', 4) + x)));
            return res;

        }

        private Transformer? GetTransformer(WinRTEntity entity)
        {
            Transformer? transformer = null;
            var transformerName = $"WinRTWriterLib.Templates.{entity.GetType().Name.Replace("WinRT", "")}Template";
            try
            {
                var type = Type.GetType(transformerName);
                var ctor = type!.GetConstructor(new Type[] { entity.GetType(), this.GetType() });
                transformer = ctor!.Invoke(new object[] { entity, this }) as Transformer;
                return transformer;
            }
            catch { }
            if (entity is WinRTAssembly assembly)
            {
                transformer = new AssemblyTemplate(assembly, this);
            }
            else if (entity is WinRTRuntimeClass @class)
            {
                transformer = new RuntimeClassTemplate(@class, this);
            }
            else if (entity is WinRTProperty prop)
            {
                transformer = new PropertyTemplate(prop, this);
            }

            return transformer;
        }
    }
}