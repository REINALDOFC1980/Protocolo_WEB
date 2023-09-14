using Dapper;
using Microsoft.Data.SqlClient;
using Protocolo_web_adm.Service.IRepository;
using System.Data;

namespace Protocolo_web_adm.Service.Repository
{
    public class DapperServices : IDapperServices
    {

        private readonly IConfiguration _config;
        private string Connectionstring = "DefaultConnection";

        public DapperServices(IConfiguration config)
        {
            _config = config;
        }

        public int ExecuteScala<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return db.Execute(sp, param: parms, commandType: commandType);
        }

        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));

            return db.Query<T>(sp, param: parms, commandType: commandType).FirstOrDefault();
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            IDbConnection db = new SqlConnection(_config.GetConnectionString(Connectionstring));
            return db.Query<T>(sp, param: parms, commandType: commandType).ToList();
        }

    
    }
}
