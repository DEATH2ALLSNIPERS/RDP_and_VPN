using RemoteDesktop;
using RemoteDesktop.Button;
using RemoteDesktop.Net;
using RemoteDesktop.Net.RDP;
using System.Windows;

namespace WpfApplication4_vpn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ProcessWatcher mw;

        public MainWindow()
        {
            InitializeComponent();
            mw = new ProcessWatcher("mstsc");
            mw.ClosedProcess += Mw_ClosedProcess;
        }

        private void Mw_ClosedProcess(object sender, ProcessWatcherArgs e)
        {
            MessageBox.Show(e.ID + ":" + e.Button);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //var book = new RemoteDesktop.Net.VPN.RasphoneBook();
            //book.Open(book.RasphonePath);
            //var list = book.Entries;

            //var ras = new RemoteDesktop.Net.VPN.Rasdial();
            //var output = ras.IsConnectedVPN("TARAN");
            //ras.Connect("TARAN3", "jpietras", "Vfr4Mju73#");

            //RemoteDesktop.Net.VPN.Rasdial.LaunchVPN();

            var rdp = new Mstsc();
            rdp.ShowArgumentHelp();
            rdp.Connect("10.250.9.12");
            var mc = Mstsc.OpenedMstsc();

            RdpFile rf = new RdpFile()
            {
                FullAddress = IPPort.Parse("192.168.0.1"),
                UserName = "rgplus",
                Password = "GRPlus123#"
            };
            rf.Save("Test0.rdp");
            var upr = RemoteDesktop.Encoding.CryptRDP.Unprotect("01000000D08C9DDF0115D1118C7A00C04FC297EB01000000D929C49724C9FB41857598688D6890E60000000002000000000003660000C000000010000000028AEC0FBBCE9F9C0385F9E887AAD8140000000004800000A000000010000000A033394A302FBDF474F8F7D6F55C1071180000005FE6AC5ADA5AD53F718AACE44EF290A3E8CFE69471FA5E3414000000EED74EAD563C64DCF62784AE9312DDB6CD2C6343");

            //mw.FindNewProcess("Button");//dodanie do kontrolki przycisku ID procesu.

            RDP_Buttons but = new RDP_Buttons();
            but.Buttons.Add(new RDPbutton("Grupa", "caption", "hin", IPPort.Parse("192.168.0.154"), "/admin", new LoginRDP("jpietras", "RGPlus123#"), new LoginVPN("TARAN", "jpietras", "RGPlus123#")));
            but.Buttons.Add(new RDPbutton("grup", "cap", "hint", IPPort.Parse("192.168.34.154:80"), "", new LoginRDP("jpietras11", "RGPlus11"), new LoginVPN("Jastrzębie", "jpietras", "RGPlus123#")));
            but.Buttons.Add(new RDPbutton("grup", "cap", "hint", IPPort.Parse("192.168.34.154:80"), "", new LoginRDP("jpietras12", "RGPlus12"), new LoginVPN()));
            //but.Save();
        }

    }
}
