using Microsoft.Data.SqlClient;
using qualityControl.API.Data;
using qualityControl.SHARED.Interfaces;
using qualityControl.SHARED.Models;
using System.Data;

namespace qualityControl.API.Repositories
{
    public class QualityControlRepo : IQualityControlRepo
    {
        private readonly IDbConnectionFactory _factory;
        public QualityControlRepo(IDbConnectionFactory factory) => _factory = factory;

        public async Task<IEnumerable<QualityControl>> GetAllAsync()
        {
            const string sql = @"SELECT 
                                LOGICALREF, 
                                NAME, 
                                SETREF, 
                                MINVAL, 
                                MAXVAL
                                FROM LG_112_QCSLINE ORDER BY LOGICALREF DESC;";
            using var connection = _factory.CreateRead();
            await connection.OpenAsync();
            using var command = new SqlCommand(sql, connection);

            var list = new List<QualityControl>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new QualityControl
                {
                    LogicalRef = reader.GetInt32(reader.GetOrdinal("LOGICALREF")),
                    Name = reader.GetString(reader.GetOrdinal("NAME")),
                    SetRef = reader.GetInt32(reader.GetOrdinal("SETREF")),
                    MinVal = reader.GetDouble(reader.GetOrdinal("MINVAL")),
                    MaxVal = reader.GetDouble(reader.GetOrdinal("MAXVAL"))
                });
            }
            return list;
        }

        public async Task<IEnumerable<QualityControl>> GetBySetRefAsync(int? setref)
        {
            const string sql = @"SELECT LOGICALREF, NAME, SETREF, MINVAL, MAXVAL
                                 FROM LG_112_QCSLINE
                                 WHERE SETREF = @setref
                                 ORDER BY LOGICALREF DESC;";
            using var connection = _factory.CreateRead();
            await connection.OpenAsync();
            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@setref", SqlDbType.Int){ Value = setref });

            var list = new List<QualityControl>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(new QualityControl
                {
                    LogicalRef = reader.GetInt32(reader.GetOrdinal("LOGICALREF")),
                    Name       = reader.GetString(reader.GetOrdinal("NAME")),
                    SetRef     = reader.GetInt32(reader.GetOrdinal("SETREF")),
                    MinVal     = reader.GetDouble(reader.GetOrdinal("MINVAL")),
                    MaxVal     = reader.GetDouble(reader.GetOrdinal("MAXVAL"))
                });
            }
            return list;
        }

        public async Task<QualityControl?> GetQualityControlByIdAsync(int logref)
        {
            const string sql = "SELECT LOGICALREF, NAME, SETREF, MINVAL, MAXVAL FROM LG_112_QCSLINE WHERE LOGICALREF=@logref;";
            using var connection = _factory.CreateRead();
            await connection.OpenAsync();
            using var command = new SqlCommand(sql, connection);
            command.Parameters.Add(new SqlParameter("@logref", SqlDbType.Int) { Value = logref });

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new QualityControl
                {
                    LogicalRef = reader.GetInt32(reader.GetOrdinal("LOGICALREF")),
                    Name = reader.GetString(reader.GetOrdinal("NAME")),
                    SetRef = reader.GetInt32(reader.GetOrdinal("SETREF")),
                    MinVal = reader.GetDouble(reader.GetOrdinal("MINVAL")),
                    MaxVal = reader.GetDouble(reader.GetOrdinal("MAXVAL"))
                };
            }
            return null;
        }
    }
}
