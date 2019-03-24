using RubyHouseServices.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyHouseServices.DAO
{
    public class UserDAO
    {
        RubyHouseDbContext db = null;
        public UserDAO()
        {
            db = new RubyHouseDbContext();
        }

        public long Insert(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user.ID;
        }
    }
}
