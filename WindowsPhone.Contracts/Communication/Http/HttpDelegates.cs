using System.Net;

namespace WindowsPhone.Contracts.Communication.Http
{
    public delegate void HttpDownloaded(object Sender, byte[] Data, long Duration, string Key, WebHeaderCollection Headers = null );
    public delegate void HttpDownloadError(object Sender, System.Exception exception, string body, string Key);
    public delegate void HttpDownloadProgress(object Sender, long ContentLength, long BytesSoFar, double Progress, long Duration, string Key);
    public delegate void HttpDownloadTimeout(object Sender, long Duration, string Key);
}
