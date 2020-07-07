using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ApiHttpClient.Exceptions;
using ApiHttpClient.Interfaces;

namespace ApiHttpClient
{
    public class ApiHttpClient : ApiHttpClientBase
    {
        private readonly HttpClient _client;
        private IResponse _response;
        

        public ApiHttpClient()
        {
            _client = new HttpClient(new HttpClientHandler());
            _response = null;
        }

        public ApiHttpClient(HttpMessageHandler handler)
        {
            _client = new HttpClient(handler);
        }

        public ApiHttpClient(HttpClient client)
        {
            _client = client;
        }

        public override async Task MakeRequestAsync()
        {
            try
            {
                HttpResponseMessage response;
                var requestMessage = GetRequestMessage();
                using (requestMessage)
                {
                    response = await _client.SendAsync(requestMessage);
                }
                _response = new ApiHttpClientResponse(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServerBadRequestException();
            }
        }

        public override void MakeRequest()
        {
            try
            {
                HttpResponseMessage response;
                var requestMessage = GetRequestMessage();
                using (requestMessage)
                {
                    response = _client.SendAsync(requestMessage).Result;
                }
                _response =  new ApiHttpClientResponse(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ServerBadRequestException();
            }
        }

        public override IResponse GetResponse()
        {
            return _response;
        }

        protected HttpMethod GetHttpMethod()
        {
            switch (_method)
            {
                case RequestMethod.Get:       { return HttpMethod.Get;     }
                case RequestMethod.Post:      { return HttpMethod.Post;    }
                case RequestMethod.Delete:    { return HttpMethod.Delete;  }
                case RequestMethod.Head:      { return HttpMethod.Head;    }
                case RequestMethod.Put:       { return HttpMethod.Put;     }
                case RequestMethod.Patch:     { return HttpMethod.Trace;   }
                case RequestMethod.Option:    { return HttpMethod.Options; }
                default:                      { return HttpMethod.Get;     }
            }
        }

        protected HttpContent GetRequestBody()
        {
            if(_requestParameters == null) return new StringContent("", Encoding.UTF8);
            if(_files.Keys.Count == 0) return new FormUrlEncodedContent(_requestParameters);
            var formData = new MultipartFormDataContent(CreateFormDataBoundary());
            foreach(var item in _files.Keys)
                formData.Add(new StreamContent(_files[item]),item,_files[item].Name);
            foreach(var item in _requestParameters.Keys)
                formData.Add(new StringContent(_requestParameters[item]),item);
            return formData;
        }

        private string CreateFormDataBoundary()
        {
            return $"----------{Guid.NewGuid():N}";
        }

        protected HttpRequestMessage GetRequestMessage()
        {
            var requestMessage = new HttpRequestMessage();
            foreach (var item in _requestHeaders.Keys)
            {
                requestMessage.Headers.Add(item, _requestHeaders[item]);
            }
            requestMessage.Method = GetHttpMethod();
            requestMessage.RequestUri = new Uri(_url);
            if (new List<HttpMethod> { HttpMethod.Post, HttpMethod.Put }.Contains(requestMessage.Method))
            {
                requestMessage.Content = GetRequestBody();
            }
            return requestMessage;
        }
    }
}
