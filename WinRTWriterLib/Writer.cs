﻿using System.ComponentModel;
using WinRTWriterLib.Templates;

namespace WinRTWriterLib
{
    namespace Templates
    {
        internal interface Transformer
        {
            public string TransformText();
        }
        public partial class NamespaceTemplate : Transformer
        {
            public NamespaceTemplate(WinRTNamespace @namespace, Writer writer)
            {
                Namespace = @namespace;
                Writer = writer;
            }
            public Writer Writer { get; }

            public WinRTNamespace Namespace { get; }
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
        List<WinRTNamespace> namespaces = new();
        public WinRTNamespace DefineNamespace(string assemblyName)
        {
            var fragments = assemblyName.Split('.');
            if (fragments.Length == 1)
            {
                var ns = new WinRTNamespace() { Name = assemblyName };

                namespaces.Add(ns);
                return ns;
            }
            else
            {
                var top = DefineNamespace(fragments[0]);
                var parent = top;
                for (int i = 1; i < fragments.Length; i++)
                {
                    var child = parent.DefineNamespace(fragments[i]);
                    parent = child;
                }
                return parent; // innermost namespace
            }
        }

        public void Write()
        {
            Console.WriteLine("// This file was generated by a tool");
            foreach (var ns in namespaces)
            {
                var output = Transform(ns);
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return transformer;
        }
    }
}