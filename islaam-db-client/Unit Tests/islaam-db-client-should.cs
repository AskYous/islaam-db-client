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
            var result = islaamDB.PersonAPI.Search("Sa'dee");
            Assert.NotNull(result);
            Assert.Contains("Sa'dee", result.name);
            Assert.Contains("Sa'dee", result.kunya);
        }
    }
}
