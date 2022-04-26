using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.Models
{
    public class CategoryBook
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public Category Category { get; set; }
    }
}
