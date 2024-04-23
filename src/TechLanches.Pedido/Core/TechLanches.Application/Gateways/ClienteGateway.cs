using TechLanches.Application.Gateways.Interfaces;
using TechLanches.Application.Ports.Repositories;
using TechLanches.Domain.Entities;
using TechLanches.Domain.ValueObjects;

namespace TechLanches.Application.Gateways
{
    public class ClienteGateway : IClienteGateway
    {

        public ClienteGateway()
        {
        }

        public Task<Cliente> BuscarPorCpf(Cpf cpf)
        {
            throw new NotImplementedException();
        }

        public Task<Cliente> BuscarPorId(int idCliente)
        {
            throw new NotImplementedException();
        }

        public Task<Cliente> Cadastrar(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync()
        {
            throw new NotImplementedException();
        }
    }
}