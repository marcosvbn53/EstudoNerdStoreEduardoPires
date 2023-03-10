using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Models;
using NSE.Catalogo.API.Models.Interfaces;
using NSE.Core.Data;

namespace NSE.Catalogo.API.Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {        
        private readonly CatalogoContext _catalogoContext;
        public IUnitOfWork UnitOfWork => _catalogoContext;
        

        public ProdutoRepository(CatalogoContext catalogoContext)
        {
            _catalogoContext = catalogoContext;
        }               

        public async Task<Produto> ObterPorId(Guid id)
        {
            return await _catalogoContext.Produtos.FindAsync(id);
        }

        public async Task<IEnumerable<Produto>> ObterTodos()
        {
            return await _catalogoContext.Produtos.AsNoTracking().ToListAsync();
        }

        public void Adicionar(Produto produto)
        {
            _catalogoContext.Produtos.Add(produto);
        }

        public void Atualizar(Produto produto)
        {
            _catalogoContext.Produtos.Update(produto);
        }
        public void Dispose()
        {
            _catalogoContext?.Dispose();
        }
    }
}
