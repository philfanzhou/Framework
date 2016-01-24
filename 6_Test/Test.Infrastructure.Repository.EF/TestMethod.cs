using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Test.Infrastructure.Repository.EF.Metadata;

namespace Test.Infrastructure.Repository.EF
{
    class TestMethod
    {
        public static void TestReadAndWrite()
        {
            Person expected = new Person
            {
                Id = 1,
                FirstName = "Hi",
                LastName = "Hello"
            };

            PersonDataService.Add(expected);

            var result = PersonDataService.GetAll();
            var actual = result.ToList()[0];

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.FirstName, actual.FirstName);
            Assert.AreEqual(expected.LastName, actual.LastName);


            actual = PersonDataService.GetPerson(expected.Id);

            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.FirstName, actual.FirstName);
            Assert.AreEqual(expected.LastName, actual.LastName);
        }

        public static void TestAddRange()
        {
            List<Person> personList = new List<Person>();
            personList.Add(new Person
            {
                Id = 2,
                FirstName = "Hi2",
                LastName = "Hello2"
            });
            personList.Add(new Person
            {
                Id = 3,
                FirstName = "Hi3",
                LastName = "Hello3"
            });

            PersonDataService.AddRange(personList);

            var result = PersonDataService.GetAll();
            var actual = result.ToList()[1];

            Assert.AreEqual(2, actual.Id);
        }
    }
}
