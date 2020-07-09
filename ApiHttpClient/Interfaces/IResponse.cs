using System.Collections.Generic;
using System.IO;

namespace ApiHttpClient.Interfaces
{
    public interface IResponse
    {
        string GeHeaderLine(string header);
        ICollection<string> GetHeader(string header);
        bool HasHeader(string header);
        Cookie GetCookie(string name);
        ICollection<Cookie> GetCookies();
        bool HasCookie(string name);
        string GetProtocolVersion();
        Stream GetBodyAsStream();
        string GetBodyAsString();
        int GetStatusCode();
        string GetReasonPhrase();
    }
}
