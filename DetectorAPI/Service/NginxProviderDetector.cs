using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using DetectorAPI.Provider;
using DetectorAPI.V1.Models;
using Microsoft.Extensions.Logging;

namespace DetectorAPI.Service
{
    public class NginxProviderDetector : IProviderDetector
    {
        private readonly ILogger<NginxProviderDetector> _logger;
        private readonly IHttpActionProvider _httpActionProvider;

        public NginxProviderDetector(ILogger<NginxProviderDetector> logger, IHttpActionProvider httpActionProvider)
        {
            _logger = logger;
            _httpActionProvider = httpActionProvider;
        }

        public string GetProviderName() => "Nginx";

        public async Task<ProviderDetection> Detect(IEnumerable<string> hostnames)
        {
            var clients = new List<Domain>();
            var errors = new List<ClientError>();

            foreach (var hostname in hostnames)
            {
                try
                {
                    var headerInfo = await _httpActionProvider.GetResponseHeaders(hostname, "Server");

                    if (headerInfo.Contains("nginx"))
                    {
                        var ips = await _httpActionProvider.GetIps(hostname);

                        clients.Add(new Domain()
                        {
                            HostName = hostname,
                            IpAddresses = ips.ToArray()
                        });
                    }

                }
                catch (SocketException exception)
                {
                    errors.Add(new ClientError()
                    {
                        Client = hostname,
                        ErrorMessage = exception.Message
                    });
                    _logger.LogError(exception.Message);
                }
                catch (HttpRequestException exception)
                {
                    errors.Add(new ClientError()
                    {
                        Client = hostname,
                        ErrorMessage = exception.Message
                    });
                    _logger.LogError(exception.Message);
                }
                catch (InvalidOperationException exception)
                {
                    errors.Add(new ClientError()
                    {
                        Client = hostname,
                        ErrorMessage = exception.Message
                    });
                    _logger.LogError(exception.Message);
                }
                catch (UriFormatException exception)
                {
                    errors.Add(new ClientError()
                    {
                        Client = hostname,
                        ErrorMessage = exception.Message
                    });
                    _logger.LogError(exception.Message);
                }
            }

            return new ProviderDetection()
            {
                Provider = GetProviderName(),
                ClientErrors = errors.ToArray(),
                Clients = clients.ToArray()
            };
        }
    }
}
