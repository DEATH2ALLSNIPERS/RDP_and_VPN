using System;

namespace RemoteDesktop.Net
{
    public class NetExceptions: Exception
    {
        public NetExceptions(string message): base(message) {}
    }

    public class IPAdressExceptions : Exception
    {
        public IPAdressExceptions(string message) : base(message) { }
    }

    public class PortNumberExceptions : Exception
    {
        public PortNumberExceptions(string message) : base(message) { }
    }
}
