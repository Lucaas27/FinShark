namespace api.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // Foreign key
        public int? StockId { get; set; }
        //Navigation property
        public Stock? Stock { get; set; }

    }
}