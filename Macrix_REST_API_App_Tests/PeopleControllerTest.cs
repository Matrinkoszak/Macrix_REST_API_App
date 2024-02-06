using Bogus;
using Macrix_REST_API_App.Controllers;
using Macrix_REST_API_App.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Macrix_REST_API_App_Tests
{
    public class PeopleControllerTest
    {
        Mock<MacrixContext> _dbContext;
        PeopleController _peopleController;

        public PeopleControllerTest()
        {
            _dbContext = new Mock<MacrixContext>();

            var mockData = GeneratePeople(1).AsQueryable();

            var mockSet =  new Mock<DbSet<Macrix_REST_API_App.Models.Person>>();
            mockSet.As<IQueryable<Macrix_REST_API_App.Models.Person>>().Setup(m => m.Provider).Returns(mockData.Provider);
            mockSet.As<IQueryable<Macrix_REST_API_App.Models.Person>>().Setup(m => m.ElementType).Returns(mockData.ElementType);
            mockSet.As<IQueryable<Macrix_REST_API_App.Models.Person>>().Setup(m => m.Expression).Returns(mockData.Expression);
            mockSet.As<IQueryable<Macrix_REST_API_App.Models.Person>>().Setup(m => m.GetEnumerator()).Returns(() => mockData.GetEnumerator());

            _dbContext.Setup(x=>x.People).Returns(mockSet.Object);

            _peopleController = new PeopleController(_dbContext.Object);
        }

        private List<Macrix_REST_API_App.Models.Person> GeneratePeople(int count)
        {
            var faker = new Faker<Macrix_REST_API_App.Models.Person>()
                .RuleFor(c => c.FirstName, f => f.Person.FirstName)
                .RuleFor(c => c.LastName, f => f.Person.LastName)
                .RuleFor(c => c.StreetName, f => f.Address.StreetName())
                .RuleFor(c => c.HouseNumber, f => long.Parse(f.Address.BuildingNumber()))
                .RuleFor(c => c.ApartmentNumber, f => long.Parse(f.Address.BuildingNumber()))
                .RuleFor(c => c.PostalCode, f => f.Address.ZipCode())
                .RuleFor(c => c.Town, f => f.Address.City())
                .RuleFor(c => c.PhoneNumber, f => new Random().Next(100000000,999999999))
                .RuleFor(c => c.DateOfBirth, f => f.Person.DateOfBirth.ToString("s", System.Globalization.CultureInfo.InvariantCulture));

            return faker.Generate(count);

        }

        [Fact]
        public async Task GetPeopleTest()
        {
            var response = await _peopleController.GetPeople();
            Assert.NotNull(response);
        }
    }
}