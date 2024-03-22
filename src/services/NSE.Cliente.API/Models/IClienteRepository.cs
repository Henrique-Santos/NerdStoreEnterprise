using NSE.Core.Data;

namespace NSE.Clientes.API.Models
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        void Adicionar(Cliente cliente);

        void AdicionarEndereco(Endereco endereco);

        Task<IEnumerable<Cliente>> ObterTodos();

        Task<Cliente> ObterPorCpf(string cpf);

        Task<Endereco> ObterEnderecoPorId(Guid id);
    }
}