namespace Framework.Infrastructure.Repository.EF.SQLite
{
    public class SQLiteConnectionConfig
    {
        public string Database { get; set; }

        public override string ToString()
        {
            return Database.ToString();
        }
    }
}
