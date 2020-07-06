using System.Collections.Generic;
using System.Threading.Tasks;
using DetectorAPI.V1.Models;

namespace DetectorAPI.Service
{
    public interface IProviderDetector
    {
        public string GetProviderName();
        public Task<ProviderDetection> Detect(IEnumerable<string> hostnames);
    }
}
