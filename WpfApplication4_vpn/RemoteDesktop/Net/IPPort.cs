using System;
using System.Text.RegularExpressions;

namespace RemoteDesktop.Net
{
    /// <summary>
    /// Provides an Internet Protocol (IPv4) address and port.
    /// </summary>
    public class IPPort: IEquatable<IPPort>//, IComparable, IComparable<IPPort>
    {
        public static readonly IPPort None = null;
        public static readonly IPPort MaxValue = new IPPort(new byte[] { 255, 255, 255, 255 }, 65535);
        public static readonly IPPort MinValue = new IPPort(new byte[] { 0, 0, 0, 0 }, 0);
        /// <summary>An Internet Protocol (IP) address</summary>
        public IP IP { get; set; }
        /// <summary>Port number</summary>
        public ushort Port { get; set; } = 0;

        /// <summary>
        /// Initializes a new instance of the IPPort class with the address specified as an <see cref="long"/>.
        /// </summary>
        /// <param name="newAddress">The long value of the IP address. For example, the value 0x2414188f in big-endian format would be the IP address "143.24.20.36".</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IPPort(long newAddress)//4294967295L
        {
            IP = new IP(newAddress);
        }

        /// <summary>
        /// Initializes a new instance of the IPPort class with the address specified as a <see cref="byte"/> array.
        /// </summary>
        /// <param name="address">The byte array value of the IP address.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public IPPort(byte[] address)
        {
            IP = new IP(address);
        }

        /// <summary>
        /// Initializes a new instance of the IPPort class with the address specified as a <see cref="byte"/> array and the specified port number.
        /// </summary>
        /// <param name="address">The <see cref="byte"/> array value of the IP address.</param>
        /// <param name="port">The <see cref="ushort"/> value of Port number.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public IPPort(byte[] address, ushort port)//65535
        {
            IP = new IP(address);
            Port = port;
        }

        /// <summary>
        /// Initializes a new instance of the IPPort class with the address specified as a <see cref="RemoteDesktop.Net.IP"/> .
        /// </summary>
        /// <param name="ip">The <see cref="RemoteDesktop.Net.IP"/> value of the IP address.</param>
        public IPPort(IP ip)
        {
            IP = ip;
        }

        /// <summary>
        /// Initializes a new instance of the IPPort class with the address specified as a <see cref="RemoteDesktop.Net.IP"/> and the specified port number.
        /// </summary>
        /// <param name="ip">The <see cref="RemoteDesktop.Net.IP"/> value of the IP address.</param>
        /// <param name="port">The <see cref="ushort"/> value of Port number.</param>
        public IPPort(IP ip, ushort port)
        {
            IP = ip;
            Port = port;
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

            string ValidPortRegex = @":\d{1,5}$";

            if (ipString.Contains(":"))
            {
                var qwe = ipString.Split(':');
                var ip = IP.Parse(qwe[0]);

                if (!Regex.IsMatch(ipString, ValidPortRegex))
                    throw new PortNumberExceptions($"Invalid Port number '{ipString.Remove(0, ipString.IndexOf(':'))}' !");
                return new IPPort(ip, ushort.Parse(qwe[1]));
            }
            else
            {
                return new IPPort(IP.Parse(ipString));
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
        /// Converts an Internet address to its standard notation.
        /// </summary>
        /// <param name="format"></param>
        /// <returns>A string that contains the IP address in either IPv4 notation.</returns>
        public string ToString(string format) => Port == 0 ? $"{IP.ToString(format)}" : $"{IP.ToString(format)}:{Port}";

        /// <summary>
        /// Get parameter format for Mstsc argument.
        /// </summary>
        public string Parameter => $"/v:{this.ToString()}";

    }
}
