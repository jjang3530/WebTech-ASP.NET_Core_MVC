﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JJOEC.Models.MetadataClasses
{
    [ModelMetadataType(typeof(JJFarmMetadata))]
    public partial class JJFarm
    {

    }

    public class JJFarmMetadata
    {
        public int FarmId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string ProvinceCode { get; set; }
        public string PostalCode { get; set; }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Directions { get; set; }
        public DateTime? DateJoined { get; set; }
        public DateTime? LastContactDate { get; set; }

        public Province ProvinceCodeNavigation { get; set; }
        public ICollection<Plot> Plot { get; set; }
    }
}