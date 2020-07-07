using System;
using System.Linq;
using System.Net.Http;
using ApiHttpClient;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ApiHttpClient.ApiHttpClient(new HttpClient());
            client
                .SetUrl("https://jsonplaceholder.typicode.com/todos/1")
                .SetRequestMethod(RequestMethod.Get)
                .MakeRequest();
            var response = client.GetResponse();
            Console.WriteLine(response.GetStatusCode());
            Console.WriteLine(response.GetProtocolVersion());
            Console.WriteLine(response.GetReasonPhrase());
            Console.WriteLine(response.GetHeader("X-Powered-By").First());
            Console.WriteLine(response.HasHeader("X-Powered-By"));
            Console.WriteLine(response.HasHeader("Content-Type"));
            Console.WriteLine(response.GetBodyAsString());
            Console.WriteLine(response.GetBodyAsString());
            Console.WriteLine(response.GetBodyAsStream().Length);
            Console.WriteLine(response.GetBodyAsStream());
            Console.ReadLine();
        }
    }
}
