namespace PosBooksConsumer.Models
{
    public class Request
    {
        public Client Requester { get; set; }

        public Book BookRequest { get; set; }

        public DateTime RequestDate { get; set; }
    }
}
