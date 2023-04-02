using Microsoft.EntityFrameworkCore;
using NSE.Core.Data;
using NSE.Pedidos.Dominio.Pedidos;

namespace NSE.Pedidos.Infra.Data.Repository
{
    internal class PedidoRepository : IPedidoRepository
    {
        public IUnitOfWork UnitOfWork => _context;
        private readonly PedidosContext _context;
        public PedidoRepository(PedidosContext context)
        {
            _context = context;
        }

        public void Adicionar(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
        }

        public void Atualizar(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<PedidoItem> ObterItemPorPedido(Guid pedidoId, Guid produtoId)
        {
            return await _context.PedidoItems
                .FirstOrDefaultAsync(px => px.ProdutoId == produtoId && px.PedidoId == pedidoId);
        }

        public async Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId)
        {
            return await _context.Pedidos
                .Include(px => px.PedidoItems)
                .AsNoTracking().Where(p => p.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<Pedido> ObterPorId(Guid id)
        {
            return await _context.Pedidos.FindAsync(id);
        }

        public async Task<PedidoItem> ObterItemPorId(Guid id)
        {
            return await _context.PedidoItems.FindAsync(id);
        }
    }
}
