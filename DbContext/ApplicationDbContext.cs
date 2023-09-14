using Microsoft.EntityFrameworkCore;


namespace Protocolo_web_adm.DBContext
{

    namespace Protocolo_web_adm.DataContext
    {
        public class ApplicationDbContext : DbContext
        {
           public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        }
    }

}
