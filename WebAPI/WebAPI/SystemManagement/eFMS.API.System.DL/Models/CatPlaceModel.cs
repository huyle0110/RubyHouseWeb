using eFMS.API.System.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eFMS.API.System.DL.Models
{
    public class CatPlaceModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string DisplayName { get; set; }
        public string Address { get; set; }
        public Guid? DistrictId { get; set; }
        public Guid? ProvinceId { get; set; }
        public int? CountryId { get; set; }
        public string AreaId { get; set; }
        public string LocalAreaId { get; set; }
        public string ModeOfTransport { get; set; }
        public string GeoCode { get; set; }
        public string PlaceTypeId { get; set; }
        public string Note { get; set; }
        public string UserCreated { get; set; }
        public DateTime? DatetimeCreated { get; set; }
        public string UserModified { get; set; }
        public DateTime? DatetimeModified { get; set; }
        public bool? Inactive { get; set; }
        public DateTime? InactiveOn { get; set; }
        public List<string> convert2Mode()
        {
            if (!string.IsNullOrEmpty(this.ModeOfTransport))
            {
                return this.ModeOfTransport.Split(';').ToList();
            }
            return new List<string>();
        }
    }
}
