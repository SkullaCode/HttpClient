using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiHttpClient.Interfaces;
using static System.Int32;

namespace ApiHttpClient
{
    public abstract class ApiHttpClientBase : IApiHttpClient
    {
        protected string _url;
        protected string _method;
        protected int _timeout;
        protected string _query;
        protected string _body;
        protected bool _queryLock;
        protected bool _bodyLock;
        protected readonly IDictionary<string, string> _requestParameters;
        protected readonly IDictionary<string, string> _requestHeaders;
        protected readonly IDictionary<string, string> _requestQueryParameters;
        protected readonly IDictionary<string, FileStream> _files;
        protected readonly ICollection<Cookie> _cookies;

        protected ApiHttpClientBase()
        {
            _url = null;
            _method = null;
            _timeout = MinValue;
            _queryLock = false;
            _bodyLock = false;

            _requestParameters = new Dictionary<string, string>();
            _requestHeaders = new Dictionary<string, string>();
            _requestQueryParameters = new Dictionary<string, string>();
            _files = new Dictionary<string, FileStream>();
            _cookies = new List<Cookie>();
        }

        public IApiHttpClient SetUrl(string url)
        {
            _url = url;
            return this;
        }

        public IApiHttpClient SetQueryParameter(string name, string value)
        {
            if (_queryLock) return this;
            _requestQueryParameters.Add(name, value);
            return this;
        }

        public IApiHttpClient SetQuery(string query)
        {
            _query = query;
            _queryLock = true;
            return this;
        }

        public IApiHttpClient SetQuery(IDictionary<string, string> collection)
        {
            foreach (var item in collection.Keys)
                SetQueryParameter(item, collection[item]);
            return this;
        }

        public IApiHttpClient SetBodyParameter(string name, string value)
        {
            if (_bodyLock) return this;
            _requestParameters.Add(name, value);
            return this;
        }

        public IApiHttpClient SetBody(string body)
        {
            _body = body;
            _bodyLock = true;
            return this;
        }

        public IApiHttpClient SetBody(IDictionary<string, string> collection)
        {
            foreach (var item in collection.Keys)
                SetBodyParameter(item, collection[item]);
            return this;
        }

        public IApiHttpClient SetHeaderParameter(string name, string value)
        {
            _requestHeaders.Add(name, value);
            return this;
        }

        public IApiHttpClient SetHeader(IDictionary<string, string> collection)
        {
            foreach (var item in collection.Keys)
                SetHeaderParameter(item, collection[item]);
            return this;
        }

        public IApiHttpClient SetCookie(Cookie cookie)
        {
           _cookies.Add(cookie);
           return this;
        }

        public IApiHttpClient SetFile(string name, FileStream content)
        {
            _files.Add(name, content);
            return this;
        }

        public IApiHttpClient SetRequestMethod(string method)
        {
            _method = method;
            return this;
        }

        public IApiHttpClient SetRequestTimeout(int timeout)
        {
            _timeout = timeout;
            return this;
        }

        public IApiHttpClient FollowRedirects(int depth)
        {
            throw new NotImplementedException();
        }

        public abstract Task MakeRequestAsync();
        public abstract void MakeRequest();

        public abstract IResponse GetResponse();

        protected string BuildUrl()
        {
            var builder = new UriBuilder(_url)
            {
                Port = -1,
                Query = BuildQuery()
            };
            return builder.ToString();
        }

        protected string BuildQuery()
        {
            //todo url encode values
            return string.Join("&", _requestQueryParameters.Select(item => $"{item.Key}={item.Value}").ToList());
        }
    }
}
