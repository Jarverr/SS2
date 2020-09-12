using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAB4_API
{
    class Website
    {
        public Website(string link)
        {
            _linkToWeb = new RestClient(link);
        }
        public async Task<string> DownloadAsync(string path)
        {
            var request = new RestRequest(path, Method.GET);
            var response = _linkToWeb.ExecuteAsync(request);
            return response.Result.Content;
        }
        //to bylo z zajęć i nie wiem czemu to jest w kodzie, nigdzie nie jest uzyte
        /*public Task<IRestResponse> DownloadAsync(string path)
        {
            var request = new RestRequest(path, Method.GET);
            var response = _linkToWeb.ExecuteAsync(request);
            return response;
        }*/
        public RestClient _linkToWeb { get; set; }
    }
}
