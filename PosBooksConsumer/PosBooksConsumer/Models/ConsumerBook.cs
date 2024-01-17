using PosBooksCore.Models;

namespace PosBooksConsumer.Models
{
    public class ConsumerBook : PosBooksCore.Models.Book
    {
        public Client? Renter { get; set; }
    }
}
