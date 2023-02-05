using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NSE.Identidade.API.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        //public ApplicationDbContext() : base(GetOptions()) { }

        //"Server=localhost,49161;Database=NerdStoreEnterprise;User Id=sa; Password=pdx#32R2;" 

        private static string strconn = "Server=localhost,49161;Database=NerdStoreEnterprise;User Id=sa; Password=pdx#32R2;";

        private static DbContextOptions GetOptions()
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), strconn).Options;
        }

    }
}
