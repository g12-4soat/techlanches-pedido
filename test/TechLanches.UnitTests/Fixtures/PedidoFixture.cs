﻿using TechLanches.Domain.Constantes;

namespace TechLanches.UnitTests.Fixtures
{
    public class PedidoFixture : IDisposable
    {
        public readonly IStatusPedidoValidacaoService StatusPedidoValidacaoService;

        public PedidoFixture()
        {
            var validacoes = new List<IStatusPedidoValidacao>
            {
                new StatusPedidoCriadoValidacao(),
                new StatusPedidoCanceladoValidacao(),
                new StatusPedidoCanceladoPorPagamentoValidacao(),
                new StatusPedidoEmPreparacaoValidacao(),
                new StatusPedidoDescartadoValidacao(),
                new StatusPedidoFinalizadoValidacao(),
                new StatusPedidoProntoValidacao(),
                new StatusPedidoRecebidoValidacao(),
                new StatusPedidoRetiradoValidacao()
            };

            StatusPedidoValidacaoService = new StatusPedidoValidacaoService(validacoes);
        }

        public Pedido GerarPedidoValido()
        {
            return new Pedido(new Cpf(Constants.CPF_USER_DEFAULT), new List<ItemPedido> { new ItemPedido(1, 1, 1) });
        }

        public List<Pedido> GerarPedidosValidos()
        {
            return new List<Pedido> { new Pedido(new Cpf(Constants.CPF_USER_DEFAULT), new List<ItemPedido> { new ItemPedido(1, 1, 1) }) };
        }

        public Pedido GerarPedidoSemClienteValido()
        {
            return new Pedido(null, new List<ItemPedido> { new ItemPedido(1, 1, 1) });
        }

        public List<Pedido> GerarPedidosSemClientesValidos()
        {
            return new List<Pedido> { new Pedido(null, new List<ItemPedido> { new ItemPedido(1, 1, 1) }) };
        }

        public ItemPedido GerarItemPedidoValido()
        {
            return new ItemPedido(1, 1, 1);
        }

        public List<ItemPedido> GerarItensPedidoValidos()
        {
            return new List<ItemPedido> { new ItemPedido(1, 1, 1) };
        }

        public void Dispose()
        {
        }
    }
}