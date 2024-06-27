using Microsoft.AspNetCore.Mvc;
using System.Net;
using TechLanches.Adapter.API.Constantes;
using Swashbuckle.AspNetCore.Annotations;
using TechLanches.Application.DTOs;
using TechLanches.Domain.Enums;
using TechLanches.Application.Controllers.Interfaces;
using TechLanches.Domain.ValueObjects;
using TechLanches.Domain.Constantes;

namespace TechLanches.Adapter.API.Endpoints;

public static class PedidoEndpoints
{
    public static void MapPedidoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/pedidos", BuscarPedidos)
           .WithTags(EndpointTagConstantes.TAG_PEDIDO)
           .WithMetadata(new SwaggerOperationAttribute(summary: "Obter todos os pedidos", description: "Retorna todos os pedidos cadastrados"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.OK, type: typeof(List<PedidoResponseDTO>), description: "Pedidos encontrados com sucesso"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.BadRequest, type: typeof(ErrorResponseDTO), description: "Requisição inválida"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.NotFound, type: typeof(ErrorResponseDTO), description: "Pedidos não encontrados"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.InternalServerError, type: typeof(ErrorResponseDTO), description: "Erro no servidor interno"))
           .RequireAuthorization();

        app.MapGet("api/pedidos/{idPedido}", BuscarPedidoPorId)
           .WithTags(EndpointTagConstantes.TAG_PEDIDO)
           .WithMetadata(new SwaggerOperationAttribute(summary: "Obter todos os pedidos por id", description: "Retorna o pedido cadastrado por id"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.OK, type: typeof(PedidoResponseDTO), description: "Pedido encontrado com sucesso"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.BadRequest, type: typeof(ErrorResponseDTO), description: "Requisição inválida"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.NotFound, type: typeof(ErrorResponseDTO), description: "Pedido não encontrado"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.InternalServerError, type: typeof(ErrorResponseDTO), description: "Erro no servidor interno"))
           .RequireAuthorization();

        app.MapGet("api/pedidos/buscarpedidosporstatus/{statusPedido}", BuscarPedidosPorStatus)
          .WithTags(EndpointTagConstantes.TAG_PEDIDO)
          .WithMetadata(new SwaggerOperationAttribute(summary: "Obter todos os pedidos por status", description: "Retorna todos os pedidos contidos no status"))
          .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.OK, type: typeof(List<PedidoResponseDTO>), description: "Pedidos encontrados com sucesso"))
          .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.BadRequest, type: typeof(ErrorResponseDTO), description: "Requisição inválida"))
          .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.NotFound, type: typeof(ErrorResponseDTO), description: "Pedidos não encontrados"))
          .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.InternalServerError, type: typeof(ErrorResponseDTO), description: "Erro no servidor interno"))
          .RequireAuthorization();

        app.MapPut("api/pedidos/{idPedido}/trocarstatus", TrocarStatus)
           .WithTags(EndpointTagConstantes.TAG_PEDIDO)
           .WithMetadata(new SwaggerOperationAttribute(summary: "Trocar status do pedido", description: "Efetua a troca de status do pedido"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.OK, type: typeof(PedidoResponseDTO), description: "Status do pedido alterado com sucesso"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.BadRequest, type: typeof(ErrorResponseDTO), description: "Requisição inválida"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.NotFound, type: typeof(ErrorResponseDTO), description: "Status do pedido não alterado"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.InternalServerError, type: typeof(ErrorResponseDTO), description: "Erro no servidor interno"))
           .RequireAuthorization();

        app.MapGet("api/pedidos/statuspedidos", BuscarStatusPedidos)
           .WithTags(EndpointTagConstantes.TAG_PEDIDO)
           .WithMetadata(new SwaggerOperationAttribute(summary: "Buscar status pedidos", description: "Retorna todos os status de pedidos"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.OK, type: typeof(List<StatusPedidoResponseDTO>), description: "Status do pedido encontrado com sucesso"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.BadRequest, type: typeof(ErrorResponseDTO), description: "Requisição inválida"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.NotFound, type: typeof(ErrorResponseDTO), description: "Status do pedido não encontrado"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.InternalServerError, type: typeof(ErrorResponseDTO), description: "Erro no servidor interno"))
           .RequireAuthorization();

        app.MapPut("api/pedidos/{cpf}/inativar", InativarPedidosCliente)
           .WithTags(EndpointTagConstantes.TAG_PEDIDO)
           .WithMetadata(new SwaggerOperationAttribute(summary: "Inativar dados do cliente nos pedidos", description: "Inativa dados relacionados ao cliente"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.OK, type: typeof(List<StatusPedidoResponseDTO>), description: "Operação realizada com sucesso"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.BadRequest, type: typeof(ErrorResponseDTO), description: "Requisição inválida"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.NotFound, type: typeof(ErrorResponseDTO), description: "Pedidos do cliente não encontrados"))
           .WithMetadata(new SwaggerResponseAttribute((int)HttpStatusCode.InternalServerError, type: typeof(ErrorResponseDTO), description: "Erro no servidor interno"))
           .RequireAuthorization();
    }


    private static async Task<IResult> BuscarPedidos(
        [FromServices] IPedidoController pedidoController)
    {
        var pedidos = await pedidoController.BuscarTodos();

        return pedidos is not null
            ? Results.Ok(pedidos)
            : Results.BadRequest(new ErrorResponseDTO { MensagemErro = "Erro ao buscar pedidos.", StatusCode = HttpStatusCode.BadRequest });
    }

    private static async Task<IResult> BuscarPedidoPorId(
        int idPedido,
        [FromServices] IPedidoController pedidoController)
    {
        var pedido = await pedidoController.BuscarPorId(idPedido);

        return pedido is not null
            ? Results.Ok(pedido)
            : Results.NotFound(new ErrorResponseDTO { MensagemErro = $"Pedido não encontrado para o id: {idPedido}.", StatusCode = HttpStatusCode.NotFound });
    }

    private static async Task<IResult> BuscarPedidosPorStatus(
        StatusPedido statusPedido,
        [FromServices] IPedidoController pedidoController)
    {
        var pedidos = await pedidoController.BuscarPorStatus(statusPedido);

        return pedidos is not null
            ? Results.Ok(pedidos)
            : Results.BadRequest(new ErrorResponseDTO { MensagemErro = "Erro ao buscar pedidos por status.", StatusCode = HttpStatusCode.BadRequest });
    }

    private static async Task<IResult> InativarPedidosCliente(
    string cpf,
    [FromServices] IPedidoController pedidoController)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            Results.BadRequest(new ErrorResponseDTO { MensagemErro = "CPF Inválido.", StatusCode = HttpStatusCode.BadRequest });

        bool retorno = false;

        try
        {
            var cpfValido = new Cpf(cpf);

            if(cpfValido.Numero == Constants.CPF_USER_DEFAULT)
                Results.BadRequest(new ErrorResponseDTO { MensagemErro = "Não é possível inativar os dados desse cliente.", StatusCode = HttpStatusCode.BadRequest });

            retorno = await pedidoController.InativarDadosCliente(cpfValido.Numero);
        }
        catch (Exception)
        {
            Results.BadRequest(new ErrorResponseDTO { MensagemErro = "CPF Inválido.", StatusCode = HttpStatusCode.BadRequest });
        }

        return retorno
            ? Results.Ok(retorno)
            : Results.BadRequest(new ErrorResponseDTO { MensagemErro = "Erro ao inativar dados do cliente.", StatusCode = HttpStatusCode.BadRequest });
    }

    private static async Task<IResult> TrocarStatus(
        [FromRoute] int idPedido,
        [FromBody] int statusPedido,
        [FromServices] IPedidoController pedidoController)
    {
        if (!Enum.IsDefined(typeof(StatusPedido), statusPedido))
            return Results.BadRequest();

        var pedido = await pedidoController.TrocarStatus(idPedido, (StatusPedido)statusPedido);

        return pedido is not null
            ? Results.Ok(pedido)
            : Results.BadRequest(new ErrorResponseDTO { MensagemErro = "Erro ao trocar status.", StatusCode = HttpStatusCode.BadRequest });
    }

    private static async Task<IResult> BuscarStatusPedidos()
    {
        var statusPedidos = Enum.GetValues(typeof(StatusPedido))
            .Cast<StatusPedido>()
            .Select(x => new StatusPedidoResponseDTO { Id = (int)x, Nome = x.ToString() })
            .ToList();

        return statusPedidos is not null && statusPedidos.Count > 0
            ? Results.Ok(await Task.FromResult(statusPedidos))
            : Results.NotFound(new ErrorResponseDTO { MensagemErro = "Nenhum status encontrado.", StatusCode = HttpStatusCode.NotFound });
    }
}
