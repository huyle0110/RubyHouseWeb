using eFMS.API.Common.Globals;
using eFMS.API.System.Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace eFMS.API.System.DL.Models
{
    public class CatPortModel
    {
        public Guid? Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Code { get; set; }
        public string PlaceTypeId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        public string Country { get; set; }

        [Required]
        public int CountryId { get; set; }
        public string Zone { get; set; }
        public string LocalZone { get; set; }

        [Required]
        public string ModeOfTransport { get; set; }

        public bool? Inactive { get; set; } // check if false=> show on front-end
        public DateTime? DatetimeCreated { get; set; }
        public DateTime? DatetimeModified { get; set; }
    }
}
