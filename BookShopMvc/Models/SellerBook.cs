using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.Models
{
    public class SellerBook
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public Seller Seller { get; set; }
    }
}
