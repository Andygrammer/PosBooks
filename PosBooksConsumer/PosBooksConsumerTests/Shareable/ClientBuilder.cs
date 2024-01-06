using Bogus;
using PosBooksConsumer.Models;

namespace PosBooksConsumerTests.Shareable
{
    public class ClientBuilder : Faker<Client>
    {
        public ClientBuilder()
        {
            RuleFor(f => f.Name, f => f.Name.FullName());
            RuleFor(f => f.Email, f => f.Internet.Email());
        }
    }
}
