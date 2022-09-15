using System.Text.RegularExpressions;

namespace WinRTWriterLib
{
    public class WinRTEntity
    {
        protected bool AllowNamespaceName { get; set; }

        private string? _name;
        public string? Name
        { 
            get => _name; 
            internal set 
            {
                if (!AllowNamespaceName)
                {
                    if (!Regex.IsMatch(value!, @"^[A-Za-z_]\w*$"))
                    {
                        throw new ArgumentException($"Invalid Name: {value}");
                    }
                } else
                {
                    if (!Regex.IsMatch(value!, @"^([A-Za-z_]\w*)(\.([A-Za-z_]\w*))*$"))
                    {
                        throw new ArgumentException($"Invalid Name: {value}");
                    }
                }
                _name = value; 
            } 
        }
    }
}