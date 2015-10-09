using System;

namespace Test.Infrastructure.Repository.EF.Metadata
{
    public class Announcement
    {
        public int AnnouncementId { set; get; }

        public int MemberId { set; get; }

        public string Title { set; get; }

        public string Content { set; get; }

        public DateTime CreateTime { set; get; }
    }
}
