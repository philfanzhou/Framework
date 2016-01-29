using System.Data.Entity.ModelConfiguration;
using Test.Infrastructure.Repository.EF.Metadata;

namespace Test.Infrastructure.Repository.EF
{
    internal class PersonConfig : EntityTypeConfiguration<Person>
    {
        public PersonConfig()
        {
            this.HasKey(person => person.Birthday);

            this.Property(person => person.Id)
                .IsRequired();

            this.Property(person => person.FirstName)
                .HasMaxLength(20)
                .IsRequired();

            this.Property(person => person.LastName)
                .HasMaxLength(20)
                .IsRequired();

            this.Property(person => person.Age)
                .IsRequired();

            this.Property(person => person.Salary)
                .IsRequired();

            this.Property(person => person.Address)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
