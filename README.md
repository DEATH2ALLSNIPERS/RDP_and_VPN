# RDP & VPN
Class to use remote desktop pulpit and vpn connection.

````csharp
using  RemoteDesktop.Net.VPN;

    //List of all defined VPN connections.
    var book = new RasphoneBook();
    book.Open(book.RasphonePath);
    var list = book.Entries;
````
````csharp
using  RemoteDesktop.Net.VPN;

    var ras = new Rasdial();
    //Check if VPN is connected
    var output = ras.IsConnectedVPN("VPN_Name");
    //Connect to defined VPN
    ras.Connect("VPN_Name", "login", "password");
    //Launch VPN windows option
    Rasdial.LaunchVPN();
````
````csharp
using  RemoteDesktop.Net.RDP;

    //
    var rdp = new Mstsc();
    rdp.ShowArgumentHelp();

    rdp.ConnectRDP("10.250.9.12");
    var mc = Mstsc.OpenedMstsc();
````
````csharp
    //
    RdpFile rf = new RdpFile()
    {
        FullAddress = IPPort.Parse("192.168.0.1"),
        UserName = "rgplus",
        Password = "GRPlus123#"
    };
    rf.Save("Test0.rdp");
    var upr = RemoteDesktop.Encoding.CryptRDP.Unprotect("01000000D08C9DDF0115...");
````

````csharp
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

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        mw.FindNewProcess("Button");//dodanie do kontrolki przycisku ID procesu.
    }
}
````

````csharp
    RDP_Buttons but = new RDP_Buttons();

    but.Buttons.Add(new RDPbutton("Grupa", "caption", "hin", IPPort.Parse("192.168.0.154"), "/admin", new LoginRDP("jpietras", "RGPlus123#"), new LoginVPN("TARAN", "jpietras", "RGPlus123#")));

    but.Buttons.Add(new RDPbutton("grup", "cap", "hint", IPPort.Parse("192.168.34.154:80"), "", new LoginRDP("jpietras11", "RGPlus11"), new LoginVPN("Jastrzębie", "jpietras", "RGPlus123#")));

    but.Buttons.Add(new RDPbutton("grup", "cap", "hint", IPPort.Parse("192.168.34.154:80"), "", new LoginRDP("jpietras12", "RGPlus12"), new LoginVPN()));

    but.Save();
````