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

        public partial class InterfaceTemplate : Transformer
        {
            public InterfaceTemplate(WinRTInterface @interface, Writer writer)
            {
                Interface = @interface;
                Writer = writer;
            }
            public Writer Writer { get; }
            public WinRTInterface Interface { get; }
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

        public partial class MethodTemplate : Transformer
        {
            public MethodTemplate(WinRTMethod method, Writer writer)
            {
                Method = method;
                Writer = writer;
            }
            public Writer Writer { get; }
            public WinRTMethod Method { get; }
        }

        public partial class AttributeUsageTemplate : Transformer
        {
            public AttributeUsageTemplate(WinRTAttributeUsage attribute, Writer writer)
            {
                Attribute = attribute;
                Writer = writer;
            }
            public Writer Writer { get; }
            public WinRTAttributeUsage Attribute { get; }
        }

        public partial class EnumTemplate : Transformer
        {
            public EnumTemplate(WinRTEnum @enum, Writer writer)
            {
                Enum = @enum;
                Writer = writer;
            }
            public Writer Writer { get; }
            public WinRTEnum Enum { get; }
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
        internal string Transform(WinRTEntity entity, bool withIndent = true)
        {
            Transformer? transformer = GetTransformer(entity);
            var output = transformer?.TransformText() ?? string.Empty;
            if (!withIndent) return output;
            var indented = output.Split(Environment.NewLine);
            var res = string.Join(Environment.NewLine, indented.Select(x => string.IsNullOrWhiteSpace(x) ? x : (Indent == 0 ? x : new string(' ', 4) + x)));
            return res;

        }

        internal string Transform(List<WinRTAttributeUsage> attrs, bool withNewlines)
        {
            if (withNewlines)
            {
                var t = attrs.Select(_ => Transform(_, false));
                return string.Join(Environment.NewLine, t);
            }
            else
            {
                var oldIndent = Indent;
                Indent = 0;
                var ret = string.Concat(attrs.Select(_ => Transform(_, true)));
                Indent = oldIndent;
                return ret;
            }
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