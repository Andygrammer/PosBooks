namespace PosBooksConsumer.Models
{
    public class Book
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public int Id { get; set; }

        public string Publisher { get; set; }

        public int Year { get; set; }

        public Client? Renter { get; set; }
    }
}
