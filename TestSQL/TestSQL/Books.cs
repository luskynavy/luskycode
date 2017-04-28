using System;
using System.Text;
using System.Collections.Generic;


namespace TestSQL.Model {
    
    public class Books {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Author { get; set; }
        public virtual int? Note { get; set; }
    }
}
