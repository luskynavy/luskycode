using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace MvcApplication3_mvc4.Models
{
    public class SearchBook
    {
        //Current name filter
        public string NameSearch { get; set; }

        //Current Page number
        public int Page { get; set; }

        //List of books
        //public List<Book> BooksList { get; set; }
        public IPagedList<Book> BooksList { get; set; }
    }
}