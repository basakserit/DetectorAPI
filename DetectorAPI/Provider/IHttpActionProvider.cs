using System.Collections.Generic;
using System.Threading.Tasks;

namespace DetectorAPI.Provider
{
    public interface IHttpActionProvider
    {
        Task<IEnumerable<string>> GetResponseHeaders(string hostName, string key);
        Task<string> GetResponseString(string hostName);
        Task<IEnumerable<string>> GetIps(string hostName);
    }
}