namespace PosBooksConsumer.Models
{
    public class BookRequest
    {
        public Request Requester { get; set; }

        public BookDTO Book { get; set; }
    }
}
