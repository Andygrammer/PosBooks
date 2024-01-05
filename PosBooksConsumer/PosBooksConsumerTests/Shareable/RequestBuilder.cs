using Bogus;
using PosBooksConsumer.Models;

namespace PosBooksConsumerTests.Shareable
{
    public class RequestBuilder : Faker<WaitList>
    {
        public RequestBuilder()
        {
            RuleFor(f => f.RequestDate, f => f.Date.Recent());
        }
    }
}
