using System.Reflection;
using System.Xml.Linq;

namespace WinRTWriterLib
{
    public class WinRTNamespace : WinRTEntity
    {
        public string AssemblyName { get => base.Name!; }

        public List<WinRTEntity> Members { get; private set; } = new();

        private T DefineMember<T>(string name) where T : WinRTEntity, new()
        {
            var member = new T();
            member.Name = name;
            Members.Add(member);
            return member;
        }


        public static WinRTNamespace DefineNamespace(string dottedNamespace)
        {
            var fragments = dottedNamespace.Split('.');
            if (fragments.Length == 1)
            {
                var ns = new WinRTNamespace() { Name = dottedNamespace };

                return ns;
            }
            else
            {
                var top = DefineNamespace(fragments[0]);
                var parent = top;
                for (int i = 1; i < fragments.Length; i++)
                {
                    var child = parent.DefineMember<WinRTNamespace>(fragments[i]);
                    child.Parent = parent;
                    parent = child;
                }
                return parent; // innermost namespace
            }
        }

        public WinRTRuntimeClass DefineRuntimeClass(string className)
        {
            return DefineMember<WinRTRuntimeClass>(className);
        }

        public WinRTInterface DefineInterface(string interfaceName)
        {
            return DefineMember<WinRTInterface>(interfaceName);
        }

        public WinRTStruct DefineStruct(string name)
        {
            return DefineMember<WinRTStruct>(name);
        }

        public WinRTEnum DefineEnum(string name)
        {
            return DefineMember<WinRTEnum>(name);
        }

        public WinRTDelegate DefineDelegate(string name)
        {
            return DefineMember<WinRTDelegate>(name);
        }
    }
}