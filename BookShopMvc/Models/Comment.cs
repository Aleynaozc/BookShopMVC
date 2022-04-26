namespace BookShopMvc.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public Book Book { get; set; }
    }
}
