using Microsoft.AspNetCore.Mvc;
using System.Net;
using TechLanches.Adapter.API.Constantes;
using Swashbuckle.AspNetCore.Annotations;
using TechLanches.Application.DTOs;
using TechLanches.Application.Controllers.Interfaces;
using TechLanches.Domain.ValueObjects;
using System.IdentityModel.Tokens.Jwt;
using TechLanches.Domain.Constantes;

namespace TechLanches.Adapter.API.Endpoints
{
    public static class CheckoutEndpoints
    {
        public static void MapCheckoutEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/checkout", Checkout)
               .WithTags(EndpointTagConstantes.TAG_CHECKOUT)
               .WithMetadata(new SwaggerOperationAttribute(summary: "Realizar checkout do pedido", description: "Retorna o checkout do pedido"))
               .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.OK, type: typeof(CheckoutResponseDTO), description: "Checkout realizado com sucesso"))
               .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.BadRequest, type: typeof(ErrorResponseDTO), description: "Requisição inválida"))
               .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.NotFound, type: typeof(ErrorResponseDTO), description: "Falha ao realizar o checkout"))
               .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.InternalServerError, type: typeof(ErrorResponseDTO), description: "Erro no servidor interno"))
               .RequireAuthorization();
        }

        public static async Task<IResult> Checkout(
            [FromBody] PedidoRequestDTO pedidoDto,
            [FromServices] IPedidoController pedidoController,
            [FromServices] IProdutoController produtoController,
            [FromServices] ICheckoutController checkoutController
            )
        {
            if (pedidoDto.ItensPedido.Count == 0)
                return Results.BadRequest(MensagensConstantes.SEM_NENHUM_ITEM_PEDIDO);

            var itensPedido = new List<ItemPedido>();

            foreach (var itemPedido in pedidoDto.ItensPedido)
            {
                var dadosProduto = await produtoController.BuscarPorId(itemPedido.IdProduto);
                var itemPedidoCompleto = new ItemPedido(dadosProduto.Id, itemPedido.Quantidade, dadosProduto.Preco);

                itensPedido.Add(itemPedidoCompleto);
            }

            var novoPedido = await pedidoController.Cadastrar(RetornarCpf(), itensPedido);

            var checkoutValido = await checkoutController.ValidarCheckout(novoPedido.Id);

            var qrdCodeData = new PagamentoResponseDTO();

            if (checkoutValido)
                qrdCodeData = await checkoutController.GerarPagamentoCheckout(novoPedido.Id, novoPedido.Valor);

            return Results.Ok(qrdCodeData);
        }

        private static Cpf RetornarCpf()
        {
            //TODO pegar do httpcontext
            var cpf = Constants.USER_DEFAULT;
            //var cpf = user.Username == Constants.USER_DEFAULT ? Constants.CPF_USER_DEFAULT : user.Username;

            return new Cpf(cpf);
        }
    }
}
