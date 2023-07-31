using System.ComponentModel.DataAnnotations;

//Classe qui étend la classe Book pour rajouter les traductions.
namespace MvcApplication3_mvc4
{
    [MetadataType(typeof(BookExtend))]
    public partial class Book
    {
    }

    public class BookExtend
    {
        [Display(Name = "Name", ResourceType = typeof(Resources.Resources))]
        public global::System.String name;

        [Display(Name = "Note", ResourceType = typeof(Resources.Resources))]
        public global::System.String note;
    }
}