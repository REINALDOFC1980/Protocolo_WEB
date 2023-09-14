using Dapper;
using System.Data;

namespace Protocolo_web_adm.Service.IRepository
{
    public interface IDapperServices
    {
        T Get<T>(string sp, DynamicParameters? parms = null,
           CommandType commandType = CommandType.StoredProcedure);


        List<T> GetAll<T>(string sp, DynamicParameters? parms = null,
            CommandType commandType = CommandType.StoredProcedure);


        int ExecuteScala<T>(string sp, DynamicParameters? parms = null,
            CommandType commandType = CommandType.StoredProcedure);
        
    }
}
