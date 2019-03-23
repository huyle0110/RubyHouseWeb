using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyHouseServices.EF
{
    public class User
    {
        public long ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
