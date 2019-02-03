using System.Collections.Generic;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using RemoteDesktop.Encoding;
using RemoteDesktop.Net;

namespace RemoteDesktop.Button
{
    /// <summary>
    /// Storing information for XML button.
    /// </summary>
    public struct RDPbutton
    {
        /// <summary>Group name</summary>
        public string group;
        /// <summary>Caption name</summary>
        public string caption;
        /// <summary>Hint name</summary>
        public string hint;
        /// <summary>IP address and port number for RDP</summary>
        public IPPort ipport;
        /// <summary>Additional parameters</summary>
        public string parameters;
        /// <summary>User name and password dor RDP</summary>
        public LoginRDP rdplogin;
        /// <summary>Connection name, User name and password for VPN</summary>
        public LoginVPN vpnlogin;

        public RDPbutton(string group, string caption, string hint, IPPort ipport, string parameters, LoginRDP rdplogin, LoginVPN vpnlogin)
        {
            this.group = group;
            this.caption = caption;
            this.hint = hint;
            this.ipport = ipport;
            this.parameters = parameters;
            this.rdplogin = rdplogin;
            this.vpnlogin = vpnlogin;
        }

        public override string ToString()
        {
            return $"{group}-{caption} {ipport}";
        }
    }

    /// <summary>
    /// Manage buttons.
    /// </summary>
    class RDP_Buttons
    {
        /// <summary>
        /// List of buttons information.
        /// </summary>
        public List<RDPbutton> Buttons { get; set; }

        /// <summary>
        /// Initializes a new instance of the RDP_Buttons class and load buttons from file.
        /// </summary>
        public RDP_Buttons()
        {
            Buttons = new List<Button.RDPbutton>();
            if (File.Exists("rdp_buttons.xml"))
            {
                XElement rdp = XElement.Load("rdp_buttons.xml");

                foreach (var item in rdp.Elements())//<Button>
                {
                    Buttons.Add(new RDPbutton()
                    {
                        group = item.Element("group").Value,
                        caption = item.Element("caption").Value,
                        hint = item.Element("hint").Value,
                        ipport = IPPort.Parse(item.Element("ipport").Value),
                        parameters = item.Element("parameters").Value,
                        rdplogin = new LoginRDP(item.Element("rdplogin").Element("user").Value,
                                                Base64.DecodeBase64(item.Element("rdplogin").Element("pass").Value)),
                        vpnlogin = new LoginVPN(item.Element("vpnlogin").Element("name").Value,
                                                item.Element("vpnlogin").Element("user").Value,
                                                Base64.DecodeBase64(item.Element("vpnlogin").Element("pass").Value))
                    });

                }
            }
            else Debug.WriteLine("Not found file 'rdp_buttons.xml'");                 
        }

        /// <summary>
        /// Save list of buttons.
        /// </summary>
        public void Save()
        {
            List<XElement> xel = new List<XElement>();
            foreach (var item in Buttons)
            {
                xel.Add(
                    new XElement("Button",
                        new XElement("group", item.group),
                        new XElement("caption", item.caption),
                        new XElement("hint", item.hint),
                        new XElement("ipport", item.ipport),
                        new XElement("parameters", item.parameters),
                        new XElement("rdplogin",
                            new XElement("user", item.rdplogin.User),
                            new XElement("pass", Base64.EncodeBase64(item.rdplogin.Pass))
                        ),
                        new XElement("vpnlogin",
                            new XElement("name", item.vpnlogin.Name),
                            new XElement("user", item.vpnlogin.User),
                            new XElement("pass", Base64.EncodeBase64(item.vpnlogin.Pass))
                        )
                    )
                );
            }
            XElement rdpbuttons = new XElement("RDP_Buttons", xel);//root

            rdpbuttons.Save("rdp_buttons.xml");
        }
    }
}
