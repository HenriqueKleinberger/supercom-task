using Microsoft.Extensions.DependencyInjection;
using SupercomTask.Models;
using System.Net;
using SupercomTaskTests.Builders;
using Newtonsoft.Json;
using SupercomTask.DTO;
using SupercomTask.Constants;
using System.Text;
using SupercomTaskTests.Utils;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SupercomTaskTests.Controllers.CardControllerTests
{
    public class PutCardTests : IClassFixture<WebApplicationFactory<Program>>
    {
        public const string IN_PROGRESS = "In Progress";
        public const string PUT_URL = "/cards";

        private readonly WebApplicationFactory<Program> _factory;

        public PutCardTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ShouldUpdateValidCard()
        {
            // arrange
            
            Card cardToUpdate;
            CardDTO cardDTO = new CardDTOBuilder()
                .WithDeadLine(DateTime.Now.AddDays(2))
                .WithTitle("Updated Title")
                .WithDescription("Updated Description")
                .WithStatus(IN_PROGRESS)
                .Build();

            await using var application = new SupercomTaskApplication();

            using (var scope = application.Services.CreateScope())
            {
                var scopeService = scope.ServiceProvider;
                var dbContext = scopeService.GetRequiredService<SuperComTaskContext>();

                Status toDo = new StatusBuilder().Build();
                Status InProgress = new StatusBuilder().WithName(IN_PROGRESS).Build();

                cardToUpdate = new CardBuilder()
                    .WithTitle("Put Title 1")
                    .WithDescription("Put Description 1")
                    .WithDeadLine(new DateTime(2024, 1, 10))
                    .WithStatus(toDo)
                    .Build();

                cardToUpdate = dbContext.Cards.Add(cardToUpdate).Entity;
                dbContext.Status.Add(InProgress);

                dbContext.SaveChanges();
            }

            // act
            HttpContent content = new StringContent(JsonConvert.SerializeObject(cardDTO), Encoding.UTF8, "application/json");
            using var client = application.CreateClient();
            using var response = await client.PutAsync($"{PUT_URL}/{cardToUpdate.CardId}", content);

            // assert
            string responseBody = await response.Content.ReadAsStringAsync();
            CardDTO cardUpdated = JsonConvert.DeserializeObject<CardDTO>(responseBody);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            AssertObjects.AssertEquals(cardUpdated, cardDTO);
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenStatusIsInvalid()
        {
            // arrange
            string IN_PROGRESS = "In Progress";
            Card cardToUpdate;
            CardDTO cardDTO = new CardDTOBuilder()
                .WithDeadLine(DateTime.Now.AddDays(2))
                .WithTitle("Updated Title")
                .WithDescription("Updated Description")
                .WithStatus(IN_PROGRESS)
                .Build();

            await using var application = new SupercomTaskApplication();

            using (var scope = application.Services.CreateScope())
            {
                var scopeService = scope.ServiceProvider;
                var dbContext = scopeService.GetRequiredService<SuperComTaskContext>();

                Status toDo = new StatusBuilder().Build();

                cardToUpdate = new CardBuilder()
                    .WithTitle("Put Invalid Status Title 1")
                    .WithDescription("Description 1")
                    .WithDeadLine(new DateTime(2024, 1, 10))
                    .WithStatus(toDo)
                    .Build();

                cardToUpdate = dbContext.Cards.Add(cardToUpdate).Entity;

                dbContext.SaveChanges();
            }

            // act
            HttpContent content = new StringContent(JsonConvert.SerializeObject(cardDTO), Encoding.UTF8, "application/json");
            using var client = application.CreateClient();
            using var response = await client.PutAsync($"{PUT_URL}/{cardToUpdate.CardId}", content);

            // assert
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(responseBody, ErrorMessages.INVALID_STATUS);
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenDeadLineIsBeforeCardCreatedDate()
        {
            // arrange
            CardDTO cardDTO = new CardDTOBuilder().WithDeadLine(DateTime.Now.AddDays(-1)).Build();
            Card cardToUpdate;
            await using var application = new SupercomTaskApplication();

            using (var scope = application.Services.CreateScope())
            {
                var scopeService = scope.ServiceProvider;
                var dbContext = scopeService.GetRequiredService<SuperComTaskContext>();

                Status toDo = new StatusBuilder().Build();

                cardToUpdate = new CardBuilder()
                    .WithTitle("Put Invalid Status Title 1")
                    .WithDescription("Description 1")
                    .WithDeadLine(new DateTime(2024, 1, 10))
                    .WithStatus(toDo)
                    .Build();

                cardToUpdate = dbContext.Cards.Add(cardToUpdate).Entity;

                dbContext.SaveChanges();
            }

            // act
            HttpContent content = new StringContent(JsonConvert.SerializeObject(cardDTO), Encoding.UTF8, "application/json");
            using var client = application.CreateClient();
            using var response = await client.PutAsync($"{PUT_URL}/{cardToUpdate.CardId}", content);

            // assert
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(responseBody, ErrorMessages.DEADLINE_VALIDATION);
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenCardNotFound()
        {
            // arrange
            CardDTO cardDTO = new CardDTOBuilder()
                .WithDeadLine(DateTime.Now.AddDays(2))
                .WithTitle("Updated Title")
                .WithDescription("Updated Description")
                .WithStatus(IN_PROGRESS)
                .Build();

            await using var application = new SupercomTaskApplication();

            using (var scope = application.Services.CreateScope())
            {
                var scopeService = scope.ServiceProvider;
                var dbContext = scopeService.GetRequiredService<SuperComTaskContext>();

                Status InProgress = new StatusBuilder().WithName(IN_PROGRESS).Build();
                dbContext.Status.Add(InProgress);

                dbContext.SaveChanges();
            }

            // act
            HttpContent content = new StringContent(JsonConvert.SerializeObject(cardDTO), Encoding.UTF8, "application/json");
            using var client = application.CreateClient();
            using var response = await client.PutAsync($"{PUT_URL}/1", content);

            // assert
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(responseBody, ErrorMessages.CARD_NOT_FOUND);
        }
    }
}