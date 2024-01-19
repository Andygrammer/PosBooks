using System.ComponentModel.DataAnnotations;

namespace PosBooksCore.Models
{
    public class WaitList
    {
        [Key]
        public Client Requester { get; set; }

        [Key]
        public Book BookRequest { get; set; }

        public DateTime RequestDate { get; set; }
    }
}
