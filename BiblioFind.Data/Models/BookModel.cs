using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioFind.Data.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsBorrowed { get; set; }  // Indique si le livre est emprunté
        public int AuthorModelId { get; set; }
        public AuthorModel? Author { get; set; }
        public int? ShelfModelId { get; set; }  // L'ID du rayon
        public ShelfModel? Shelf { get; set; }  // La référence au rayon
    }
}
