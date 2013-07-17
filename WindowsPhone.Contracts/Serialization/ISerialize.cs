namespace WindowsPhone.Contracts.Serialization
{
    public interface ISerialize
    {
        T Deserialize<T>(byte[] Data);
        T Deserialize<T>(string Data);
        string Serialize<T>(T Object);
    }
}
