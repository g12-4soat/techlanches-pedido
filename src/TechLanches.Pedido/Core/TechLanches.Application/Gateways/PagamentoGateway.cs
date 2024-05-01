using System.Net.Http.Json;
using TechLanches.Application.Constantes;
using TechLanches.Application.DTOs;
using TechLanches.Application.Gateways.Interfaces;

namespace TechLanches.Application.Gateways
{
    public class PagamentoGateway : IPagamentoGateway
    {
        private readonly HttpClient _httpClient;

        public PagamentoGateway(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient(Constants.NOME_API_PAGAMENTOS);
        }

        public async Task<PagamentoResponseDTO> GerarPagamento(PagamentoRequestDTO pagamentoRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/pagamentos", pagamentoRequest);
                                
            var result = await response.Content.ReadFromJsonAsync<PagamentoResponseDTO>();

            return result;
            //return Task.FromResult(new CheckoutResponseDTO { PedidoId = pedidoId });
        }

        public async Task<PagamentoResponseDTO> RetornarPagamentoPorPedidoId(int pedidoId)
        {
            return await _httpClient.GetFromJsonAsync<PagamentoResponseDTO>($"api/pagamentos/status/{pedidoId}");
            //return Task.FromResult(new PagamentoResponseDTO());
        }
    }
}
