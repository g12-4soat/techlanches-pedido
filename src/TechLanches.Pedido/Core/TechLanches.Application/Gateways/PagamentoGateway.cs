using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TechLanches.Application.Constantes;
using TechLanches.Application.DTOs;
using TechLanches.Application.Gateways.Interfaces;

namespace TechLanches.Application.Gateways
{
    public class PagamentoGateway : IPagamentoGateway
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<PagamentoGateway> _logger;

        public PagamentoGateway(
            IHttpClientFactory httpClientFactory,
            IMemoryCache memoryCache,
            ILogger<PagamentoGateway> logger)
        {
            _httpClient = httpClientFactory.CreateClient(Constants.NOME_API_PAGAMENTOS);
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<PagamentoResponseDTO> GerarPagamento(PagamentoRequestDTO pagamentoRequest)
        {
            AddAuthenticationHeader();

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/pagamentos", pagamentoRequest);

                var result = await response.Content.ReadFromJsonAsync<PagamentoResponseDTO>();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar requisição para API Pagamentos");
                throw;
            }
        }

        public async Task<PagamentoResponseDTO> RetornarPagamentoPorPedidoId(int pedidoId)
        {
            AddAuthenticationHeader();

            try
            {
                return await _httpClient.GetFromJsonAsync<PagamentoResponseDTO>($"api/pagamentos/status/{pedidoId}");
            }
            catch (HttpRequestException httpEx) when (httpEx.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar requisição para API Pagamentos");
                throw;
            }
        }

        private void AddAuthenticationHeader()
        {
            var token = _memoryCache.Get<string>(Constants.AUTH_TOKEN_KEY);

            if (token == null)
            {
                _logger.LogError("Token não encontrado no cache");
                throw new ArgumentNullException(nameof(token));
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
