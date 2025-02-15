using Azure;
using Dapper;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SV21T1020081.DomainModels;
using System.Buffers;

namespace SV21T1020081.DataLayers.SQLServer
{
    public class ProvinceDAL : BaseDAL, ISimpleDAL<Province>
    {
        public ProvinceDAL(string connectionString) : base(connectionString)
        {
        }

        public List<Province> List()
        {
            List<Province> data = new List<Province>();
            using (var connection = OpenConnection())
            {
                var sql = @"select *
                            from Provinces";
          
                data = connection.Query<Province>(sql: sql, commandType: System.Data.CommandType.Text).ToList();
            }
            return data;
        }
    }
}
