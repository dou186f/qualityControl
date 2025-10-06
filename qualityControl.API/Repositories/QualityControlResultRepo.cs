using Microsoft.Data.SqlClient;
using qualityControl.API.Data;
using qualityControl.SHARED.Dtos;
using qualityControl.SHARED.Interfaces;
using System.Data;

namespace qualityControl.API.Repositories
{
    public sealed class QualityControlResultRepo : IQualityControlResultRepo
    {
        private readonly IDbConnectionFactory _factory;
        public QualityControlResultRepo(IDbConnectionFactory factory) => _factory = factory;

        public async Task<int> UpsertAsync(QualityControlResult dto)
        {
            const string sql = @"
                            MERGE dbo.QualityControlResult AS target
                            USING (SELECT @wo AS WorkOrderRef, @qc AS QcRef) AS source
                            ON (target.WorkOrderRef = source.WorkOrderRef AND target.QcRef = source.QcRef)
                            WHEN MATCHED THEN
                                UPDATE SET
                                    Result = @res,
                                    Name = @name,
                                    SetRef = @set,
                                    CreatedAt = SYSUTCDATETIME()
                            WHEN NOT MATCHED THEN
                                INSERT (WorkOrderRef, QcRef, SetRef, Name, Result, CreatedAt)
                                VALUES (@wo, @qc, @set, @name, @res, SYSUTCDATETIME())
                            OUTPUT INSERTED.LogicalRef;";

            using var connection = _factory.CreateWrite();
            await connection.OpenAsync();
            
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@wo",  dto.WorkOrderRef);
            command.Parameters.AddWithValue("@qc",  dto.QcRef);
            command.Parameters.AddWithValue("@name", (object?)dto.Name ?? DBNull.Value);
            command.Parameters.AddWithValue("@set", (object?)dto.SetRef ?? DBNull.Value);
            command.Parameters.AddWithValue("@res", dto.Result);

            var idObj = await command.ExecuteScalarAsync();
            return Convert.ToInt32(idObj);
        }

        public async Task<Dictionary<int,(bool Result, int LogicalRef)>> GetResultsMapAsync(int workOrderRef)
        {
            const string sql = @"
                SELECT QcRef, Result, LogicalRef
                FROM dbo.QualityControlResult
                WHERE WorkOrderRef = @wo;";

            using var connection = _factory.CreateWrite();
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@wo", workOrderRef);

            var map = new Dictionary<int,(bool, int)>();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var qcRef = reader.GetInt32(reader.GetOrdinal("QcRef"));
                var res   = reader.GetBoolean(reader.GetOrdinal("Result"));
                var id    = reader.GetInt32(reader.GetOrdinal("LogicalRef"));
                map[qcRef] = (res, id);
            }
            return map;
        }

        public async Task<QualityControlResult?> GetQCResultAsync(int logref)
        {
            const string sql = @"
                        SELECT LogicalRef, WorkOrderRef, QcRef, SetRef, Name, Result, CreatedAt
                        FROM dbo.QualityControlResult
                        WHERE LogicalRef = @logref;";

            using var connection = _factory.CreateWrite();
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@logref", logref);

            using var reader =await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new QualityControlResult
            {
                LogicalRef = reader.GetInt32(reader.GetOrdinal("LogicalRef")),
                WorkOrderRef = reader.GetInt32(reader.GetOrdinal("WorkOrderRef")),
                QcRef = reader.GetInt32(reader.GetOrdinal("QcRef")),
                SetRef = reader.IsDBNull(reader.GetOrdinal("SetRef")) ? null : reader.GetInt32(reader.GetOrdinal("SetRef")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                Result = reader.GetBoolean(reader.GetOrdinal("Result"))
            };
        }
    }
}
