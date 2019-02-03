using System;

namespace RemoteDesktop
{
    /// <summary>
    /// Storing information for RDP logging.
    /// </summary>
    public struct LoginRDP : ILogin
    {
        public static readonly LoginRDP None;

        /// <summary>User name</summary>
        public string User { get; private set; }
        /// <summary>User password</summary>
        public string Pass { get; private set; }

        static LoginRDP()
        {
            None = new LoginRDP();
        }

        /// <summary>
        /// Initializes a new instance of the Login struct with the user name and password specified as an <see cref="string"/>.
        /// </summary>
        /// <param name="user">User name</param>
        /// <param name="pass">User password</param>
        public LoginRDP(string user, string pass)
        {
            this.User = user;
            this.Pass = pass;
        }

        public override string ToString() => $"User:{User}; Password:{Pass}";
    }

    /// <summary>
    /// Storing information for VPN logging.
    /// </summary>
    public struct LoginVPN : ILogin
    {
        public static readonly LoginVPN None;

        /// <summary>Connection name</summary>
        public string Name { get; private set; }
        /// <summary>User name</summary>
        public string User { get; private set; }
        /// <summary>User password</summary>
        public string Pass { get; private set; }

        static LoginVPN()
        {
            None = new LoginVPN();
        }

        /// <summary>
        /// Initializes a new instance of the Login struct with the user name and password specified as an <see cref="string"/>.
        /// </summary>
        /// <param name="name">Login name</param>
        /// <param name="user">User name</param>
        /// <param name="pass">User password</param>
        public LoginVPN(string name, string user, string pass)
        {
            this.Name = name;
            this.User = user;
            this.Pass = pass;
        }

        public override string ToString() => $"Name:{Name}; User:{User}; Password:{Pass}";
    }

}
