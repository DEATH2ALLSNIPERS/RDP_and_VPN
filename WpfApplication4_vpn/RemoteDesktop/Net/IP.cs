using System;
using System.Text.RegularExpressions;

namespace RemoteDesktop.Net
{
    /// <summary>
    /// Provides an Internet Protocol (IPv4) address.
    /// </summary>
    public struct IP : IEquatable<IP>, IComparable<IP>, ICloneable
    {
        //public static readonly IP MaxValue = new IP(new byte[] { 255, 255, 255, 255 });
        //public static readonly IP MinValue = new IP(new byte[] { 0, 0, 0, 0 });

        public byte Octet1;
        public byte Octet2;
        public byte Octet3;
        public byte Octet4;

        /// <summary>
        /// Initializes a new instance of the <see cref="IP"/> struct with the address specified as a <see cref="byte"/> array.
        /// </summary>
        /// <param name="ip">The byte array value of the IP address.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public IP(byte[] ip)
        {
            if (ip == null) throw new ArgumentNullException("The IP adress is null!");
            if (ip.Length < 4) throw new ArgumentException("The IP address does not contain all octets!");

            Octet1 = ip[0];
            Octet2 = ip[1];
            Octet3 = ip[2];
            Octet4 = ip[3];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IP"/> struct with the address specified as a <see cref="byte"/>.
        /// </summary>
        /// <param name="octet1">First part of address</param>
        /// <param name="octet2">Second par of address</param>
        /// <param name="octet3">Third par of address</param>
        /// <param name="octet4">Fourth par of address</param>
        public IP(byte octet1, byte octet2, byte octet3, byte octet4)
        {
            Octet1 = octet1;
            Octet2 = octet2;
            Octet3 = octet3;
            Octet4 = octet4;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IP"/> struct with the address specified as an <see cref="long"/>.
        /// </summary>
        /// <param name="address">The long value of the IP address. For example, the value 0x2414188f in big-endian format would be the IP address "143.24.20.36".</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IP(long address)
        {
            if (address < 0 || address > 0x00000000FFFFFFFF) throw new ArgumentOutOfRangeException($"'{address}' is out of range of a IP address.");

            Octet1 = Octet2 = Octet3 = Octet4 = 0;

            var ip = LongToIP(address);
            Octet1 = ip[0];
            Octet2 = ip[1];
            Octet3 = ip[2];
            Octet4 = ip[3];
        }       

        public int CompareTo(IP other)
        {
            if (IP2Long(this.ToByteArray()) > IP2Long(other.ToByteArray())) return 1;
            else if (IP2Long(this.ToByteArray()) < IP2Long(other.ToByteArray())) return -1;
            else return 0;
        }

        /// <summary>
        /// Compares two IP addresses.
        /// </summary>
        /// <param name="obj">An <see cref="IP"/> instance to compare to the current instance.</param>
        /// <returns>true if the two addresses are equal; otherwise, false.</returns>
        public bool Equals(IP other)
        {
            if (Octet1 == other.Octet1 && Octet2 == other.Octet2 &&
                Octet3 == other.Octet3 && Octet4 == other.Octet4) return true;
            return false;
        }

        public override string ToString()
        {
            return Octet1.ToString("D1") + '.' + Octet2.ToString("D1") + '.' +
                    Octet3.ToString("D1") + '.' + Octet4.ToString("D1");
        }

        public string ToString(string format)
        {
            return Octet1.ToString(format) + '.' + Octet2.ToString(format) + '.' +
                    Octet3.ToString(format) + '.' + Octet4.ToString(format);
        }

        /// <summary>
        /// Converts an IP address string to an <see cref="IP"/> instance.
        /// </summary>
        /// <param name="ipString">A string that contains an IP address in dotted-quad notation for IPv4 notation.</param>
        /// <returns>An <see cref="IP"/> instance.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="IPAdressExceptions"></exception>
        /// <exception cref="FormatException"></exception>
        public static IP Parse(string ipString)
        {
            if (string.IsNullOrEmpty(ipString)) throw new ArgumentNullException("IP Address is null!");

            string ValidIpAddressRegex = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])";

            if (Regex.IsMatch(ipString, ValidIpAddressRegex + "$"))
            {
                var qw = ipString.Split('.');
                byte[] ad = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    ad[i] = byte.Parse(qw[i]);
                }
                return new IP(ad);
            }
            else throw new IPAdressExceptions($"Invalid IP Address '{ipString}' !");
        }

        /// <summary>
        /// Determines whether a string is a valid IP address.
        /// </summary>
        /// <param name="ipString">The string to validate.</param>
        /// <param name="address">The <see cref="IP"/> version of the string.</param>
        /// <returns>true if ipString was able to be parsed as an IP address; otherwise, false.</returns>
        public static bool TryParse(string ipString, out IP address)
        {
            try
            {
                address = Parse(ipString);
                return true;
            }
            catch (Exception)
            {
                address = new IP();
                return false;
            }
        }

        public byte[] ToByteArray() => new byte[] { Octet1, Octet2, Octet3, Octet4 };

        public byte this[int index] => this.ToByteArray()[index];

        //public static bool IsNull(IP value) => ;

        private byte[] LongToIP(long longIP)
        {
            byte[] ip = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                int num = (int)(longIP / Math.Pow(256, (3 - i)));
                longIP = longIP - (long)(num * Math.Pow(256, (3 - i)));
                ip[i] = (byte)num;
            }
            return ip;
        }

        private long IP2Long(byte[] ipBytes)
        {
            double num = 0;
            if (ipBytes.Length == 4)
            {
                for (int i = ipBytes.Length - 1; i >= 0; i--)
                {
                    num += ((ipBytes[i] % 256) * Math.Pow(256, (3 - i)));
                }
            }
            return (long)num;
        }

        private long IP2Long(string ip)
        {
            string[] ipBytes;
            double num = 0;
            if (!string.IsNullOrEmpty(ip))
            {
                ipBytes = ip.Split('.');
                for (int i = ipBytes.Length - 1; i >= 0; i--)
                {
                    num += ((int.Parse(ipBytes[i]) % 256) * Math.Pow(256, (3 - i)));
                }
            }
            return (long)num;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override int GetHashCode()
        {
            var hashCode = -908088530;
            hashCode = hashCode * -1521134295 + Octet1.GetHashCode();
            hashCode = hashCode * -1521134295 + Octet2.GetHashCode();
            hashCode = hashCode * -1521134295 + Octet3.GetHashCode();
            hashCode = hashCode * -1521134295 + Octet4.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public static bool operator ==(IP a, IP b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(IP a, IP b)
        {
            return !a.Equals(b);
        }

    }
}
