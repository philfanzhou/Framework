using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework
{
    public class PitmanContext : DbContext
    {
        public PitmanContext()
            : base("Pitman")
        {
            //数据库不存在时重新创建数据库
            Database.SetInitializer<PitmanContext>(new CreateDatabaseIfNotExists<PitmanContext>());

            //每次启动应用程序时创建数据库
            //Database.SetInitializer<PitmanContext>(new DropCreateDatabaseAlways<PitmanContext>());

            //模型更改时重新创建数据库
            //Database.SetInitializer<PitmanContext>(new DropCreateDatabaseIfModelChanges<PitmanContext>());

            //从不创建数据库
            //Database.SetInitializer<PitmanContext>(null);
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Announcement> Announcement { get; set; }
    }
}
