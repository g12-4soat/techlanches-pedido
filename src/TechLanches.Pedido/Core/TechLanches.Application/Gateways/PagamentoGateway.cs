﻿using Microsoft.Extensions.Caching.Memory;
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
            AddAuthenticationHeader(Constants.AUTH_TOKEN_KEY);

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/pagamentos/qrcode", pagamentoRequest);

                var result = await response.Content.ReadFromJsonAsync<PagamentoResponseDTO>();
                result.PedidoId = pagamentoRequest.PedidoId;
                result.Valor = pagamentoRequest.Valor;
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
            AddAuthenticationHeader(Constants.AUTH_TOKEN_KEY);

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

        public void AddAuthenticationHeader(string key)
        {
            GetAuthenticationHeader(key);
        }

        private void GetAuthenticationHeader(string key)
        {
            var token = _memoryCache.Get<string>(key);

            ArgumentException.ThrowIfNullOrEmpty(token, nameof(token));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
