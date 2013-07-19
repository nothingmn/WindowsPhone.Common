using System.Net;

namespace WindowsPhone.Contracts.Communication.Http
{
    public interface IHttpPayload
    {
        string Key { get; set; }
        string Url { get; set; }
        bool POST { get; set; }
        byte[] Data { get; set; }
        WebHeaderCollection Headers { get; set; }
        string RawData { get; set; }
        System.Net.CookieContainer Cookies { get; set; }
    }
}