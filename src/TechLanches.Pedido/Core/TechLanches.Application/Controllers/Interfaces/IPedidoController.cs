using TechLanches.Application.DTOs;
using TechLanches.Domain.Enums;
using TechLanches.Domain.ValueObjects;

namespace TechLanches.Application.Controllers.Interfaces
{
    public interface IPedidoController
    {
        Task<List<PedidoResponseDTO>> BuscarTodos();
        Task<PedidoResponseDTO> BuscarPorId(int idPedido);
        Task<List<PedidoResponseDTO>> BuscarPorStatus(StatusPedido statusPedido);
        Task<PedidoResponseDTO> Cadastrar(Cpf cpf, List<ItemPedido> itensPedido);
        Task<PedidoResponseDTO> TrocarStatus(int pedidoId, StatusPedido statusPedido);
        Task<bool> InativarDadosCliente(string cpf);
    }
}
