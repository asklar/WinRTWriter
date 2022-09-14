using System.Security.Cryptography.X509Certificates;

namespace WinRTWriterLib
{
    public class NamedValue : WinRTEntity
    {
        public NamedValue(string name, WinRTType type)
        {
            Name = name;
            Type = type;
        }
        public WinRTType Type { get; set; }
        public object? Value { get; set; }
    }
    public class WinRTAttribute : WinRTEntity
    {
        public object[] Args { get; private set; }
        public WinRTAttribute(string name, params object[] args)
        {
            Name = name;
            Args = args;
        }

        private string Quote(object _)
        {
            if (_ is string) return $"\"{_}\"";
            else if (_ is WinRTEnumValue enumValue) return $"{enumValue.EnumType.Name}.{enumValue.Name}";
            else return _.ToString()!;
        }

        internal string ArgumentsString
        {
            get => string.Join(", ", Args.Select(Quote));
        }
    }
}