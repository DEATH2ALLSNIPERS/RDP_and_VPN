using System.Diagnostics;

namespace RemoteDesktop.Net
{
    /*
    public enum NetStatus
    {
        Disconnected,
        Connnected,
        AlreadyConnected,
        NoConnections,
    }
    */

    public interface IConnection
    {
        string FileName { get; }
        bool Connect(string Arguments);
        bool Connect(params string[] Arguments);
        bool Connect(string Parameter, params string[] Arguments);
        void ShowArgumentHelp(); 
    }

    public abstract class AOpenCMD : IConnection
    {
        public abstract string FileName { get; }

        public abstract bool Connect(string arguments);

        public abstract bool Connect(params string[] arguments);

        public abstract bool Connect(string parameter, params string[] arguments);

        /// <summary>
        /// Show argument help list.
        /// </summary>
        public void ShowArgumentHelp()
        {
            Process.Start($"{FileName} /?");
        }

        public override string ToString() => $"File:{FileName}";
    }

}
