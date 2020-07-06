using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DetectorAPI.Provider;
using DetectorAPI.Service;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DetectorAPI.Test
{
    public class NginxProviderDetectorTest
    {
        private readonly NginxProviderDetector _nginxProviderDetector;

        public NginxProviderDetectorTest()
        {
            var loggerMock = new Mock<ILogger<NginxProviderDetector>>();

            var httpActionProviderMock = new Mock<IHttpActionProvider>();

            httpActionProviderMock.Setup(x =>
                x.GetResponseHeaders(It.Is<string>(hostName => hostName.Equals("nginxUri")),
                    It.Is<string>(key => key.Equals("Server")))).Returns(Task.FromResult(new List<string>()
            {
                "nginx"
            }.AsEnumerable()));

            httpActionProviderMock.Setup(x =>
                x.GetIps(It.Is<string>(hostname => hostname.Equals("nginxUri")))).Returns(Task.FromResult(new List<string>()
                {
                    "0.0.0.0"
                }.AsEnumerable()));

            httpActionProviderMock.Setup(x =>
                x.GetResponseHeaders(It.Is<string>(hostName => hostName.Equals("httpRequestExceptionUri")),
                    It.Is<string>(key => key.Equals("Server")))).Throws<HttpRequestException>();

            httpActionProviderMock.Setup(x =>
                x.GetResponseHeaders(It.Is<string>(hostName => hostName.Equals("invalidOperationExceptionUri")),
                    It.Is<string>(key => key.Equals("Server")))).Throws<InvalidOperationException>();

            httpActionProviderMock.Setup(x =>
                x.GetResponseHeaders(It.Is<string>(hostName => hostName.Equals("uriFormatException")),
                    It.Is<string>(key => key.Equals("Server")))).Throws<UriFormatException>();

            _nginxProviderDetector = new NginxProviderDetector(loggerMock.Object, httpActionProviderMock.Object);
        }

        [Fact]
        public async Task Detect_ShouldReturnHosts_WithHeaderNameNginx()
        {
            var result = await _nginxProviderDetector.Detect(new List<string>()
            {
                "validUri", "nginxUri"
            });

            var resultArray = result.Clients.ToArray();

            Assert.Single(resultArray);
            Assert.Equal("nginxUri", resultArray[0].HostName);
        }

        [Fact]
        public async Task Detect_ShouldReturnHosts_WithIpAdressess()
        {
            var result = await _nginxProviderDetector.Detect(new List<string>() { "nginxUri", "validUri" });

            Assert.Single(result.Clients);
            Assert.Equal("nginxUri", result.Clients[0].HostName);
            Assert.Single(result.Clients[0].IpAddresses);
            Assert.Equal("0.0.0.0", result.Clients[0].IpAddresses[0]);
            Assert.Empty(result.ClientErrors);
        }

        [Fact]
        public async Task Detect_ShouldNotThrow_WhenHttpRequestException()
        {
            await _nginxProviderDetector.Detect(new List<string>()
            {
                "httpRequestExceptionUri"
            });
        }

        [Fact]
        public async Task Detect_ShouldNotThrow_WhenInvalidOperationException()
        {
            await _nginxProviderDetector.Detect(new List<string>()
            {
                "invalidOperationExceptionUri"
            });
        }

        [Fact]
        public async Task Detect_ShouldNotThrow_WhenUriFormatException()
        {
            await _nginxProviderDetector.Detect(new List<string>()
            {
                "uriFormatException"
            });
        }
    }
}