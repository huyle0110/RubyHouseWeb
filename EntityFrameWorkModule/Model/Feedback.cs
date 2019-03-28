using EntityFrameWorkModule.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkModule.Model
{
    public class Feedback
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public string Content { get; set; }
        public DateTime? CreateDate { get; set; }
        public FeedbackStatus Read { get; set; }
    }
}
