using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eFMS.API.System.DL.Models
{
    public class CatCommodityModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Local Description is required")]
        [MaxLength(250, ErrorMessage = "Local Description must be between 1 to 250")]
        public string CommodityNameVn { get; set; }
        [Required(ErrorMessage = "English Description is required")]
        [MaxLength(250, ErrorMessage = "English Description must be between 1 to 250")]
        public string CommodityNameEn { get; set; }
        public string CommodityGroupName { get; set; }
        [Required]
        public short? CommodityGroupId { get; set; }
        public string Note { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DatetimeCreated { get; set; }
        public string UserModified { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public bool? Inactive { get; set; }
        public DateTime? InactiveOn { get; set; }
    }
}
