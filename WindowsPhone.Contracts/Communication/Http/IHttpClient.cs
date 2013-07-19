namespace WindowsPhone.Contracts.Communication.Http
{
    public interface IHttpClient
    {
        ICredential Credential { get; set; }
        string ProxyAddress { get; set; }
        int ProxyPort { get; set; }
        void HTTPAsString(IHttpPayload Payload);
        event HttpDownloaded OnHttpDownloaded;
        event HttpDownloadError OnHttpDownloadError;
        event HttpDownloadProgress OnHttpDownloadProgress;
        event HttpDownloadTimeout OnHttpDownloadTimeout;

    }
}