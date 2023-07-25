using System.ComponentModel.DataAnnotations;

namespace Watchlist.Models
{
    public class ModeleVueFilm
    {
        //NOTE : [Key] à rajouter par rapport au tuto
        [Key]
        public int IdFilm { get; set; }

        public string Titre { get; set; }
        public int Annee { get; set; }
        public bool PresentDansListe { get; set; }
        public bool Vu { get; set; }
        public int? Note { get; set; }
    }
}