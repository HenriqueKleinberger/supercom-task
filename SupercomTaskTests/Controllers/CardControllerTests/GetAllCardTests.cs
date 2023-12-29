using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SupercomTask.DTO;
using SupercomTask.Models;
using SupercomTaskTests.Builders;
using System.Net;

namespace SupercomTaskTests.Controllers.CardControllerTests
{
    public class GetAllCardTests
    {
        public const string GET_ALL = "/cards";

        [Fact]
        public async Task ShouldGetAllCards()
        {
            // arrange
            Card card1;
            Card card2;

            await using var application = new SupercomTaskApplication();
            using (var scope = application.Services.CreateScope())
            {
                var scopeService = scope.ServiceProvider;
                var dbContext = scopeService.GetRequiredService<SuperComTaskContext>();

                Status status = new StatusBuilder().Build();

                card1 = new CardBuilder()
                    .WithTitle("Get Title 1")
                    .WithDescription("Get Description 1")
                    .WithDeadLine(new DateTime(2024, 1, 10))
                    .WithStatus(status)
                    .Build();

                card2 = new CardBuilder()
                    .WithTitle("Get Title 2")
                    .WithDescription("Get Description 2")
                    .WithDeadLine(new DateTime(2024, 1, 13))
                    .WithStatus(status)
                    .Build();

                card1 = dbContext.Cards.Add(card1).Entity;
                card2 = dbContext.Cards.Add(card2).Entity;

                dbContext.SaveChanges();
            }

            // act

            using var client = application.CreateClient();
            using var response = await client.GetAsync(GET_ALL);

            // assert
            string responseBody = await response.Content.ReadAsStringAsync();
            List<CardDTO> cards = JsonConvert.DeserializeObject<List<CardDTO>>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(cards.Count, 2);
            AssertEquals(card1, cards[0]);
            AssertEquals(card2, cards[1]);
        }

        private void AssertEquals(Card expected, CardDTO actual)
        {
            Assert.Equal(expected.CardId, actual.Id);
            Assert.Equal(expected.Status.Name, actual.Status);
            Assert.Equal(expected.Title, actual.Title);
            Assert.True(DateTime.Equals(expected.Deadline, actual.Deadline));
            Assert.True(DateTime.Equals(expected.CreatedAt, actual.CreatedAt));
        }
    }
}