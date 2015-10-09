using System.Data.Entity.ModelConfiguration;
using Test.Infrastructure.Repository.EF.Metadata;

namespace Test.Infrastructure.Repository.EF
{
    internal class PersonConfig : EntityTypeConfiguration<Person>
    {
        public PersonConfig()
        {
            this.HasKey(person => person.Id);

            this.Property(person => person.FirstName)
                .HasMaxLength(20)
                .IsRequired();

            this.Property(person => person.LastName)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
