using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DetectorAPI.Provider
{
    public class HttpActionProvider : IHttpActionProvider
    {
        public async Task<IEnumerable<string>> GetResponseHeaders(string hostName, string key)
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(hostName);
            return response.Headers.GetValues("Server").ToList();
        }

        public Task<string> GetResponseString(string hostName)
        {
            return new WebClient().DownloadStringTaskAsync(hostName);
        }

        public async Task<IEnumerable<string>> GetIps(string hostName)
        {
            var host = hostName.Contains("https://") ? hostName.Replace("https://", "") : (hostName.Contains("http://") ? hostName.Replace("http://", "") : hostName);
            var hostAddresses = await Dns.GetHostAddressesAsync(host);
            return hostAddresses.Select(x => x.ToString());
        }
    }
}