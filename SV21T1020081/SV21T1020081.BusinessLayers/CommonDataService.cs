using SV21T1020081.DataLayers;
using SV21T1020081.DomainModels;

namespace SV21T1020081.BusinessLayers
{
    public class CommonDataService
    {
        public static readonly ICommonDAL<Customer> customerDB;
        public static readonly ICommonDAL<Shipper> shipperDB;
        public static readonly ICommonDAL<Employee> employeeDB;
        public static readonly ICommonDAL<Supplier> supplierDB;
        public static readonly ICommonDAL<Category> categoryDB;

        public static readonly ISimpleDAL<Province> provinceDB;

        static CommonDataService()
        {
            string connectionString = Configuration.ConnectionString;

            customerDB = new DataLayers.SQLServer.CustomerDAL(connectionString);
            shipperDB = new DataLayers.SQLServer.ShipperDAL(connectionString);
            employeeDB = new DataLayers.SQLServer.EmployeeDAL(connectionString);
            supplierDB = new DataLayers.SQLServer.SupplierDAL(connectionString);
            categoryDB = new DataLayers.SQLServer.CategoryDAL(connectionString);

            provinceDB= new DataLayers.SQLServer.ProvinceDAL(connectionString);
        }
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            //rowCount = customerDB.Count(searchValue);
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue);
        }

        public static int AddCustommer(Customer data)
        {
            return customerDB.Add(data);
        }
        public static Customer? GetCustomer(int id)
        {
            return customerDB.Get(id);  
        }
        public static bool UpdateCustomer(Customer data)
        {
            return customerDB.Update(data);
        }
        public static bool DeleteCustomer(int id)
        {
            if (customerDB.InUsed(id))
                return false;
            return customerDB.Delete(id);
        }
        public static bool InUsedCustomer(int id)
        {
            return customerDB.InUsed(id);
        }
        public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            //rowCount = customerDB.Count(searchValue);
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue);
        }
        public static int AddShipper(Shipper data)
        {
            return shipperDB.Add(data);
        }
        public static Shipper? GetShipper(int id)
        {
            return shipperDB.Get(id);
        }
        public static bool UpdateShipper(Shipper data)
        {
            return shipperDB.Update(data);
        }
        public static bool DeleteShipper(int id)
        {
            if (shipperDB.InUsed(id))
                return false;
            return shipperDB.Delete(id);
        }
        public static bool InUsedShipper(int id)
        {
            return shipperDB.InUsed(id);
        }
        public static List<Employee> ListOfEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
 
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue);
        }
        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }
        public static Employee? GetEmployee(int id)
        {
            return employeeDB.Get(id);
        }
        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }
        public static bool DeleteEmployee(int id)
        {
            if (employeeDB.InUsed(id))
                return false;
            return employeeDB.Delete(id);
        }
        public static bool InUsedEmployee(int id)
        {
            return employeeDB.InUsed(id);
        }
        public static List<Supplier> ListOfSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {

            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue);
        }
        public static int AddSupplier(Supplier data)
        {
            return supplierDB.Add(data);
        }
        public static Supplier? GetSupplier(int id)
        {
            return supplierDB.Get(id);
        }
        public static bool UpdateSupplier(Supplier data)
        {
            return supplierDB.Update(data);
        }
        public static bool DeleteSupplier(int id)
        {
            if (supplierDB.InUsed(id))
                return false;
            return supplierDB.Delete(id);
        }
        public static bool InUsedSupplier(int id)
        {
            return supplierDB.InUsed(id);
        }


        public static List<Category> ListOfCategories(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {

            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue);
        }
        public static int AddCategory(Category data)
        {
            return categoryDB.Add(data);
        }
        public static Category? GetCategory(int id)
        {
            return categoryDB.Get(id);
        }
        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }
        public static bool DeleteCategory(int id)
        {
            if (categoryDB.InUsed(id))
                return false;
            return categoryDB.Delete(id);
        }
        public static bool InUsedCategory(int id)
        {
            return categoryDB.InUsed(id);
        }
        public static List<Province> ListOfProvines()
        {


            return provinceDB.List();
        }
        public static List<Category> ListCategory()
        {


            return categoryDB.List();
        }
        public static List<Supplier> ListSupplier()
        {


            return supplierDB.List();
        }
        public static List<Customer> ListOfCustomer()
        {


            return customerDB.List();
        }
        public static List<Shipper> ListShipper()
        {


            return shipperDB.List();
        }
    }
}
