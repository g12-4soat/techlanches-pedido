namespace TechLanches.Application.DTOs
{
    /// <summary>
    /// Schema utilizado para envio de dados do pedido.
    /// </summary>
    public class PedidoRequestDTO
    {
        public List<ItemPedidoRequestDTO> ItensPedido { get; set; } 
    }

    /// <summary>
    /// Schema utilizado para envio de dados do itens pedido.
    /// </summary>
    public class ItemPedidoRequestDTO
    {
        /// <summary>
        /// Id do produto
        /// </summary>
        /// <example>1</example>
        public int IdProduto { get; set; }

        /// <summary>
        /// Quantidade do item
        /// </summary>
        /// <example>2</example>
        public int Quantidade { get; set; }
    }
}
