using EntityFrameWorkModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkModule.IServices
{
    public interface IAccountServices
    {
        bool Login(User model);
    }
}
