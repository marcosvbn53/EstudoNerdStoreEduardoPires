using NSE.Core.Data;

namespace NSE.Pedidos.Dominio.Vouchers
{
    public interface IVoucherRepository : IRepository<Voucher> 
    {
        Task<Voucher> ObterVoucherPorCodigo(string codigo);
    }
}
