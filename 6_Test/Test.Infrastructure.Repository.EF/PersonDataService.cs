using Framework.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Infrastructure.Repository.EF.Metadata;

namespace Test.Infrastructure.Repository.EF
{
    internal class PersonDataService
    {
        public static void Add(Person person)
        {
            using (IRepositoryContext context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<Repository<Person>>();
                repository.Add(person);

                context.UnitOfWork.Commit();
            }
        }

        public static Person GetPerson(int id)
        {
            using (IRepositoryContext context = RepositoryContext.Create())
            {
                var repository = context.GetRepository<Repository<Person>>();
                return repository.Get(id);
            }
        }
    }
}
