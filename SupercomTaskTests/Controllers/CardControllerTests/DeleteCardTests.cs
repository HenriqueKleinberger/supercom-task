using Microsoft.Extensions.DependencyInjection;
using SupercomTask.Models;
using System.Net;
using SupercomTaskTests.Builders;
using Newtonsoft.Json;
using SupercomTask.DTO;

namespace SupercomTaskTests.Controllers.CardControllerTests
{
    public class DeletelCardTests
    {
        public const string DELETE = "/cards";
        public const string GET_ALL = "/cards";

        [Fact]
        public async Task ShouldDeleteCard()
        {
            // arrange
            Card cardToDelete;

            await using var application = new SupercomTaskApplication();
            using (var scope = application.Services.CreateScope())
            {
                var scopeService = scope.ServiceProvider;
                var dbContext = scopeService.GetRequiredService<SuperComTaskContext>();

                Status status = new StatusBuilder().Build();

                cardToDelete = new CardBuilder()
                    .WithTitle("Delete Title 1")
                    .WithDescription("Delete Description 1")
                    .WithDeadLine(new DateTime(2024, 1, 10))
                    .WithStatus(status)
                    .Build();

                cardToDelete = dbContext.Cards.Add(cardToDelete).Entity;

                dbContext.SaveChanges();
            }

            // act

            using var client = application.CreateClient();
            using var deleteResponse = await client.DeleteAsync($"{DELETE}/{cardToDelete.CardId}");
            using var getResponse = await client.GetAsync(GET_ALL);

            // assert
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
            string responseBody = await getResponse.Content.ReadAsStringAsync();
            List<CardDTO> cards = JsonConvert.DeserializeObject<List<CardDTO>>(responseBody);
            Assert.Equal(cards.Count, 0);
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