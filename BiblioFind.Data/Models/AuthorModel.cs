using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioFind.Data.Models
{

        public class AuthorModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string FirstName { get; set; }

            // Relation : Un auteur peut écrire plusieurs livres
            public ICollection<BookModel> Books { get; set; }
        }
    

}
