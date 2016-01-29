using System;

namespace Test.Infrastructure.Repository.EF.Metadata
{
    public class Person
    {
        public DateTime Birthday { get; set; }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }

        public decimal Salary { get; set; }

        public string Address { get; set; }
    }
}
