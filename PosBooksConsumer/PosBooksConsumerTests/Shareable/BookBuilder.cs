using Bogus;
using PosBooksCore.Models;

namespace PosBooksConsumerTests.Shareable
{
    public class BookBuilder : Faker<Book>
    {
        public BookBuilder()
        {
            RuleFor(f => f.Author, f => f.Name.FullName());
            RuleFor(f => f.Publisher, f => f.Company.CompanyName());
            RuleFor(f => f.Title, f => f.Random.String2(50));
            RuleFor(f => f.Year, f => f.Date.Past().Year);
        }

        public BookBuilder WithId(int id)
        {
            RuleFor(f => f.Id, id);
            return this;
        }

        public BookBuilder WithRenter(Client? client)
        {
            RuleFor(f => f.Renter, client);
            return this;
        }
    }
}
