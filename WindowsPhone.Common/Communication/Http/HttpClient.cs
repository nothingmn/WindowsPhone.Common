﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Diagnostics;
using WindowsPhone.Contracts.Communication.Http;
using WindowsPhone.Contracts.Logging;

namespace WindowsPhone.Common.Communication.Http
{
    public class HttpClient : IHttpClient
    {
        private ILog _log;
        public HttpClient(ILog log)
        {
            _log = log;
        }

        public static long Timeout = int.MaxValue;


        public event HttpDownloaded OnHttpDownloaded;
        public event HttpDownloadError OnHttpDownloadError;
        public event HttpDownloadProgress OnHttpDownloadProgress;
        public event HttpDownloadTimeout OnHttpDownloadTimeout;

        Stopwatch StopWatch { get; set; }

        public string Url { get; set; }

        private byte[] _PostData = null;
        private string Key;

        public void GET(string Url, string Key = null, System.Net.CookieContainer CookieContainer = null, string Username = null, string Password = null, string Domain = null, string Accept = null, Boolean AllowAutoRedirect = false, string ContentType = null, bool UseDefaultCredentials = false, string UserAgent = null, WebHeaderCollection Headers = null)
        {
            Download(Url, HttpMethods.GET, null, Key, CookieContainer, Username, Password, Domain, Accept, AllowAutoRedirect,
                     ContentType, UseDefaultCredentials, UserAgent, Headers);
        }


        public void HEAD(string Url, string Key = null, System.Net.CookieContainer CookieContainer = null, string Username = null, string Password = null, string Domain = null, string Accept = null, Boolean AllowAutoRedirect = false, string ContentType = null, bool UseDefaultCredentials = false, string UserAgent = null, WebHeaderCollection Headers = null)
        {
            Download(Url, HttpMethods.HEAD, null, Key, CookieContainer, Username, Password, Domain, Accept, AllowAutoRedirect,
                     ContentType, UseDefaultCredentials, UserAgent, Headers);
        }


        public void PUT(string Url, string Key = null, System.Net.CookieContainer CookieContainer = null, string Username = null, string Password = null, string Domain = null, string Accept = null, Boolean AllowAutoRedirect = false, string ContentType = null, bool UseDefaultCredentials = false, string UserAgent = null, WebHeaderCollection Headers = null)
        {
            Download(Url, HttpMethods.PUT, null, Key, CookieContainer, Username, Password, Domain, Accept, AllowAutoRedirect,
                     ContentType, UseDefaultCredentials, UserAgent, Headers);
        }


        public void DELETE(string Url, string Key = null, System.Net.CookieContainer CookieContainer = null, string Username = null, string Password = null, string Domain = null, string Accept = null, Boolean AllowAutoRedirect = false, string ContentType = null, bool UseDefaultCredentials = false, string UserAgent = null, WebHeaderCollection Headers = null)
        {
            Download(Url, HttpMethods.DELETE, null, Key, CookieContainer, Username, Password, Domain, Accept, AllowAutoRedirect,
                     ContentType, UseDefaultCredentials, UserAgent, Headers);
        }



        public void POST(string Url, byte[] PostData = null, string Key = null, System.Net.CookieContainer CookieContainer = null, string Username = null, string Password = null, string Domain = null, string Accept = null, Boolean AllowAutoRedirect = false, string ContentType = null, bool UseDefaultCredentials = false, string UserAgent = null, WebHeaderCollection Headers = null)
        {
            Download(Url, HttpMethods.POST, PostData, Key, CookieContainer, Username, Password, Domain, Accept, AllowAutoRedirect,
                     ContentType, UseDefaultCredentials, UserAgent, Headers);
        }
        public void OPTIONS(string Url, byte[] PostData = null, string Key = null, System.Net.CookieContainer CookieContainer = null, string Username = null, string Password = null, string Domain = null, string Accept = null, Boolean AllowAutoRedirect = false, string ContentType = null, bool UseDefaultCredentials = false, string UserAgent = null, WebHeaderCollection Headers = null)
        {
            Download(Url, HttpMethods.OPTIONS, PostData, Key, CookieContainer, Username, Password, Domain, Accept, AllowAutoRedirect,
                     ContentType, UseDefaultCredentials, UserAgent, Headers);
        }


        string ContentType = null;
        string Accept = null;
        bool AllowAutoRedirect = false;
        string UserAgent = null;
        private void Download(string Url, HttpMethods method = HttpMethods.GET, byte[] PostData = null, string Key = null, System.Net.CookieContainer CookieContainer = null, string Username = null, string Password = null, string Domain = null, string Accept = null, Boolean AllowAutoRedirect = false, string ContentType = null, bool UseDefaultCredentials = false, string UserAgent = null, WebHeaderCollection Headers = null)
        {
            try
            {
                _log.Info(string.Format("Url:{0}, Method:{1}, Key:{2}, Username:{3}, Password:{4}, Domain:{5}, Accept:{6}, AllowAutoRedirect:{7}, ContentType:{8}, UseDefaultCredentials:{9}, UserAgent:{10}", Url, method, Key, Username, Password, Domain, Accept, AllowAutoRedirect, ContentType, UseDefaultCredentials, UserAgent));
                StopWatch = new Stopwatch();
                StopWatch.Start();
                this.Key = Key;
                this.Url = Url;
                var request = (HttpWebRequest)WebRequest.Create(Url);

                if (Headers != null)
                {
                    foreach (var name in Headers.AllKeys)
                    {
                        try
                        {
                            request.Headers[name] = Headers[name];
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                if (CookieContainer != null) request.CookieContainer = CookieContainer;
                this.ContentType = ContentType;
                this.Accept = Accept;
                this.AllowAutoRedirect = AllowAutoRedirect;
                this.UserAgent = UserAgent;

                request.AllowAutoRedirect = AllowAutoRedirect;
                if (CookieContainer != null) request.CookieContainer = CookieContainer;
                if (Accept != null) request.Accept = Accept;
                if (ContentType != null) request.ContentType = ContentType;
                if (UserAgent != null) request.UserAgent = UserAgent;

                

                if (!string.IsNullOrEmpty(Domain) && !string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                {
                    request.Credentials = new System.Net.NetworkCredential(Username, Password, Domain);
                }
                else
                {
                    request.UseDefaultCredentials = UseDefaultCredentials;
                }
                request.Method = method.ToString();

                if (method == HttpMethods.POST || method == HttpMethods.OPTIONS)
                {
                    _PostData = PostData;
                    //for post, we typically are posting data, lets get the request stream

                    request.ContentType = ContentType;

                    if(string.IsNullOrEmpty(ContentType)) 
                        request.ContentType = "application/x-www-form-urlencoded";
                    
                    
                    request.BeginGetRequestStream(new AsyncCallback(BeginRequest), request);
                    HasElapsed();                    
                }
                else
                {
                    //for get's we just jump directly to the get respnose
                    request.BeginGetResponse(new AsyncCallback(HandleResponse), request);
                    HasElapsed();
                }

            }
            catch (Exception e)
            {

                if (OnHttpDownloadError != null) OnHttpDownloadError(this, e, FishOutBody(e), this.Key);
            }

        }

        private string FishOutBody(Exception e)
        {
            var body = "";
            try
            {
                using (StreamReader rdr = new StreamReader(((System.Net.WebException)(e.InnerException)).Response.GetResponseStream()))
                {
                    body = rdr.ReadToEnd();
                }
            }
            catch (Exception)
            {
            }
            return body;
        }
        private void BeginRequest(IAsyncResult request)
        {
            try
            {
                var req = ((HttpWebRequest)request.AsyncState);

                req.AllowAutoRedirect = AllowAutoRedirect;
                if (Accept != null) req.Accept = Accept;
                if (ContentType != null) req.ContentType = ContentType;
                if (UserAgent != null) req.UserAgent = UserAgent;
                
                HasElapsed();
                using (Stream requestStream = req.EndGetRequestStream(request))
                {
                    if (_PostData != null) requestStream.Write(_PostData, 0, _PostData.Length);
                    //write to the stream here...
                    HasElapsed();

                }
                req.BeginGetResponse(HandleResponse, req);
                HasElapsed();

            }
            catch (Exception e)
            {
                if (OnHttpDownloadError != null) OnHttpDownloadError(this, e,FishOutBody(e), this.Key);
            }

        }
        private void HasElapsed()
        {
            if (StopWatch.IsRunning && StopWatch.ElapsedMilliseconds > Timeout)
            {
                if (OnHttpDownloadTimeout != null) OnHttpDownloadTimeout(this, StopWatch.ElapsedMilliseconds, this.Key);
                throw new TimeoutException(string.Format("The excution has timedout.  Max:{0}", Timeout));
            }
        }

        private void HandleResponse(IAsyncResult result)
        {
            try
            {
                var request = (HttpWebRequest)result.AsyncState;
                HasElapsed();

                using (var response = (HttpWebResponse)request.EndGetResponse(result))
                {
                    HasElapsed();
                    using (var reader = new BinaryReader(response.GetResponseStream()))
                    {
                        HasElapsed();
                        StopWatch.Stop();
                        StopWatch.Reset();
                        StopWatch.Start();
                        var data = new List<byte>();
                        var readLength = 1024;
                        var buffer = new byte[readLength];
                        while (true)
                        {
                            var length = reader.Read(buffer, 0, readLength);
                            //only take the portion of the buffer that matters, the rest we can discard safely
                            for (int i = 0; i < length; i++)
                            {
                                data.Add(buffer[i]);
                            }
                            
                            if (OnHttpDownloadProgress != null) OnHttpDownloadProgress(this, response.ContentLength, (long)data.Count, (response.ContentLength <= 0 ? 0 : (((float)data.Count / (float)response.ContentLength) * 100)), StopWatch.ElapsedMilliseconds, this.Key);
                            if (length <= 0) break;

                        }
                        //time to signal
                        if (OnHttpDownloaded != null) OnHttpDownloaded(this, data.ToArray(), StopWatch.ElapsedMilliseconds, this.Key, response.Headers);
                    }
                }
            }
            catch (Exception e)
            {
                if (OnHttpDownloadError != null) OnHttpDownloadError(this, e,FishOutBody(e), this.Key);
            }

        }



        public Contracts.Communication.ICredential Credential { get; set; }
        public string ProxyAddress { get; set; }
        public int ProxyPort { get; set; }

        public void HTTPAsString(IHttpPayload Payload)
        {
            this.GET(Payload.Url, Payload.Key, Payload.Cookies, Credential.Username, Credential.Password, Credential.Domain);
        }
    }
}