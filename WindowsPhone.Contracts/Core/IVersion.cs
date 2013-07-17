namespace WindowsPhone.Contracts.Core
{
    public interface IVersion
    {
        int Major { get; set; }
        int Minor { get; set; }
        int Revision { get; set; }
        int Build { get; set; }
        string ToString();
        bool Equals(System.Object obj);
        bool Equals(IVersion p);
        int CompareTo(IVersion other);
        int CompareTo(object obj);


    }
}
