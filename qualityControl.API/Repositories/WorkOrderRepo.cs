using System.Data;
using Microsoft.Data.SqlClient;
using qualityControl.API.Data;
using qualityControl.SHARED.Interfaces;
using qualityControl.SHARED.Dtos;

namespace qualityControl.API.Repositories
{
    public class WorkOrderRepo : IWorkOrderRepo
    {
        private readonly IDbConnectionFactory _factory;
        public WorkOrderRepo(IDbConnectionFactory factory) => _factory = factory;

        public async Task<IEnumerable<WorkOrderDto>> GetAllWorkOrdersAsync()
        {
            const string sql = @"
                            SELECT
                            w.[LOGICALREF] AS LogicalRef, 
                            w.[LINENO_],
                            w.[OPBEGDATE]  AS OpBegDate, 
                            w.[PRODORDREF] AS Prodordref,
                            w.[ITEMREF]    AS Itemref,
                            i.[NAME]       AS ItemName,
                            i.[CODE]       AS ItemCode,
                            p.[ACTAMOUNT]  AS ProductionActamount,
                            p.[PLNAMOUNT]  AS ProductionPlnamount
                            FROM         [LG_112_DISPLINE] AS w
                            LEFT JOIN    [LG_112_ITEMS] AS i ON i.[LOGICALREF] = w.[ITEMREF]
                            LEFT JOIN    LG_112_PRODORD AS p ON p.[LOGICALREF] = w.[PRODORDREF]
                            ORDER BY w.[OPBEGDATE] DESC";

            using var conn = _factory.CreateRead();
            await conn.OpenAsync();
            using var cmd = new SqlCommand(sql, conn);

            var list = new List<WorkOrderDto>();
            using var reader = await cmd.ExecuteReaderAsync();
            while  (await reader.ReadAsync())
            {
                list.Add(new WorkOrderDto
                {
                    LogicalRef = reader.GetInt32(reader.GetOrdinal("LogicalRef")),
                    LineNo = reader.GetString(reader.GetOrdinal("LINENO_")),
                    OpBegDate = reader.GetDateTime(reader.GetOrdinal("OpBegDate")),
                    ProdordRef = reader.GetInt32(reader.GetOrdinal("Prodordref")),
                    ItemRef = reader.GetInt32(reader.GetOrdinal("Itemref")),
                    ItemName = reader.GetString(reader.GetOrdinal("ItemName")),
                    ItemCode = reader.GetString(reader.GetOrdinal("ItemCode")),
                    ProductionActamount = reader.GetDouble(reader.GetOrdinal("ProductionActamount")),
                    ProductionPlnamount = reader.GetDouble(reader.GetOrdinal("ProductionPlnamount"))
                });
            }
            return list;
        }

        public async Task<WorkOrderDto?> GetWorkOrderByIdAsync(int logref)
        {
            const string sql = @"
                            SELECT
                            w.[LOGICALREF] AS LogicalRef, 
                            w.[LINENO_], 
                            w.[OPBEGDATE]  AS OpBegDate, 
                            w.[PRODORDREF] AS Prodordref,
                            w.[ITEMREF]    AS Itemref,
                            i.[NAME]       AS ItemName,
                            i.[CODE]       AS ItemCode,
                            p.[ACTAMOUNT]  AS ProductionActamount,
                            p.[PLNAMOUNT]  AS ProductionPlnamount
                            FROM         [LG_112_DISPLINE] AS w
                            LEFT JOIN    [LG_112_ITEMS] AS i ON i.[LOGICALREF] = w.[ITEMREF]
                            LEFT JOIN    LG_112_PRODORD AS p ON p.[LOGICALREF] = w.[PRODORDREF]
                            WHERE w.LOGICALREF = @logref";

            using var conn = _factory.CreateRead();
            await conn.OpenAsync();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add(new SqlParameter("@logref", SqlDbType.Int) { Value = logref });

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new WorkOrderDto
                {
                    LogicalRef = reader.GetInt32(reader.GetOrdinal("LogicalRef")),
                    LineNo = reader.GetString(reader.GetOrdinal("LINENO_")),
                    OpBegDate = reader.GetDateTime(reader.GetOrdinal("OpBegDate")),
                    ProdordRef = reader.GetInt32(reader.GetOrdinal("Prodordref")),
                    ItemRef = reader.GetInt32(reader.GetOrdinal("Itemref")),
                    ItemName = reader.GetString(reader.GetOrdinal("ItemName")),
                    ItemCode = reader.GetString(reader.GetOrdinal("ItemCode")),
                    ProductionActamount = reader.GetDouble(reader.GetOrdinal("ProductionActamount")),
                    ProductionPlnamount = reader.GetDouble(reader.GetOrdinal("ProductionPlnamount"))
                };
            }
            return null;
        }

        public async Task<IEnumerable<WorkOrderDto>> SearchWorkOrderAsync(string? query, bool onlyFinished = false, bool onlyNotFinished = false)
        {
            var wantedWorkOrders = new List<WorkOrderDto>();

            using var connection = _factory.CreateRead();
            await connection.OpenAsync();

            var q = string.IsNullOrWhiteSpace(query) ? null : query.Trim();

            var hasInt = int.TryParse(q, out var qInt);
            var hasDate = DateTime.TryParse(q, out var qDate);

            using var command = connection.CreateCommand();
            command.CommandText = @"
                            SELECT
                            w.[LOGICALREF] AS LogicalRef, 
                            w.[LINENO_], 
                            w.[OPBEGDATE]  AS OpBegDate, 
                            w.[PRODORDREF] AS Prodordref,
                            w.[ITEMREF]    AS Itemref,
                            i.[NAME]       AS ItemName,
                            i.[CODE]       AS ItemCode,
                            p.[ACTAMOUNT]  AS ProductionActamount,
                            p.[PLNAMOUNT]  AS ProductionPlnamount
                            FROM         [LG_112_DISPLINE] AS w
                            LEFT JOIN    [LG_112_ITEMS] AS i ON i.[LOGICALREF] = w.[ITEMREF]
                            LEFT JOIN    LG_112_PRODORD AS p ON p.[LOGICALREF] = w.[PRODORDREF]
                            WHERE ((@q IS NULL     OR i.[NAME] COLLATE Latin1_General_CI_AI LIKE '%' + @q + '%'
                                                  OR i.[CODE] COLLATE Latin1_General_CI_AI LIKE '%' + @q + '%'
                                                  OR w.[LINENO_] LIKE '%' + @q + '%')
                                                  OR (@qDate IS NOT NULL AND CAST(w.[OPBEGDATE] AS date) = CAST(@qDate AS date)))
                                                  AND (@onlyFinished = 0 OR (p.[ACTAMOUNT] IS NOT NULL
                                                                         AND p.[PLNAMOUNT] IS NOT NULL
                                                                         AND ABS(p.[ACTAMOUNT] - p.[PLNAMOUNT]) < 0.00001))
                                                  AND (@onlyNotFinished = 0 OR (p.[ACTAMOUNT] IS NOT NULL
                                                                            AND p.[PLNAMOUNT] IS NOT NULL
                                                                            AND ABS(p.[ACTAMOUNT] - p.[PLNAMOUNT]) > 0.00001))
                                                  ORDER BY w.[OPBEGDATE] DESC;";

            var pQ = command.CreateParameter();
            pQ.ParameterName = "@q";
            pQ.DbType = DbType.String;
            pQ.Value = (object?)q ?? DBNull.Value;
            command.Parameters.Add(pQ);

            var pDate = command.CreateParameter();
            pDate.ParameterName = "@qDate";
            pDate.DbType = DbType.DateTime;
            pDate.Value = hasDate ? qDate : DBNull.Value;
            command.Parameters.Add(pDate);

            var pFinished = command.CreateParameter();
            pFinished.ParameterName = "@onlyFinished";
            pFinished.DbType = DbType.Boolean;
            pFinished.Value = onlyFinished;
            command.Parameters.Add(pFinished);

            var pNotFinished = command.CreateParameter();
            pNotFinished.ParameterName = "@onlyNotFinished";
            pNotFinished.DbType = DbType.Boolean;
            pNotFinished.Value = onlyNotFinished;
            command.Parameters.Add(pNotFinished);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                wantedWorkOrders.Add(new WorkOrderDto
                {
                    LogicalRef = reader.GetInt32(reader.GetOrdinal("LogicalRef")),
                    LineNo = reader.GetString(reader.GetOrdinal("LINENO_")),
                    OpBegDate = reader.GetDateTime(reader.GetOrdinal("OpBegDate")),
                    ProdordRef = reader.GetInt32(reader.GetOrdinal("Prodordref")),
                    ItemRef = reader.GetInt32(reader.GetOrdinal("Itemref")),
                    ItemName = reader.GetString(reader.GetOrdinal("ItemName")),
                    ItemCode = reader.GetString(reader.GetOrdinal("ItemCode")),
                    ProductionActamount = reader.GetDouble(reader.GetOrdinal("ProductionActamount")),
                    ProductionPlnamount = reader.GetDouble(reader.GetOrdinal("ProductionPlnamount"))
                });
            }
            return wantedWorkOrders;
        }
    }
}
