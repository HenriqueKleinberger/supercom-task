using Microsoft.Extensions.DependencyInjection;
using SupercomTask.Models;
using System.Net;
using SupercomTaskTests.Builders;
using Newtonsoft.Json;
using SupercomTask.DTO;
using SupercomTask.Constants;
using System.Text;

namespace SupercomTaskTests.Controllers.CardControllerTests
{
    public class PostCardTests
    {
        public const string POST_URL = "/Card";

        [Fact]
        public async Task ShouldInsertValidCard()
        {
            // arrange
            CardDTO cardDTO = new CardDTOBuilder().Build();

            await using var application = new SupercomTaskApplication();

            using (var scope = application.Services.CreateScope())
            {
                var scopeService = scope.ServiceProvider;
                var dbContext = scopeService.GetRequiredService<SuperComTaskContext>();

                Status status = new StatusBuilder().WithName(cardDTO.Status).Build();

                dbContext.Status.Add(status);

                dbContext.SaveChanges();
            }

            // act
            HttpContent content = new StringContent(JsonConvert.SerializeObject(cardDTO), Encoding.UTF8, "application/json");
            using var client = application.CreateClient();
            using var response = await client.PostAsync(POST_URL, content);

            // assert
            string responseBody = await response.Content.ReadAsStringAsync();
            CardDTO cardInserted = JsonConvert.DeserializeObject<CardDTO>(responseBody);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            AssertEquals(cardInserted, cardDTO);
        }

        [Fact]
        public async Task ShouldReturnBadRequestWhenStatusIsInvalid()
        {
            // arrange
            CardDTO cardDTO = new CardDTOBuilder().Build();

            await using var application = new SupercomTaskApplication();

            // act
            HttpContent content = new StringContent(JsonConvert.SerializeObject(cardDTO), Encoding.UTF8, "application/json");
            using var client = application.CreateClient();
            using var response = await client.PostAsync(POST_URL, content);

            // assert
            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(responseBody, ErrorMessages.INVALID_STATUS);
        }

        private void AssertEquals(CardDTO expected, CardDTO actual)
        {
            Assert.Equal(expected.Status, actual.Status);
            Assert.Equal(expected.Title, actual.Title);
            Assert.True(DateTime.Equals(expected.DeadLine, actual.DeadLine));
            // AJUSTAR Assert.True(DateTime.Equals(expected.CreatedAt, actual.CreatedAt));
        }
    }
}