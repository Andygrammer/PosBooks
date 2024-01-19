using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PosBooksCore.Models
{
    public class Client
    {
        public string Name { get; set; }

        public string? Email { get; set; }
    }
}
