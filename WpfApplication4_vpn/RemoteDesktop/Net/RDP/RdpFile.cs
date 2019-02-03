using System;
using System.IO;
using System.Text;

namespace RemoteDesktop.Net.RDP
{
    class RdpFile//https://www.donkz.nl/overview-rdp-file-settings/
    {
        private enum ConnectionType
        {
            /// <summary>Modem (56 Kbps).</summary>
            Modem = 1,
            /// <summary>Low-speed broadband (256 Kbps – 2 Mbps).</summary>
            LowSpeedBroadband,
            /// <summary>Satellite (2 Mbps – 16 Mbps with high latency).</summary>
            Satellite,
            /// <summary>High-speed broadband (2 Mbps – 10 Mbps).</summary>
            HighSpeedBroadband,
            /// <summary>WAN (10 Mbps or higher with high latency).</summary>
            WAN,
            /// <summary>LAN (10 Mbps or higher).</summary>
            LAN,
            /// <summary>Automatic bandwidth detection. Requires <see cref="bandwidthautodetect"/> .</summary>
            Automatic
        }

        /// <summary>
        /// Specifies the name or IP address (and optional port) of the remote computer that you want to connect to.
        /// </summary>
        public IPPort FullAddress { get; set; }
        /// <summary>
        /// Specifies the name of the user account that will be used to log on to the remote computer.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// The user password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Determines whether the connection bar appears when you are in full screen mode.
        /// </summary>
        private bool displayconnectionbar = true;
        /// <summary>
        /// Determines which local disk drives on the client computer will be redirected and available in the remote session.
        /// </summary>
        private bool drivestoredirect = true;//* – Redirect all disk drives
        /// <summary>
        /// Determines whether the client computer will automatically try to reconnect to the remote computer if the connection is dropped.
        /// </summary>
        private bool autoreconnection_enabled = true;
        /// <summary>
        /// Determines whether the clipboard on the client computer will be redirected and available in the remote session and vice versa.
        /// </summary>
        private bool redirectclipboard = true;
        /// <summary>
        /// Determines whether the remote session window appears full screen when you connect to the remote computer.
        /// </summary>
        private bool screen_mode_id = true;

        /// <summary>
        /// Creat remote desktop pulpit connection file.
        /// </summary>
        /// <param name="path">File path with name and extension.</param>
        public void Save(string path)
        {
            if (!path.EndsWith(".rdp") && !path.EndsWith(".RDP")) throw new FileFormatException("The file in the indicated path has no extension or the extension is incorrect!");
            if (string.IsNullOrEmpty(UserName)) throw new ArgumentNullException("The Username is empty!");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"full address:s:{FullAddress}");
            sb.AppendLine($"username:s:{UserName}");

            sb.AppendLine($"screen mode id:i:{(screen_mode_id ? "2" : "1")}");
            sb.AppendLine($"redirectclipboard:i:{(redirectclipboard ? "1" : "0")}");
            sb.AppendLine($"autoreconnection enabled:i:{(autoreconnection_enabled ? "1" : "0")}");
            sb.AppendLine($"displayconnectionbar:i:{(displayconnectionbar ? "1" : "0")}");
            sb.AppendLine($"drivestoredirect:s:{(drivestoredirect ? "*" :"" )}");

            sb.AppendLine("disable menu anims:i:1");
            sb.AppendLine("compression:i:1");
            sb.AppendLine($"connection type:i:{(int)ConnectionType.Automatic}");
            sb.AppendLine("networkautodetect:i:1");
            sb.AppendLine("bandwidthautodetect:i:1");
            sb.AppendLine("authentication level:i:2");

            if (!string.IsNullOrEmpty(Password))
            {
                sb.AppendLine("prompt for credentials:i:0");
                sb.AppendLine($"password 51:b:{Encoding.CryptRDP.Protect(Password)}");
            }
            else sb.AppendLine("prompt for credentials:i:1");

            StreamWriter sw = new StreamWriter(path, false);
            sw.Write(sb.ToString());
            sw.Close();
        }

        public override string ToString() => $"Address:{FullAddress};  User:{UserName}; Password:{Password}";
    }
}
