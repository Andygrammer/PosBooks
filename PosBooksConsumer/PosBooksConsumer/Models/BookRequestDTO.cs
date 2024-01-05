namespace PosBooksConsumer.Models
{
    public class BookRequestDTO
    {
        public Client Requester { get; set; }

        public Book Book { get; set; }
    }
}
