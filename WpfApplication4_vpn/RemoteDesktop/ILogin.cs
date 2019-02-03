namespace RemoteDesktop
{
    public interface ILogin
    {
        string Pass { get; }
        string User { get; }

        string ToString();
    }
}