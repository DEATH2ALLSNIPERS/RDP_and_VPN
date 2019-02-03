using System;
using System.Diagnostics;
using System.Linq;

namespace RemoteDesktop.Net.VPN
{
    /// <summary>
    /// Creat Connection to definied VPN connection.
    /// </summary>
    public class Rasdial : AOpenCMD//"https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-xp/bb490979(v=technet.10)"
    {
        private const string rasdial = "rasdial.exe";

        public override string FileName => rasdial;

        private const string noConnections = "No connections";
        private const string commandSucces = "Command completed successfully";
        private const string succesConnect = "Successfully connected";
        private const string arledyConnect = "You are already connected";

        /// <summary>
        /// Initializes a new instance of the Rasdial class.
        /// </summary>
        public Rasdial(){}

        /// <summary>
        /// List of connected VPN connections.
        /// </summary>
        /// <returns>List of open VPN connection.</returns>
        /// <exception cref="NetExceptions"></exception>
        public string[] ConnectedVPN()
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = FileName;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            if (output.Contains(commandSucces))
            {
                if (output.Contains(noConnections)) throw new NetExceptions(noConnections);
                return output.Split((char)10).Reverse().Skip(2).Reverse().Skip(1).ToArray();//10 \n 0x0A
            }
            throw new NetExceptions($"'{p.StartInfo.FileName}' is not recognized as an internal or external command,\n operable program or batch file.");
        }

        /// <summary>
        /// If VPN is connected
        /// </summary>
        /// <param name="VPNName">VPN name</param>
        /// <returns>true if VPN is connected</returns>
        public bool IsConnectedVPN(string VPNName)
        {
            try
            {
                foreach (var item in ConnectedVPN())
                {
                    if (item.Equals(VPNName))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Connect to VPN
        /// </summary>
        /// <param name="Arguments">Options for connections</param>
        /// <returns>true if succesfull conection</returns>
        /// <exception cref="NetExceptions"></exception>
        public override bool Connect(string Arguments)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = Arguments.Contains(" *") ? false : true;//password
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = FileName;
            p.StartInfo.Arguments = Arguments;
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            if (output.Contains(commandSucces) && (output.Contains(succesConnect) || output.Contains(arledyConnect)))
            {
                Debug.WriteLine(output);
                return true;
                /*if (output.Contains(commandSucces))
                {
                    if (output.Contains(succesConnect)) return true;
                    if (output.Contains(arledyConnect)) return true;
                }*/
            }
            else
            {
                var errors = output.Split((char)10);
                foreach (var item in errors)
                {
                    if (item.Contains("error"))
                    {
                        throw new NetExceptions(item.Remove(0, item.IndexOf('-') + 1));
                    }
                }
                throw new NetExceptions("Another VPN connection error!");
                //Debug.WriteLine(output);
            }
            //System.Diagnostics.Process.Start(rasdial, $"{VPNName} {Username} {Password}");
        }

        /// <summary>
        /// Connect to VPN
        /// </summary>
        /// <param name="VPNName">VPN name</param>
        /// <param name="Username">User name</param>
        /// <param name="Password">Password</param>
        /// <returns>true if succesfull conection</returns>
        public bool Connect(string VPNName, string Username, string Password)
        {
            return Connect($"\"{VPNName}\" {Username} {Password}");
        }

        /// <summary>
        /// Connect to VPN using <see cref="LoginVPN"/>.
        /// </summary>
        /// <param name="vpn">Parameter needed for connect vpn.</param>
        /// <returns>true if succesfull conection</returns>
        public bool Connect(LoginVPN vpn)
        {
            return Connect($"\"{vpn.Name}\" {vpn.User} {vpn.Pass}");
        }

        /// <summary>
        /// Connect to VPN
        /// </summary>
        /// <param name="VPNName">VPN name</param>
        /// <param name="Arguments">Opctional argument for connecion</param>
        /// <returns>true if succesfull conection</returns>
        public override bool Connect(string VPNName, params string[] Arguments)//[/domain:domain*] [/phone:phonenumber] [/callback:callbacknumber] [/phonebook:phonebookpath] [/prefixsuffix**]
        {
            return Connect($"\"{VPNName}\" {string.Join(" ",Arguments)}");
        }

        /// <summary>
        /// Disconect VPN connection
        /// </summary>
        /// <param name="VPNName">VPN name</param>
        public void Disconnect(string VPNName)
        {
            //System.Diagnostics.Process.Start(rasdial, $"\"{VPNName}\" /disconnect");
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = FileName;
            p.StartInfo.Arguments = $"\"{VPNName}\" /disconnect";
            p.Start();
        }

        /// <summary>
        /// Launch Windows 10 VPN settings.
        /// </summary>
        public static void LaunchVPN()
        {
            if (Environment.OSVersion.Version.Build >= 9200)
            {
                Process.Start("ms-settings:network-vpn");//Win10
            }
            else
            {
                Process.Start("NCPA.cpl");
            }
        }

        public override bool Connect(params string[] arguments)
        {
            throw new NotImplementedException();
        }
    }
}
