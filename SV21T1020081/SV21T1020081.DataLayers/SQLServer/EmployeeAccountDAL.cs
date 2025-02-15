using Dapper;
using SV21T1020081.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020081.DataLayers.SQLServer
{
    public class EmployeeAccountDAL : BaseDAL, IUserAccountDAL
    {
        public EmployeeAccountDAL(string connectionString) : base(connectionString)
        {
        }

        public UserAccount? Authorize(string username, string password)
        {
            UserAccount? data = null;

            using (var connection = OpenConnection())

            {

                var sql = @"SELECT EmployeeID AS UserId,
                            Email AS UserName,
                            FullName AS DisplayName,
                            Photo,
                            RoleNames
                            FROM
                            Employees WHERE Email = @Email AND Password = @Password";

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
                var sql = @"update Employees 
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
