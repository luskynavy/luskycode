using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;
using TestSQL.Model; 

namespace TestSQL.Maps {

    public class BooksMap : ClassMap<Books> {
        
        public BooksMap() {
			Table("books");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Identity().Column("Id");
			Map(x => x.Name).Column("Name");
			Map(x => x.Author).Column("Author");
			Map(x => x.Note).Column("Note");
        }
    }
}
