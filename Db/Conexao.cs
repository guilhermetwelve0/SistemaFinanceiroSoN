using System;

namespace Db
{
    public class Conexao
    {
        private static readonly string server = "LAPTOP-65P1PT7M\\SQLEXPRESS";
        private static readonly string database = "Son_Financeiro";
        private static readonly string username = "sa";
        private static readonly string password = "112233";

        public static string GetSringConnection() => $"Server={server};Database={database};User Id={username};Password={password}";

    }
}
