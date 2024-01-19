using Bogus;
using PosBooksCore.Models;

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
