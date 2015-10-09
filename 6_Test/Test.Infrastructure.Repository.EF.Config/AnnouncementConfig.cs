using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Infrastructure.Repository.EF.Metadata;

namespace Test.Infrastructure.Repository.EF
{
    internal class AnnouncementConfig : EntityTypeConfiguration<Announcement>
    {
        public AnnouncementConfig()
        {
            this.HasKey(a => a.AnnouncementId);

            this.Property(a => a.Content)
                .HasMaxLength(100);

            this.Property(a => a.CreateTime)
                .IsRequired();

            this.Property(a => a.MemberId)
                .IsRequired();

            this.Property(a => a.Title)
                .HasMaxLength(10);
        }
    }
}
