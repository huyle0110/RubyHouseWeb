using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameWorkModule.Common
{
    public enum ProductStatus
    {
        Inactive,
        Active
    }
    public enum UserStatus
    {
        Inactive,
        Active
    }
    public enum FeedbackStatus
    {
        NotYet,
        Read
    }
    public enum UserType
    {
        Admin = 1,
        Staff = 2,
        Cust = 3
    }

    public enum levelCategory
    {
        Parent, Child
    }
}
