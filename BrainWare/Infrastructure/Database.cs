using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainWare.Infrastructure {
    using System.Configuration;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class Database {
        public async static Task<SqlConnection> GetConnection() {
            var cs = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            var c = new SqlConnection(cs);
            await c.OpenAsync();

            return c;
        }

        public static async Task ExecuteReader(SqlCommand Command, Action<SqlDataReader> ParseRow) {
            using (var r = await Command.ExecuteReaderAsync()) {
                while (await r.ReadAsync()) {
                    ParseRow(r);
                }
            }
        }

        public static async Task<int> ExecuteNonQuery(SqlCommand Command) {
            return await Command.ExecuteNonQueryAsync();
        }

        //public DbDataReader ExecuteReader(string query) {


        //    var sqlQuery = new SqlCommand(query, _connection);

        //    return sqlQuery.ExecuteReader();
        //}

        //public int ExecuteNonQuery(string query) {
        //    var sqlQuery = new SqlCommand(query, _connection);

        //    return sqlQuery.ExecuteNonQuery();
        //}

    }
}