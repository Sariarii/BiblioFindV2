using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioFind.Data.Models
{
    public class MemberModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }  // Prénom du membre
        public string LastName { get; set; }   // Nom du membre
        public string FullName => $"{FirstName} {LastName}";  // Propriété calculée
    }


}
