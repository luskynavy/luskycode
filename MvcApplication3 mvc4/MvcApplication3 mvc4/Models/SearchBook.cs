using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace MvcApplication3_mvc4.Models
{
    public class SearchBook
    {
        public string NameSearch { get; set; }
        public int Page { get; set; }
        //public List<Book> BooksList { get; set; }
        public IPagedList<Book> BooksList { get; set; }
    }
}