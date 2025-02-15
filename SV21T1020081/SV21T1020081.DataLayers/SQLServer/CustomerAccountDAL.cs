using Dapper;
using SV21T1020081.DomainModels;

namespace SV21T1020081.DataLayers.SQLServer
{
    public class CustomerAccountDAL : BaseDAL, IUserAccountDAL
    {
        public CustomerAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount? Authorize(string username, string password)
        {
            UserAccount? data = null;

            using (var connection = OpenConnection())

            {

                var sql = @"SELECT CustomerID AS UserId,
                            Email AS UserName,
                            CustomerName AS DisplayName
                            FROM
                            Customers WHERE Email = @Email AND Password = @Password";

                var parameters = new
                {
                    Email = username,

                    Password = password

                };

                data = connection.QueryFirstOrDefault<UserAccount>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
            }
            return data;
        }

        public bool ChangePassword(string username, string password)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"update Customers 
                           set Password = @Password
                           where Email =@Email 
					";
                var parameters = new
                {
                    Email = username,
                    Password = password
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
      
    }
}
