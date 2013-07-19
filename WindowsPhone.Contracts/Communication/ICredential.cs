namespace WindowsPhone.Contracts.Communication
{
    public interface ICredential
    {
        bool Default { get; set; }
        string Name { get; set; }
        string Host { get; set; }
        int Port { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string Domain { get; set; }
        System.Uri Uri { get; }
        string ToString();
    }
}