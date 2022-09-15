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

        public WinRTNamespace DefineNamespace(string namespaceName)
        {
            return DefineMember<WinRTNamespace>(namespaceName);
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