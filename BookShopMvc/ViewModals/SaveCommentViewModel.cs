using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.ViewModals
{
    public class SaveCommentViewModel
    {
        public int BookID{ get; set; }
        public string Comment { get; set; }
    }
}
