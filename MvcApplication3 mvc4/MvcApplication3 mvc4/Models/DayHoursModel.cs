using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication3_mvc4.Models
{
    public class DayHoursModel
    {
        //int id { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime ArrivalTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime LunchTime { get; set; }

        [DisplayFormat(DataFormatString="{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan DepartTime { get; set; }
    }
}