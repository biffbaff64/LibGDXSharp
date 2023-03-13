using LibGDXSharp.Network;

namespace LibGDXSharp.Core
{
    public interface INet
    {
        public interface IHttpResponse
        {
            byte[] GetResult();

            string GetResultAsString();

            StreamReader GetResultAsStream();

            HttpStatus GetStatus();

            string GetHeader( string name );

            Dictionary< string, List< string > > GetHeaders();
        }

        /// <summary>
        /// 
        /// </summary>
        public interface IHttpMethods
        {
            public const string Head   = "HEAD";
            public const string Get    = "GET";
            public const string Post   = "POST";
            public const string Put    = "PUT";
            public const string Patch  = "PATCH";
            public const string Delete = "DELETE";
        }

        /// <summary>
        /// 
        /// </summary>
        public class HttpRequest : IPoolable
        {
            private          string?                       _httpMethod;
            private          string?                       _url;
            private readonly Dictionary< string, string >? _headers;
            private          int                           _timeOut = 0;

            private string?       _content;
            private StreamReader? _contentStream;
            private long          _contentLength;

            private bool _followRedirects    = true;
            private bool _includeCredentials = false;

            public HttpRequest()
            {
                this._headers = new Dictionary< string, string >();
            }

            public HttpRequest( string httpMethod ) : this()
            {
                this._httpMethod = httpMethod;
            }

            public void SetUrl( string url )
            {
                this._url = url;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            public void SetHeader( string name, string value )
            {
                if ( _headers != null )
                {
                    _headers[ name ] = value;
                }
            }

            public void SetContent( string content )
            {
                this._content = content;
            }

            public void SetContent( StreamReader contentStream, long contentLength )
            {
                this._contentStream = contentStream;
                this._contentLength = contentLength;
            }

            public void SetTimeOut( int timeOut )
            {
                this._timeOut = timeOut;
            }

            public void SetFollowRedirects( bool followRedirects )
            {
                if ( followRedirects || Gdx.App?.Type != Application.ApplicationType.WebGL )
                {
                    this._followRedirects = followRedirects;
                }
                else
                {
                    throw new ArgumentException
                        ( "Following redirects can't be disabled using the GWT/WebGL backend!" );
                }
            }

            public void SetIncludeCredentials( bool includeCredentials )
            {
                this._includeCredentials = includeCredentials;
            }

            public void SetMethod( string httpMethod )
            {
                this._httpMethod = httpMethod;
            }

            public int GetTimeOut()
            {
                return _timeOut;
            }

            public string? GetMethod()
            {
                return _httpMethod;
            }

            public string? GetUrl()
            {
                return _url;
            }

            public string? GetContent()
            {
                return _content;
            }

            public StreamReader? GetContentStream()
            {
                return _contentStream;
            }

            public long GetContentLength()
            {
                return _contentLength;
            }

            public Dictionary< string, string >? GetHeaders()
            {
                return _headers;
            }

            public bool GetFollowRedirects()
            {
                return _followRedirects;
            }

            public bool GetIncludeCredentials()
            {
                return _includeCredentials;
            }

            public void Reset()
            {
                _httpMethod = null;
                _url        = null;
                _headers?.Clear();
                _timeOut = 0;

                _content       = null;
                _contentStream = null;
                _contentLength = 0;

                _followRedirects = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public interface IHttpResponseListener
        {
            void HandleHttpResponse( IHttpResponse httpResponse );

            void Failed( Exception t );

            void Cancelled();
        }

        public void SendHttpRequest( HttpRequest httpRequest, IHttpResponseListener httpResponseListener );

        public void CancelHttpRequest( HttpRequest httpRequest );

        public enum Protocol
        {
            Tcp
        }

        public IServerSocket NewServerSocket( Protocol protocol, string hostname, int port, ServerSocketHints hints );

        public IServerSocket NewServerSocket( Protocol protocol, int port, ServerSocketHints hints );

        public ISocket NewClientSocket( Protocol protocol, string host, int port, SocketHints hints );

        public bool OpenUri( string uri );
    }
}
