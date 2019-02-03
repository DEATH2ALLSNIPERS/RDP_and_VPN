using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace RemoteDesktop.Net.VPN
{
    /// <summary>
    /// Information of defined VPN connection.
    /// </summary>
    public struct Rasphone
    {
        /// <summary>VPN connection name.</summary>
        public string Name { get; private set; }
        /// <summary>VPN connection IP Address.</summary>
        public IPPort IP { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Rasphone struct with name and IP Address.
        /// </summary>
        /// <param name="name">VPN connection name</param>
        /// <param name="iP">VPN connection IP Address</param>
        public Rasphone(string name, IPPort iP)
        {
            Name = name;
            IP = iP;
        }

        public override string ToString()
        {
            return $"Name:{Name}; IP:{IP}";
        }
    }

    /// <summary>
    /// Provides list of specify the information that the Remote Access Connection Manager needs to establish a remote connection.
    /// </summary>
    class RasphoneBook
    {
        //private const string rasphone = "rasphone.pbk";//C:\Users\jpietras\AppData\Roaming\Microsoft\Network\Connections\Pbk
        /// <summary>
        /// Rasphone file path
        /// </summary>
        public string RasphonePath { get; } = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) +
                          @"\Microsoft\Network\Connections\Pbk\rasphone.pbk";

        /// <summary>
        /// List of all defined VPN connections.
        /// </summary>
        public Rasphone[] Entries { get; private set; }

        /// <summary>
        /// Load all defined VPNs.
        /// </summary>
        /// <param name="path">rasphone.pbk file path</param>
        public void Open(string path)
        {
            List<Rasphone> lt = new List<Rasphone>();
            const string pattern = @"\[(.*?)\]";

            //var matches = Regex.Matches(System.IO.File.ReadAllText(path), pattern);//only name

            using (StreamReader sr = File.OpenText(path))
            {
                string tmp_name = null, tmp_ip = null;
                string s = String.Empty;
                while ((s = sr.ReadLine()) != null)
                {
                    if (Regex.IsMatch(s, pattern))
                    {
                        tmp_name = s.Replace("[", "").Replace("]", "");
                    }
                    if (s.IndexOf("PhoneNumber=")==0)
                    {
                        tmp_ip = s.Remove(0, 12);//s.IndexOf('=') + 1
                    }
                    if (!string.IsNullOrEmpty(tmp_name) && !string.IsNullOrEmpty(tmp_ip))
                    {
                        lt.Add(new Rasphone(tmp_name, IPPort.Parse(tmp_ip)));
                        tmp_name = tmp_ip = null;
                    }
                }
            }
            Entries = lt.ToArray();
        }

        public override string ToString()
        {
            return $"VPN connection Count:{Entries.Length}";
        }
    }
}
