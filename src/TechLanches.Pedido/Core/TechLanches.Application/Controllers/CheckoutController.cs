using Microsoft.Extensions.Options;
using TechLanches.Adapter.ACL.Pagamento.QrCode.Provedores.MercadoPago;
using TechLanches.Application.Controllers.Interfaces;
using TechLanches.Application.DTOs;
using TechLanches.Application.Gateways;
using TechLanches.Application.Gateways.Interfaces;
using TechLanches.Application.Options;
using TechLanches.Application.Ports.Repositories;
using TechLanches.Application.Presenters.Interfaces;
using TechLanches.Application.UseCases.Pagamentos;
using TechLanches.Domain.Enums;

namespace TechLanches.Application.Controllers
{
    public class CheckoutController : ICheckoutController
    {
        private readonly IPedidoGateway _pedidoGateway;
        private readonly ICheckoutPresenter _checkoutPresenter;

        public CheckoutController(
            IMercadoPagoMockadoService mercadoPagoMockadoService, 
            IMercadoPagoService mercadoPagoService, 
            IPedidoRepository pedidoRepository, 
            ICheckoutPresenter checkoutPresenter, 
            IOptions<ApplicationOptions> applicationOptions)
        {
            _pedidoGateway = new PedidoGateway(pedidoRepository);
            _checkoutPresenter = checkoutPresenter;
        }

        public async Task<bool> ValidarCheckout(int pedidoId)
            => await CheckoutUseCase.ValidarPedidoCompleto(pedidoId, _pedidoGateway);

        public async Task<CheckoutResponseDTO> CriarPagamentoCheckout(int pedidoId, bool getImage = false)
        {
            return null;
            //var pedido = await _pedidoGateway.BuscarPorId(pedidoId);

            //var pedidoMercadoPago = _checkoutPresenter.ParaPedidoACLDto(pedido);

            //var qrCode = await _pagamentoGateway.GerarPagamentoEQrCodeMercadoPago(pedidoMercadoPago);

            //await PagamentoUseCase.Cadastrar(pedidoId, FormaPagamento.QrCodeMercadoPago, pedido.Valor, _pagamentoGateway);
            //await _pagamentoGateway.CommitAsync();

            //var bytesQrCode = new byte[] { };

            //if (getImage) bytesQrCode = _qrCodeGeneratorService.GenerateByteArray(qrCode);

            //return _checkoutPresenter.ParaDto(pedidoId, qrCode, bytesQrCode);
        }
    }
}