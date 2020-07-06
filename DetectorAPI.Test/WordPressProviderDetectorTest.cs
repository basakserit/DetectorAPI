using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DetectorAPI.Provider;
using DetectorAPI.Service;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DetectorAPI.Test
{
    public class WordPressProviderDetectorTest
    {
        private readonly WordPressProviderDetector _wordPressProviderDetector;

        public WordPressProviderDetectorTest()
        {
            var loggerMock = new Mock<ILogger<WordPressProviderDetector>>();

            var httpActionProviderMock = new Mock<IHttpActionProvider>();

            httpActionProviderMock.Setup(x =>
                x.GetResponseString(It.IsAny<string>())).Returns(Task.FromResult(string.Empty));

            httpActionProviderMock.Setup(x =>
                    x.GetResponseString(It.Is<string>(hostName => hostName.Equals("wordPressUri"))))
                .Returns(Task.FromResult("wp-content"));

            httpActionProviderMock.Setup(x =>
                    x.GetResponseString(It.Is<string>(hostName => hostName.Equals("httpRequestExceptionUri"))))
                .Throws<HttpRequestException>();

            httpActionProviderMock.Setup(x =>
                    x.GetResponseString(
                        It.Is<string>(hostName => hostName.Equals("invalidOperationExceptionUri"))))
                .Throws<InvalidOperationException>();

            httpActionProviderMock.Setup(x =>
                    x.GetResponseString(It.Is<string>(hostName => hostName.Equals("uriFormatException"))))
                .Throws<UriFormatException>();

            httpActionProviderMock.Setup(x =>
                    x.GetResponseString(It.Is<string>(hostName => hostName.Equals("webExceptionUri"))))
                .Throws<WebException>();

            _wordPressProviderDetector =
                new WordPressProviderDetector(loggerMock.Object, httpActionProviderMock.Object);
        }

        [Fact]
        public async Task Detect_ShouldReturnHosts_WithContentWp()
        {
            var result = await _wordPressProviderDetector.Detect(new List<string>()
            {
                "validUri", "wordPressUri"
            });

            var resultArray = result.Clients.ToArray();

            Assert.Single(resultArray);
            Assert.Equal("wordPressUri", resultArray[0].HostName);
        }

        [Fact]
        public async Task Detect_ShouldNotThrow_WhenHttpRequestException()
        {
            await _wordPressProviderDetector.Detect(new List<string>()
            {
                "httpRequestExceptionUri"
            });
        }

        [Fact]
        public async Task Detect_ShouldNotThrow_WhenInvalidOperationException()
        {
            await _wordPressProviderDetector.Detect(new List<string>()
            {
                "invalidOperationExceptionUri"
            });
        }

        [Fact]
        public async Task Detect_ShouldNotThrow_WhenUriFormatException()
        {
            await _wordPressProviderDetector.Detect(new List<string>()
            {
                "uriFormatException"
            });
        }

        [Fact]
        public async Task Detect_ShouldNotThrow_WhenWebException()
        {
            await _wordPressProviderDetector.Detect(new List<string>()
            {
                "webExceptionUri"
            });
        }
    }
}