using System.Collections.Generic;
using System.Threading.Tasks;
using DetectorAPI.V1.Models;

namespace DetectorAPI.Service
{
    public interface IDetectorService
    {
        public Task<ProviderDetectionResponse> DetectProvider(IEnumerable<string> hostnames);
        public Task<IpDetectionResponse> DetectIps(IEnumerable<string> hostnames);
    }
}