using TechLanches.Domain.Enums;
using TechLanches.Domain.ValueObjects;

namespace TechLanches.Application.DTOs
{
    /// <summary>
    /// Schema utilizado para retorno de dados do pedido.
    /// </summary>
    public class PedidoResponseDTO
    {
        public PedidoResponseDTO()
        {
            ItensPedido = new List<ItemPedidoResponseDTO>();
            Pagamento = new PagamentoResponseDTO();
        }

        /// <summary>
        /// Id do pedido
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// Id do cliente
        /// </summary>
        /// <example>2</example>
        public string ClienteCpf { get; set; }

        /// <summary>
        /// Nome do Status do pedido 
        /// </summary>
        /// <example>PedidoCriado</example>
        public string NomeStatusPedido { get; set; }

        /// <summary>
        /// Status do pedido
        /// </summary>
        /// <example>PedidoCriado</example>
        public StatusPedido StatusPedido { get; set; }

        /// <summary>
        /// Valor total do pedido
        /// </summary>
        /// <example>12</example>
        public decimal Valor { get; set; }

        public List<ItemPedidoResponseDTO> ItensPedido { get; set; }

        public PagamentoResponseDTO Pagamento { get; set; }
    }
}
