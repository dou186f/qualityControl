using Microsoft.Data.SqlClient;
using qualityControl.API.Data;
using qualityControl.SHARED.Interfaces;
using qualityControl.SHARED.Models;
using System.Data;

namespace qualityControl.API.Repositories
{
    public class ItemRepo : IItemRepo
    {
        private readonly IDbConnectionFactory _factory;
        public ItemRepo(IDbConnectionFactory factory) => _factory = factory;

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            const string sql = "SELECT LOGICALREF, CODE, NAME, QCCSETREF FROM LG_112_ITEMS ORDER BY LOGICALREF DESC";
            using var conn = _factory.CreateRead();
            await conn.OpenAsync();
            using var command = new SqlCommand(sql, conn);

            var list = new List<Item>();
            using var reader = await command.ExecuteReaderAsync();
            while  (await reader.ReadAsync())
            {
                list.Add(new Item
                {
                    LogicalRef = reader.GetInt32(reader.GetOrdinal("LOGICALREF")),
                    Code = reader.GetString(reader.GetOrdinal("CODE")),
                    Name = reader.GetString(reader.GetOrdinal("NAME")),
                    QccSetRef = reader.GetInt32(reader.GetOrdinal("QCCSETREF"))
                });
            }
            return list;
        }

        public async Task<Item?> GetItemByIdAsync(int logref)
        {
            const string sql = "SELECT LOGICALREF, CODE, NAME, QCCSETREF FROM LG_112_ITEMS WHERE LOGICALREF=@logref";
            using var conn = _factory.CreateRead();
            await conn.OpenAsync();
            using var command = new SqlCommand(sql, conn);
            command.Parameters.Add(new SqlParameter("@logref", SqlDbType.Int) { Value = logref });

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Item
                {
                    LogicalRef = reader.GetInt32(reader.GetOrdinal("LOGICALREF")),
                    Code       = reader.GetString(reader.GetOrdinal("CODE")),
                    Name       = reader.GetString(reader.GetOrdinal("NAME")),
                    QccSetRef  = reader.GetInt32(reader.GetOrdinal("QCCSETREF"))
                };
            }
            return null;
        }

        public async Task<IEnumerable<Item>> SearchItemAsync(string? query)
        {
            var wantedItems = new List<Item>();

            using var connection = _factory.CreateRead();
            await connection.OpenAsync();

            using var command = connection.CreateCommand();
            command.CommandText = @"SELECT LOGICALREF, CODE, NAME, QCCSETREF FROM LG_112_ITEMS 
                                    WHERE (@query IS NULL OR NAME LIKE '%' + @query + '%'
                                                          OR CODE LIKE '%' + @query + '%') ORDER BY LOGICALREF DESC;";
            command.Parameters.AddWithValue("@query", (object?)query ?? DBNull.Value);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                wantedItems.Add(new Item
                {
                    LogicalRef = reader.GetInt32(reader.GetOrdinal("LOGICALREF")),
                    Code = reader.GetString(reader.GetOrdinal("CODE")),
                    Name = reader.GetString(reader.GetOrdinal("NAME")),
                    QccSetRef = reader.GetInt32(reader.GetOrdinal("QCCSETREF"))
                });
            }
            return wantedItems;
        }
    }
}
