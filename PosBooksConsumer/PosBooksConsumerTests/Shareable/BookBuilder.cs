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
    }
}
