﻿namespace TechLanches.Application.DTOs
{
    public class PagamentoRequestDTO
    {
        /// <summary>
        /// Id Pedido
        /// </summary>
        /// <example>1</example>
        public int PedidoId { get; set; }

        /// <summary>
        /// Valor Pagamento
        /// </summary>
        /// <example>15.50</example>
        public decimal Valor { get; set; }
    }
}
