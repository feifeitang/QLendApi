namespace restapi.Settings
{
    public class MssqlSettings
    {
        public string Host { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public string ConnectionString
        {
            get
            {
                return $"Server={Host},{Port};User Id={User};Password={Password};Database=QLendDB;";
            }
        }
    }
}