namespace WinRTWriterLib
{
    public class WinRTAssembly : WinRTEntity
    {
        public WinRTAssembly(string assemblyName)
        {
            Name = assemblyName;
        }

        public string AssemblyName { get => base.Name!; }

        public List<WinRTType> Members { get; private set; } = new();

        private T DefineMember<T>(string name) where T : WinRTType, new()
        {
            var member = new T();
            member.Name = name;
            Members.Add(member);
            return member;
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