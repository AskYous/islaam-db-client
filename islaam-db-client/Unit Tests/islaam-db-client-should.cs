using System.Collections.Generic;
using System.Linq;
using islaam_db_client;
using Xunit;

namespace islaam_db_client_unit_tests
{
    public class UnitTest
    {
        private readonly IslaamDBClient islaamDB;
        public UnitTest()
        {
            islaamDB = new IslaamDBClient(APIKey.KEY);
        }

        [Theory]
        [InlineData(72, 130, null)]
        [InlineData(72, 112, "Shaykh")]
        [InlineData(72, 59, "Flag-bearer")]
        public void GetCorrectPraiseTitles(int recommendee, int recommender, string title)
        {
            var praise = islaamDB.PraisesAPI.GetData().First(p => p.recommendeeId == recommendee && p.recommenderId == recommender && p.title == title);
            Assert.NotNull(praise);
        }

        [Theory]
        [InlineData(96, new int[] { 82 })]
        [InlineData(98, new int[] { 97, 97, 72, 69 })]
        [InlineData(72, new int[] { 130, 112, 59, 73, 73, 73, 102, 125, 125, 125, 44, 113, 113 })]
        public void GetPeopleWhoPraisedAPerson(int personId, IList<int> praisers)
        {
            var person = islaamDB.PersonAPI.GetData().First(p => p.id == personId);
            praisers = praisers.OrderBy(p => p).ToList(); // sort
            var praises = islaamDB.PraisesAPI // sort
                .GetData()
                .Where(praise => praise.recommendeeId == personId)
                .OrderBy(praise => praise.recommenderId)
                .ToList();

            // same length
            Assert.Equal(praises.Count, praisers.Count);

            // same numbers
            for (var i = 0; i < praisers.Count; i++)
            {
                Assert.Equal(praises[i].recommenderId, praisers[i]);
            }
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
        [InlineData("Bin Baz", 73)]
        [InlineData("Muhammad al-Jaamee", 74)]
        [InlineData("Muhammad ibn 'Abdel Wahhaab", 210)]
        [InlineData("Shaykh Muhammad Baazmool", 71)]
        [InlineData("Muhammad Baazmool", 71)]
        [InlineData("Muhammad Bazmul", 71)]
        [InlineData("Muhammad bin Zarrad", 30)]
        [InlineData("Ibn Hajr", 16)]
        [InlineData("Mujaahid", 89)]
        public void SearchForAPerson(string query, int id)
        {
            var result = islaamDB.PersonAPI.Search(query);
            Assert.True(result.Count > 0);
            Assert.Equal(result[0].id, id);
        }
    }
}