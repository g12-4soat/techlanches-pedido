using Microsoft.Extensions.Caching.Memory;
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

        public PagamentoGateway(
            IHttpClientFactory httpClientFactory,
            IMemoryCache memoryCache)
        {
            _httpClient = httpClientFactory.CreateClient(Constants.NOME_API_PAGAMENTOS);
            _memoryCache = memoryCache;
        }

        public async Task<PagamentoResponseDTO> GerarPagamento(PagamentoRequestDTO pagamentoRequest)
        {
            AddAuthenticationHeader();

            var response = await _httpClient.PostAsJsonAsync("api/pagamentos", pagamentoRequest);

            var result = await response.Content.ReadFromJsonAsync<PagamentoResponseDTO>();

            return result;
        }

        public async Task<PagamentoResponseDTO> RetornarPagamentoPorPedidoId(int pedidoId)
        {
            AddAuthenticationHeader();

            return await _httpClient.GetFromJsonAsync<PagamentoResponseDTO>($"api/pagamentos/status/{pedidoId}");
        }

        private void AddAuthenticationHeader()
        {
            var token = _memoryCache.Get<string>(Constants.AUTH_TOKEN_KEY);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
