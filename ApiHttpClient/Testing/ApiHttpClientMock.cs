using System;
using System.Net.Http;
using System.Threading.Tasks;
using ApiHttpClient.Exceptions;
using ApiHttpClient.Interfaces;

namespace ApiHttpClient.Testing
{
    public class ApiHttpClientMock : ApiHttpClientBase
    {
        private readonly ResponseHandling _handler;
        private readonly IResponse _response;

        public ApiHttpClientMock(ResponseHandling handler, HttpResponseMessage sampleResponse)
        {
            _handler = handler;
            _response = new ApiHttpClientResponse(sampleResponse);
        }


        public override async Task MakeRequestAsync()
        {
            switch (_handler)
            {
                case ResponseHandling.ReturnException:
                    throw new ServerBadRequestException();
                case ResponseHandling.ReturnRandom:
                    {
                        var random = new Random();
                        if ((random.Next(int.MinValue, int.MaxValue) % 2) == 0)
                            throw new ServerBadRequestException();
                        break;
                    }
                case ResponseHandling.ReturnSuccess:
                {
                    await new Task(null);
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void MakeRequest()
        {
            switch (_handler)
            {
                case ResponseHandling.ReturnException:
                    throw new ServerBadRequestException();
                case ResponseHandling.ReturnRandom:
                {
                    var random = new Random();
                    if ((random.Next(int.MinValue, int.MaxValue) % 2) == 0)
                        throw new ServerBadRequestException();
                    break;
                }
                case ResponseHandling.ReturnSuccess:
                {

                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override IResponse GetResponse()
        {
            return _response;
        }
    }
}
