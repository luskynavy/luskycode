using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace MvcApplication3_mvc4
{
    [MetadataType(typeof(BookExtend))]
    public partial class Book
    {
    }

    class BookExtend
    {
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        public global::System.String name;

        [Display(Name = "Note", ResourceType = typeof(Resources.Resources))]
        public global::System.String note;
    }
}