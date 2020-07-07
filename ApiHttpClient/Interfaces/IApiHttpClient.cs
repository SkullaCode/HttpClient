using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ApiHttpClient.Interfaces
{
    public interface IApiHttpClient
    {
        IApiHttpClient SetUrl(string url);
        IApiHttpClient SetQueryParameter(string name, string value);
        IApiHttpClient SetQuery(string query);
        IApiHttpClient SetQuery(IDictionary<string,string> collection);
        IApiHttpClient SetBodyParameter(string name, string value);
        IApiHttpClient SetBody(string body);
        IApiHttpClient SetBody(IDictionary<string, string> collection);
        IApiHttpClient SetHeaderParameter(string name, string value);
        IApiHttpClient SetHeader(IDictionary<string, string> collection);
        IApiHttpClient SetCookieParameter(string name, string value);
        IApiHttpClient SetCookie(IDictionary<string, string> collection);
        IApiHttpClient SetFile(string name, FileStream content);
        IApiHttpClient SetRequestMethod(string method);
        IApiHttpClient SetRequestTimeout(int timeout);
        IApiHttpClient FollowRedirects(int depth);
        Task  MakeRequestAsync();
        void MakeRequest();
        IResponse GetResponse();
    }
}
