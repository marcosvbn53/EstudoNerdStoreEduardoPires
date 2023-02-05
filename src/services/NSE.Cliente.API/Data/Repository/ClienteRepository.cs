using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Models;
using NSE.Clientes.API.Models.Interfaces;
using NSE.Core.Data;

namespace NSE.Clientes.API.Data.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ClientesContext _context;

        public ClienteRepository(ClientesContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Cliente>> ObterTodos()
        {
            return await _context.Clientes.AsNoTracking().ToListAsync();
        }

        public Task<Cliente> ObterPorCpf(string cpf)
        {            
            var res = _context.Clientes.FirstOrDefault(px => px.Cpf.Numero == cpf);
            return Task.FromResult(res);
        }

        public void Adicionar(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
        }
        
        public void Dispose()
        {
            
        }
    }
}
