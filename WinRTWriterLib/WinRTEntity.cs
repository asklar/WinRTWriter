using System.Text.RegularExpressions;

namespace WinRTWriterLib
{
    public class WinRTEntity
    {
        protected bool AllowNamespaceName { get; set; }

        public WinRTEntity? Parent { get; protected set; }
        public WinRTEntity GetTopLevel()
        {
            if (Parent == null) return this;
            return Parent.GetTopLevel();
        }

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

        public string? FullName
        {
            get
            {
                var parentFN = string.Empty;
                if (Parent != null)
                {
                    parentFN = Parent.FullName + ".";
                }

                return parentFN + Name;
            }
        }
    }
}