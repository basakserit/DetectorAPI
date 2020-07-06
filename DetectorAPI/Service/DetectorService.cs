using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using DetectorAPI.Provider;
using DetectorAPI.V1.Models;
using Microsoft.Extensions.Logging;

namespace DetectorAPI.Service
{
    public class DetectorService : IDetectorService
    {
        private readonly IEnumerable<IProviderDetector> _providerDetectors;
        private readonly IHttpActionProvider _httpActionProvider;
        private readonly ILogger<DetectorService> _logger;

        public DetectorService(IEnumerable<IProviderDetector> providerDetectors, IHttpActionProvider httpActionProvider, ILogger<DetectorService> logger)
        {
            _providerDetectors = providerDetectors;
            _httpActionProvider = httpActionProvider;
            _logger = logger;
        }

        public async Task<ProviderDetectionResponse> DetectProvider(IEnumerable<string> hostnames)
        {
            var detections = new List<ProviderDetection>();

            foreach (var providerDetector in _providerDetectors)
            {
                var detectionResult = await providerDetector.Detect(hostnames);
                detections.Add(detectionResult);
            }

            return new ProviderDetectionResponse()
            {
                Detections = detections.ToArray()
            };
        }

        public async Task<IpDetectionResponse> DetectIps(IEnumerable<string> hostnames)
        {
            var detections = new List<IpDetection>();

            foreach (var hostname in hostnames)
            {
                try
                {
                    var ips = await _httpActionProvider.GetIps(hostname);
                    detections.Add(new IpDetection()
                    {
                        HostAddress = hostname,
                        Ips = ips.ToArray()
                    });
                }
                catch (SocketException exception)
                {
                    detections.Add(new IpDetection()
                    {
                        HostAddress = hostname,
                        ErrorMessage = exception.Message
                    });
                    _logger.LogError(exception.Message);
                }
            }

            return new IpDetectionResponse()
            {
                Detection = detections.ToArray()
            };
        }
    }
}