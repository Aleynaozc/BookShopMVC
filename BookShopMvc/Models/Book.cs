using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace BookShopMvc.Models
{
    public class Book
    {

        public Book()
        {
            Comments = new List<Comment>();
           
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string ImgUrl { get; set; }
        public int Price { get; set; }
        public List<Comment> Comments { get; set; }
       


    }
}
