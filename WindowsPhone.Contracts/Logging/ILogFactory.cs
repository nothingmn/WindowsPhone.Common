using System;

namespace WindowsPhone.Contracts.Logging
{
    public interface ILogFactory
    {
        ILog GetLogger(Type T);

        ILog GetLogger(string Name);
    }
}