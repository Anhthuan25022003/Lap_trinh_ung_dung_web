using Microsoft.Data.SqlClient;

namespace SV21T1020081.DataLayers.SQLServer
{
    /// <summary>
    /// Lop co so (lop cha) cua cac lop cai dat cac phep xu li du lieu tren SQL Server
    /// </summary>
    public abstract class BaseDAL
    {
        protected string connectionString = "";
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public BaseDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        /// <summary>
        /// tao va ket noi den csdl
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
