using SV21T1020081.DataLayers;
using SV21T1020081.DomainModels;

namespace SV21T1020081.BusinessLayers
{
    public static class UserAccountService
    {
        public static readonly IUserAccountDAL employeeAcountDB;
        public static readonly IUserAccountDAL customerAcountDB;

        static UserAccountService()
        {
            string connectionString =Configuration.ConnectionString;
            employeeAcountDB=new DataLayers.SQLServer.EmployeeAccountDAL(connectionString);
            customerAcountDB = new DataLayers.SQLServer.CustomerAccountDAL(connectionString);

        }
        public static UserAccount ? Authorize(UserType userType, string username,string password)
        {
            if (userType==UserType.Employee)
                return employeeAcountDB.Authorize(username, password);
            else
                return customerAcountDB.Authorize(username,password);
            

        }
        public static bool ChangePassword(UserType userType, string username, string password)
        {
            if (userType == UserType.Employee)
                return employeeAcountDB.ChangePassword(username, password);
            else
                return customerAcountDB.ChangePassword(username,password);
            
        }

    }

    public enum UserType
    {
        Employee,
        Customer
    }
}
