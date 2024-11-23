using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioFind.Data.Models
{
    public class ShelfModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BookModel> Books { get; set; }
    }
}
