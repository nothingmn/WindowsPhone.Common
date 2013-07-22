using System;
using System.Net;

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


        void GET(string Url, string Key = null, System.Net.CookieContainer CookieContainer = null,
                 string Username = null, string Password = null, string Domain = null, string Accept = null,
                 Boolean AllowAutoRedirect = false, string ContentType = null, bool UseDefaultCredentials = false,
                 string UserAgent = null, WebHeaderCollection Headers = null);

        void HEAD(string Url, string Key = null, System.Net.CookieContainer CookieContainer = null,
                  string Username = null, string Password = null, string Domain = null, string Accept = null,
                  Boolean AllowAutoRedirect = false, string ContentType = null, bool UseDefaultCredentials = false,
                  string UserAgent = null, WebHeaderCollection Headers = null);

        void PUT(string Url, string Key = null, System.Net.CookieContainer CookieContainer = null,
                 string Username = null, string Password = null, string Domain = null, string Accept = null,
                 Boolean AllowAutoRedirect = false, string ContentType = null, bool UseDefaultCredentials = false,
                 string UserAgent = null, WebHeaderCollection Headers = null);

        void DELETE(string Url, string Key = null, System.Net.CookieContainer CookieContainer = null,
                    string Username = null, string Password = null, string Domain = null, string Accept = null,
                    Boolean AllowAutoRedirect = false, string ContentType = null, bool UseDefaultCredentials = false,
                    string UserAgent = null, WebHeaderCollection Headers = null);


        void POST(string Url, byte[] PostData = null, string Key = null,
                  System.Net.CookieContainer CookieContainer = null, string Username = null, string Password = null,
                  string Domain = null, string Accept = null, Boolean AllowAutoRedirect = false,
                  string ContentType = null, bool UseDefaultCredentials = false, string UserAgent = null,
                  WebHeaderCollection Headers = null);

        void OPTIONS(string Url, byte[] PostData = null, string Key = null,
                     System.Net.CookieContainer CookieContainer = null, string Username = null, string Password = null,
                     string Domain = null, string Accept = null, Boolean AllowAutoRedirect = false,
                     string ContentType = null, bool UseDefaultCredentials = false, string UserAgent = null,
                     WebHeaderCollection Headers = null);

    }
}