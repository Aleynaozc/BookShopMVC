using BookShopMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.ViewModals
{
    public class CategorySellerBook
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string ImgUrl { get; set; }
        public int Price { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
        public List<int> SellerIds { get; set; } = new List<int>();


    }
}
