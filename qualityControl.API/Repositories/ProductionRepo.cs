using System.Data;
using Microsoft.Data.SqlClient;
using qualityControl.API.Data;
using qualityControl.SHARED.Interfaces;
using qualityControl.SHARED.Models;

namespace qualityControl.API.Repositories
{
    public class ProductionRepo : IProductionRepo
    {
        private readonly IDbConnectionFactory _factory;
        public ProductionRepo(IDbConnectionFactory factory) => _factory = factory;

        public async Task<IEnumerable<Production>> GetAllProductionsAsync()
        {
            const string sql = "SELECT LOGICALREF, PLNAMOUNT, ACTAMOUNT FROM LG_112_PRODORD ORDER BY LOGICALREF DESC";
            using var conn = _factory.CreateRead();
            await conn.OpenAsync();
            using var cmd = new SqlCommand(sql, conn);

            var list = new List<Production>();
            using var reader = await cmd.ExecuteReaderAsync();
            while  (await reader.ReadAsync())
            {
                list.Add(new Production
                {
                    LogicalRef = reader.GetInt32(reader.GetOrdinal("LOGICALREF")),
                    Plnamount = reader.GetDouble(reader.GetOrdinal("PLNAMOUNT")),
                    Actamount = reader.GetDouble(reader.GetOrdinal("ACTAMOUNT"))
                });
            }
            return list;
        }

        public async Task<Production?> GetProductionByIdAsync(int logref)
        {
            const string sql = "SELECT LOGICALREF, PLNAMOUNT, ACTAMOUNT FROM LG_112_PRODORD WHERE LOGICALREF=@logref";
            using var conn = _factory.CreateRead();
            await conn.OpenAsync();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@logref", SqlDbType.Int) { Value = logref });

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Production
                {
                    LogicalRef = reader.GetInt32(reader.GetOrdinal("LOGICALREF")),
                    Plnamount = reader.GetFloat(reader.GetOrdinal("PLNAMOUNT")),
                    Actamount = reader.GetFloat(reader.GetOrdinal("ACTAMOUNT"))
                };
            }
            return null;
        }
    }
}
