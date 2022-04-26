
using BookShopMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.ViewModals
{
    public class BookNCategoryNSeller
    {

        public Book Book { get; set; }

        public Category Category { get; set; }

        public Seller Seller { get; set; }

        public List<Category> Categories { get; set; }

        public List<Seller> Sellers { get; set; }
        public List<Book> Books { get; set; }
    }
}
