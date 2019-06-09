using islaam_db_client;
using Xunit;

namespace islaam_db_client_unit_tests
{
    public class UnitTest1
    {
        private readonly IslaamDBClient islaamDB;
        public UnitTest1()
        {
            islaamDB = new IslaamDBClient();
        }
        [Fact]
        public void SearchForAPerson()
        {
            var result = islaamDB.PersonAPI.Search("abu khadeeja");
            Assert.True(result.Count > 0);
            Assert.Contains("abu khadeejah", result[0].name.ToLower());
        }
    }
}
