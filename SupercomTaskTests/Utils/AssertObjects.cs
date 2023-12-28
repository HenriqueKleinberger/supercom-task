using SupercomTask.DTO;

namespace SupercomTaskTests.Utils
{
    public static class AssertObjects
    {
        public static void AssertEquals(CardDTO expected, CardDTO actual)
        {
            Assert.Equal(expected.Status, actual.Status);
            Assert.Equal(expected.Title, actual.Title);
            Assert.True(DateTime.Equals(expected.Deadline, actual.Deadline));
            // AJUSTAR Assert.True(DateTime.Equals(expected.CreatedAt, actual.CreatedAt));
        }
    }
}
