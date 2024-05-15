using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TechLanches.Application.Gateways;

namespace TechLanchesPedido.Tests.UnitTests.Application
{
    public class PagamentoGatewayTest
    {
        private IHttpClientFactory _httpClientFactory;
        private IMemoryCache _memoryCache;
        private ILogger<PagamentoGateway> _logger;

        public PagamentoGatewayTest()
        {
            _httpClientFactory = Substitute.For<IHttpClientFactory>();
            _memoryCache = Substitute.For<IMemoryCache>();
            _logger = Substitute.For<ILogger<PagamentoGateway>>();
        }

        [Fact]
        public void AddAuthenticationHeader_WithNullToken_ThrowsException()
        {
            // Arrange
            var httpClient = new HttpClient();
            _httpClientFactory.CreateClient(Arg.Any<string>()).Returns(httpClient);
            var gateway = new PagamentoGateway(_httpClientFactory, _memoryCache, _logger);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => gateway.AddAuthenticationHeader(null));
        }
    }
}
