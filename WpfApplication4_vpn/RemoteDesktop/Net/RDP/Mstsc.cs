using System.Diagnostics;

namespace RemoteDesktop.Net.RDP
{
    /// <summary>
    /// Creat connection width mstsc to Remote Desktop Copmuter
    /// </summary>
    public class Mstsc : AOpenCMD//https://docs.microsoft.com/en-us/windows-server/administration/windows-commands/mstsc
    {
        private readonly string mstsc = "mstsc.exe";
        public override string FileName { get => mstsc; }

        /// <summary>Connects you to a session for administering the server.</summary>
        public static readonly string arg_admin = "/admin";
        /// <summary>Starts Remote Desktop Connection in full-screen mode.</summary>
        public static readonly string arg_full = "/f";
        /// <summary>Runs Remote Desktop in public mode. In public mode, passwords and bitmaps are not cached.</summary>
        public static readonly string arg_public = "/public";
        /// <summary>Matches the Remote Desktop width and height with the local virtual desktop, spanning across multiple monitors if necessary.</summary>
        public static readonly string arg_span = "/span";

        /// <summary>
        /// Connect to RDP.
        /// </summary>
        /// <param name="ServerPort">Specifies the remote computer and, optionally, the port number to which you want to connect.</param>
        /// <returns></returns>
        public override bool Connect(string ServerPort)//[/v:<Server>[:<Port>]] 
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = FileName;
            p.StartInfo.Arguments = ServerPort.StartsWith("/v:") ? ServerPort : $"/v:{ServerPort}";
            return p.Start();
        }

        /// <summary>
        /// Connect to RDP using <see cref="IPPort"/>.
        /// </summary>
        /// <param name="ServerPort">Specifies the remote computer and, optionally, the port number to which you want to connect.</param>
        /// <returns></returns>
        public bool Connect(IPPort ServerPort)
        {
            return Connect(ServerPort.Parameter);
        }

        /// <summary>
        /// Connect to RDP width parameter
        /// </summary>
        /// <param name="ServerPort">Specifies the remote computer and, optionally, the port number to which you want to connect.</param>
        /// <param name="Arguments">Opctional parameters to connection</param>
        /// <returns></returns>
        public override bool Connect(string ServerPort, params string[] Arguments)//[/admin] [/f] [/w:<Width> /h:<Height>] [/public] [/span]
        {
            string ipaddress = ServerPort.StartsWith("/v:") ? ServerPort : $"/v:{ServerPort}";

            return Connect($"{ipaddress} {string.Join(" ", Arguments)}");
        }

        /// <summary>
        /// Connect to RDP width parameter
        /// </summary>
        /// <param name="Arguments">Opctional parameters to connection</param>
        /// <returns></returns>
        public override bool Connect(params string[] Arguments)
        {
            return Connect(string.Join(" ", Arguments));
        }

        /// <summary>
        /// Connect to RDP using <see cref="IPPort"/> width parameter.
        /// </summary>
        /// <param name="ServerPort">Specifies the remote computer and, optionally, the port number to which you want to connect.</param>
        /// <param name="Arguments">Opctional parameters to connection</param>
        /// <returns></returns>
        public bool Connect(IPPort ServerPort, params string[] Arguments)//[/admin] [/f] [/w:<Width> /h:<Height>] [/public] [/span]
        {
            return Connect(ServerPort.ToString(), Arguments);
        }

        /// <summary>
        /// List of opened mstsc applications.
        /// </summary>
        /// <returns>List of opened mstsc process</returns>
        public static Process[] OpenedMstsc()
        {
            // Show all processes on the local computer.
            return Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension("mstsc.exe"));
        }

    }
}
