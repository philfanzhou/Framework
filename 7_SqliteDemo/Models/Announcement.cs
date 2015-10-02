using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework
{
    [Table("Announcement")]
    public class Announcement
    {
        public Announcement()
        {
            CreateTime = DateTime.Now;
        }

        [Key]
        [Display(Name = "公告ID")]
        public Int64 AnnouncementId { set; get; }

        [Display(Name = "发布人")]
        public int? MemberId { set; get; }

        [Display(Name = "公告标题")]
        [Required(ErrorMessage = "公告标题必须填写")]
        [StringLength(100, ErrorMessage = "公告标题必须小于100个字", MinimumLength = 2)]
        public string Title { set; get; }

        [Display(Name = "公告内容")]
        [Required(ErrorMessage = "公告内容必须填写")]
        public string Content { set; get; }

        [Display(Name = "发布时间")]
        public DateTime CreateTime { set; get; }
    }


}
