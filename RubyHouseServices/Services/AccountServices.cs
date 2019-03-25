using RubyHouseServices.EF;
using RubyHouseServices.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubyHouseServices.Services
{
    public class AccountServices : IAccountServices
    {
        public bool Login(User model)
        {
            RubyHouseDbContext _context;
            _context = new RubyHouseDbContext();

            var result = _context.Users.Find(model);
            return result != null;
        }
    }
}
