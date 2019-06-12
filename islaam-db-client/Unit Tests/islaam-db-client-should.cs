using islaam_db_client;
using Xunit;

namespace islaam_db_client_unit_tests
{
    public class UnitTest1
    {
        private readonly IslaamDBClient islaamDB;
        public UnitTest1()
        {
            islaamDB = new IslaamDBClient(APIKey.KEY);
        }

        [Theory]
        [InlineData("abu khadeeja", 99)]
        [InlineData("badr", 253)]
        [InlineData("abdurrazaq badr", 253)]
        [InlineData("'Abdur-Razzaaq", 253)]
        [InlineData("'Abdur-Razzaaq Al-Badr", 253)]
        [InlineData("moosaa richardson", 69)]
        [InlineData("As-Sa'dee", 7)]
        [InlineData("ibn taymiyyah", 215)]
        [InlineData("ibn taymiyya", 215)]
        [InlineData("Imaam Ahmad", 106)]
        public void SearchForAPerson(string query, int id)
        {
            var result = islaamDB.PersonAPI.Search(query);
            Assert.True(result.Count > 0);
            Assert.Equal(result[0].id, id);
        }
    }
}
