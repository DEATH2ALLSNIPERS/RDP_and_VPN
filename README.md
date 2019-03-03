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

    //Creat instatnce
    var rdp = new Mstsc();
    //Schow argumnet help in console
    rdp.ShowArgumentHelp();

    //Connect to ip address
    rdp.ConnectRDP("192.168.0.1");
    //list opened remote desktop app.
    var mc = Mstsc.OpenedMstsc();
````
````csharp
    //Creat rdp file width parameter
    RdpFile rf = new RdpFile()
    {
        FullAddress = IPPort.Parse("192.168.0.1"),
        UserName = "username",
        Password = "password"
    };
    rf.Save("Test0.rdp");
    
    //encoding password from rdp file
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

    but.Buttons.Add(new RDPbutton("Grupa", "caption", "hin", IPPort.Parse("192.168.0.1"), "/admin", new LoginRDP("username", "password"), new LoginVPN("VPN_Name", "login", "password")));

    but.Buttons.Add(new RDPbutton("grup", "cap", "hint", IPPort.Parse("192.168.0.2"), "", new LoginRDP("username", "password"), new LoginVPN("VPN_Name", "login", "password")));

    but.Buttons.Add(new RDPbutton("grup", "cap", "hint", IPPort.Parse("192.168.1.3"), "", new LoginRDP("username", "password"), new LoginVPN()));

    but.Save();
````
