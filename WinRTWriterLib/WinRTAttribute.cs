using System.CodeDom;
using System.Security.Cryptography.X509Certificates;

namespace WinRTWriterLib
{
    public class AttributeUsageValue
    {
        public static AttributeUsageValue[] From(params object[] values)
        {
            return values.Select(_ => FromObject(_)).ToArray();
        }
        public static AttributeUsageValue FromObject(object? value)
        {
            if (value == null) return new AttributeUsageValue(WinRTBasicType.Null, null);
            else if (value is string s) return new AttributeUsageValue(WinRTBasicType.String, s);
            else if (value is int i) return new AttributeUsageValue(WinRTBasicType.Int32, i);
            else if (value is float f) return new AttributeUsageValue(WinRTBasicType.Single, f);
            else if (value is double d) return new AttributeUsageValue(WinRTBasicType.Double, d);
            else throw new NotImplementedException();
        }
        public AttributeUsageValue(WinRTType type, object? value)
        {
            Type = type;
            Value = value;
        }
        public WinRTType? Type { get; set; }
        public object? Value { get; set; }

        public override string ToString()
        {
            if (Type == WinRTBasicType.String)
            {
                return $"\"{Value}\"";
            }
            else if (Type is WinRTEnum enumType)
            {
                return $"{enumType.Name}.{(Value as WinRTEnumValue)!.Name}";
            } else if (Type == null)
            {
                return "null";
            }
            else return Value!.ToString()!;
            
        }
    }

    public class WinRTArgument : WinRTEntityWithAttributes
    {
        public WinRTArgument(string name, WinRTType type)
        {
            Name = name;
            Type = type;
        }

        public WinRTType? Type { get; set; }
    }
    public class WinRTAttributeUsage : WinRTEntity
    {
        public AttributeUsageValue[] Args { get; private set; }
        public WinRTAttributeUsage(string name, params AttributeUsageValue[] args)
        {
            Name = name;
            Args = args;
        }


        internal string ArgumentsString
        {
            get => string.Join(", ", Args.AsEnumerable());
        }
    }
}