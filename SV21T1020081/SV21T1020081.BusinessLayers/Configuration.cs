namespace SV21T1020081.BusinessLayers
{
    public static class Configuration
    {
        private static string connectionString = "";

        public static void Initalize(string connectionString)
        {
            Configuration.connectionString = connectionString;
        }
        public static string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }
    }
}
