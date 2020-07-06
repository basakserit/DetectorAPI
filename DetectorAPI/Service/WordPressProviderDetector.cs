using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DetectorAPI.Provider;
using DetectorAPI.V1.Models;
using Microsoft.Extensions.Logging;

namespace DetectorAPI.Service
{
    public class WordPressProviderDetector : IProviderDetector
    {
        private readonly ILogger<WordPressProviderDetector> _logger;
        private readonly IHttpActionProvider _httpActionProvider;

        public WordPressProviderDetector(ILogger<WordPressProviderDetector> logger,
            IHttpActionProvider httpActionProvider)
        {
            _logger = logger;
            _httpActionProvider = httpActionProvider;
        }

        public string GetProviderName() => "WordPress";

        public async Task<ProviderDetection> Detect(IEnumerable<string> hostnames)
        {
            var clients = new List<Domain>();
            var errors = new List<ClientError>();

            foreach (var hostname in hostnames)
            {
                try
                {
                    var data = await _httpActionProvider.GetResponseString(hostname);

                    if (data.Contains("wp-content"))
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
                catch (WebException exception)
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
                catch (Exception exception)
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
                Clients = clients.ToArray(),
                ClientErrors = errors.ToArray()
            };
        }
    }
}