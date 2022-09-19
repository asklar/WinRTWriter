namespace WinRTWriterLib
{
    public interface IWriter
    {
        void Write(IEnumerable<WinRTNamespace> namespaces);
        void EnterScope();
        void ExitScope();

    }
}