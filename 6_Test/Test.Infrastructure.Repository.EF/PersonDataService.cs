using Framework.Infrastructure.Container;
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
            using (IRepositoryContext context = ContainerHelper.Resolve<IRepositoryContext>())
            {
                var repository = new Repository<Person>(context);
                repository.Add(person);

                repository.UnitOfWork.Commit();
            }
        }

        public static void AddRange(IEnumerable<Person> persons)
        {
            using (IRepositoryContext context = ContainerHelper.Resolve<IRepositoryContext>())
            {
                var repository = new Repository<Person>(context);
                repository.AddRange(persons);

                repository.UnitOfWork.Commit();
            }
        }

        public static IEnumerable<Person> GetAll()
        {
            using (IRepositoryContext context = ContainerHelper.Resolve<IRepositoryContext>())
            {
                var repository = new Repository<Person>(context);
                return repository.GetAll();
            }
        }

        public static Person GetPerson(int id)
        {
            using (IRepositoryContext context = ContainerHelper.Resolve<IRepositoryContext>())
            {
                var repository = new Repository<Person>(context);
                return repository.Get(id);
            }
        }
    }
}
