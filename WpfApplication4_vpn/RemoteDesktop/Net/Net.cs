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

    class Net
    {
        protected string FileName { get; set; }

        /// <summary>
        /// Connect to application
        /// </summary>
        /// <param name="Arguments">Options for connections</param>
        /// <returns>true if succesfull conection</returns>
        virtual protected bool Connect(string Arguments)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = FileName;
            p.StartInfo.Arguments = Arguments;
            return p.Start();
        }

        public override string ToString() => $"File:{FileName}";
    }
}
