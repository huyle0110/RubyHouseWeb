using eFMS.API.Common.Globals;
using System;
using System.Collections.Generic;
using System.Text;

namespace eFMS.API.System.DL.Models.Criteria
{
    public class PortIndexCriteria
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Zone { get; set; }
        public string Mode { get; set; }
        public string LocalZone { get; set; }
    }
}
