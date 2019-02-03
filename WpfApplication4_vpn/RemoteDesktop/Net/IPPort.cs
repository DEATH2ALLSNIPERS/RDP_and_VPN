using System;
using System.Text.RegularExpressions;

namespace RemoteDesktop.Net
{
    /// <summary>
    /// Provides an Internet Protocol (IP) address and port.
    /// </summary>
    public class IPPort
    {
        public static readonly IPPort None = null;
        public static readonly IPPort MaxValue = new IPPort(new byte[] { 255, 255, 255, 255 }, 65535);
        public static readonly IPPort MinValue = new IPPort(new byte[] { 0, 0, 0, 0 }, 0);
        /// <summary>An Internet Protocol (IP) address</summary>
        public string IP { get; private set; } = "0.0.0.0";
        /// <summary>Port number</summary>
        public ushort Port { get; private set; } = 0;

        /// <summary>
        /// Initializes a new instance of the IPPort class with the address specified as an <see cref="long"/>.
        /// </summary>
        /// <param name="newAddress">The long value of the IP address. For example, the value 0x2414188f in big-endian format would be the IP address "143.24.20.36".</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IPPort(long newAddress)//4294967295L
        {
            if (newAddress < 0 || newAddress > 0x00000000FFFFFFFF) throw new ArgumentOutOfRangeException($"'{newAddress}' is out of range of a IP address.");
            IP = LongToIP(newAddress);//37831048
        }

        /// <summary>
        /// Initializes a new instance of the IPPort class with the address specified as a <see cref="byte"/> array.
        /// </summary>
        /// <param name="address">The byte array value of the IP address.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public IPPort(byte[] address)
        {
            Pull(address, 0);
        }

        /// <summary>
        /// Initializes a new instance of the IPPort class with the address specified as a <see cref="byte"/> array and the specified port number.
        /// </summary>
        /// <param name="address">The <see cref="byte"/> array value of the IP address.</param>
        /// <param name="port">The <see cref="ushort"/> value of Port number.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public IPPort(byte[] address, ushort port)//65535
        {
            Pull(address, port);
        }

        /// <summary>
        /// Initializes a new instance of the IPPort class with the address specified as a <see cref="byte"/> array and the specified port number.
        /// </summary>
        /// <param name="address">The <see cref="byte"/> array value of the IP address.</param>
        /// <param name="port">The <see cref="ushort"/> value of Port number.</param>
        /// <param name="zero">Adding the leading zeros for the individual quartets of the IP address.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public IPPort(byte[] address, ushort port, bool zero)
        {
            Pull(address, port, zero);
        }

        /// <summary>
        /// Converts an IP address string to an <see cref="IPPort"/> instance.
        /// </summary>
        /// <param name="ipString">A string that contains an IP address in dotted-quad notation for IPv4 notation.</param>
        /// <returns>An <see cref="IPPort"/> instance.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IPAdressExceptions"></exception>
        /// <exception cref="PortNumberExceptions"></exception>
        /// <exception cref="FormatException"></exception>
        public static IPPort Parse(string ipString)
        {
            if (string.IsNullOrEmpty(ipString)) throw new ArgumentNullException("IP Address is null!");

            string ValidIpAddressRegex = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])";
            string ValidPortRegex = @":\d{1,5}$";

            if (ipString.Contains(":"))
            {
                if (!Regex.IsMatch(ipString, ValidIpAddressRegex))
                    throw new IPAdressExceptions($"Invalid IP Address '{ipString}' !");
                if (!Regex.IsMatch(ipString, ValidPortRegex))
                    throw new PortNumberExceptions($"Invalid Port number '{ipString.Remove(0, ipString.IndexOf(':'))}' !");
                var qw = ipString.Split('.', ':');
                byte[] ad = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    ad[i] = byte.Parse(qw[i]);
                }
                return new IPPort(ad, ushort.Parse(qw[4]));
            }
            else
            {
                if (Regex.IsMatch(ipString, ValidIpAddressRegex + "$"))
                {
                    var qw = ipString.Split('.');
                    byte[] ad = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        ad[i] = byte.Parse(qw[i]);
                    }
                    return new IPPort(ad);
                }
                else throw new IPAdressExceptions($"Invalid IP Address '{ipString}' !");
            }
        }

        /// <summary>
        /// Determines whether a string is a valid IP address.
        /// </summary>
        /// <param name="ipString">The string to validate.</param>
        /// <param name="address">The <see cref="IPPort"/> version of the string.</param>
        /// <returns>true if ipString was able to be parsed as an IP address; otherwise, false.</returns>
        public static bool TryParse(string ipString, out IPPort address)
        {
            try
            {
                address = Parse(ipString);
                return true;
            }
            catch (Exception)
            {
                address = None;
                return false;
            }
        }

        /// <summary>
        /// Compares two IP addresses and port.
        /// </summary>
        /// <param name="obj">An instance to compare to the current instance.</param>
        /// <returns>true if the two addresses are equal; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(IPPort))
            {
                Equals((IPPort)obj);
            }
            return false;
        }

        /// <summary>
        /// Compares two IP addresses and port.
        /// </summary>
        /// <param name="obj">An <see cref="IPPort"/> instance to compare to the current instance.</param>
        /// <returns>true if the two addresses are equal; otherwise, false.</returns>
        public bool Equals(IPPort obj)
        {
            if (obj.IP == IP && obj.Port == Port)
            {
                return true;
            }
            else return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Converts an Internet address to its standard notation.
        /// </summary>
        /// <returns>A string that contains the IP address in either IPv4 notation.</returns>
        public override string ToString() => Port == 0 ? $"{IP}" : $"{IP}:{Port}";

        /// <summary>
        /// Get parameter format for Mstsc argument.
        /// </summary>
        public string Parameter => $"/v:{this.ToString()}";

        /// <summary>
        /// Provides a copy of the <see cref="IPPort"/> as an array of bytes.
        /// </summary>
        /// <returns>A <see cref="byte"/> array.</returns>
        public byte[] GetAddressBytes()
        {
            var val = IP.Split('.');
            byte[] bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = byte.Parse(val[i]);
            }
            return bytes;
        }

        private void Pull(byte[] address, ushort port, bool zero = false)
        {
            if (address == null) throw new ArgumentNullException("The IP adress is null!");
            if (address.Length < 4) throw new ArgumentException("The IP address does not contain all octets!");
            if (zero)
            {
                IP = address[0].ToString("D3") + '.' + address[1].ToString("D3") + '.' +
                    address[2].ToString("D3") + '.' + address[3].ToString("D3");
                Port = port;
            }
            else
            {
                IP = address[0].ToString() + '.' + address[1].ToString() + '.' +
                    address[2].ToString() + '.' + address[3].ToString();
                Port = port;
            }
        }

        private string LongToIP(long longIP)
        {
            string ip = string.Empty;
            for (int i = 0; i< 4; i++)
            {
                int num = (int)(longIP / Math.Pow(256, (3 - i)));
                longIP = longIP - (long)(num* Math.Pow(256, (3 - i)));
                if (i == 0)
                    ip = num.ToString();
                else
                ip  = ip + "." + num.ToString();
            }
            return ip;
        }

        private long IP2Long(string ip)
        {
            string[] ipBytes;
            double num = 0;
            if(!string.IsNullOrEmpty(ip))
            {
                ipBytes = ip.Split('.');
                for (int i = ipBytes.Length - 1; i >= 0; i--)
                {
                    num += ((int.Parse(ipBytes[i]) % 256) * Math.Pow(256, (3 - i)));
                }
            }
            return (long)num;
        }

    }
}
