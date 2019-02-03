using System.Diagnostics;

namespace RemoteDesktop.Net.RDP
{
    /// <summary>
    /// Creat connection width mstsc to Remote Descop Pulpit
    /// </summary>
    class Mstsc : Net//https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/mstsc
    {
        private const string mstsc = "mstsc.exe";

        /// <summary>Connects you to a session for administering the server.</summary>
        public static readonly string arg_admin = "/admin";
        /// <summary>Starts Remote Desktop Connection in full-screen mode.</summary>
        public static readonly string arg_full = "/f";
        /// <summary>Runs Remote Desktop in public mode. In public mode, passwords and bitmaps are not cached.</summary>
        public static readonly string arg_public = "/public";
        /// <summary>Matches the Remote Desktop width and height with the local virtual desktop, spanning across multiple monitors if necessary.</summary>
        public static readonly string arg_span = "/span";

        /// <summary>
        /// Initialize Mstsc class.
        /// </summary>
        public Mstsc()
        {
            FileName = mstsc;
        }

        /// <summary>
        /// Connect to RDP.
        /// </summary>
        /// <param name="ServerPort">Specifies the remote computer and, optionally, the port number to which you want to connect.</param>
        /// <returns></returns>
        public bool ConnectRDP(string ServerPort)//[/v:<Server>[:<Port>]] 
        {
            return ServerPort.StartsWith("/v:") ? Connect(ServerPort) : Connect($"/v:{ServerPort}");
        }

        /// <summary>
        /// Connect to RDP using <see cref="IPPort"/>.
        /// </summary>
        /// <param name="ServerPort">Specifies the remote computer and, optionally, the port number to which you want to connect.</param>
        /// <returns></returns>
        public bool ConnectRDP(IPPort ServerPort)
        {
            return Connect(ServerPort.Parameter);
        }

        /// <summary>
        /// Connect to RDP width parameter
        /// </summary>
        /// <param name="ServerPort">Specifies the remote computer and, optionally, the port number to which you want to connect.</param>
        /// <param name="Arguments">Opctional parameters to connection</param>
        /// <returns></returns>
        public bool ConnectRDP(string ServerPort, params string[] Arguments)//[/admin] [/f] [/w:<Width> /h:<Height>] [/public] [/span]
        {
            string arg = null;
            for (int i = 0; i < Arguments.Length; i++)
            {
                arg += Arguments[i] + " ";
            }
            return ConnectRDP($"{ServerPort} {arg}");
        }

        /// <summary>
        /// Connect to RDP using <see cref="IPPort"/> width parameter.
        /// </summary>
        /// <param name="ServerPort">Specifies the remote computer and, optionally, the port number to which you want to connect.</param>
        /// <param name="Arguments">Opctional parameters to connection</param>
        /// <returns></returns>
        public bool ConnectRDP(IPPort ServerPort, params string[] Arguments)//[/admin] [/f] [/w:<Width> /h:<Height>] [/public] [/span]
        {
            return ConnectRDP(ServerPort.ToString(), Arguments);
        }

        /// <summary>
        /// List of opened mstsc applications.
        /// </summary>
        /// <returns>List of opened mstsc process</returns>
        public static Process[] OpenedMstsc()
        {
            // Show all processes on the local computer.
            return Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(mstsc));
        }

        /// <summary>
        /// Show argument help list.
        /// </summary>
        public void ShowArgumentHelp()
        {
            Connect("/?");
        }

    }
}
