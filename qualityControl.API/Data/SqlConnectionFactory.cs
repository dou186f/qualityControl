using Microsoft.Data.SqlClient;

namespace qualityControl.API.Data
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateRead();
        SqlConnection CreateWrite();
    }

    public sealed class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _readCs;
        private readonly string _writeCs;   
        public SqlConnectionFactory(IConfiguration cfg)
        {
            _readCs = cfg.GetConnectionString("Read") ?? throw new InvalidOperationException("Connection string 'Read' not found.");
            _writeCs = cfg.GetConnectionString("Write") ?? _readCs;
        }
        public SqlConnection CreateRead() => new SqlConnection(_readCs);
        public SqlConnection CreateWrite() => new SqlConnection(_writeCs);
    }
}
