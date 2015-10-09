namespace Framework.Infrastructure.Repository.EF.SQLite
{
    public class SqliteConnectionConfig
    {
        public string Database { get; set; }

        public override string ToString()
        {
            return Database.ToString();
        }
    }
}
