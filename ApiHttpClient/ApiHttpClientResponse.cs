using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using ApiHttpClient.Exceptions;
using ApiHttpClient.Interfaces;

namespace ApiHttpClient
{
    public class ApiHttpClientResponse : IResponse
    {
        private readonly HttpResponseMessage _response;
        public ApiHttpClientResponse(HttpResponseMessage response)
        {
            _response = response;
        }
        public string GeHeaderLine(string header)
        {
            var res = GetHeader(header);
            var str = string.Join(", ", res);
            return str;
        }

        public ICollection<string> GetHeader(string header)
        {
            var res = _response.Headers.FirstOrDefault(x => x.Key == header);
            if(res.Key == null) return new List<string>();
            return (ICollection<string>) res.Value;
        }

        public bool HasHeader(string header)
        {
            var res = _response.Headers.FirstOrDefault(x => x.Key == header);
            return (res.Key != null && res.Key == header);
        }

        public string GetProtocolVersion()
        {
            return _response.Version.ToString();
        }

        public Stream GetBodyAsStream()
        {
            var stream = _response.Content.ReadAsStreamAsync().Result;
            return !stream.CanRead ? Stream.Null : stream;
        }

        public string GetBodyAsString()
        {
            var stream = GetBodyAsStream();
            if (!stream.CanRead) return "";
            using (var reader = new StreamReader(GetBodyAsStream()))
            {
                return reader.ReadToEnd();
            }
        }

        public int GetStatusCode()
        {
            return (int)_response.StatusCode;
        }

        public string GetReasonPhrase()
        {
            return _response.ReasonPhrase;
        }
    }
}
