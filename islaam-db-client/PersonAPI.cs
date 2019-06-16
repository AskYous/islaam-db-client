using System;
using System.Collections.Generic;
using System.Linq;

namespace islaam_db_client
{
    public class PersonAPI
    {
        private APICaller Caller;
        public PersonAPI(APICaller caller)
        {
            this.Caller = caller;
        }
        public IList<Person> GetData()
        {
            var values = Caller.Get("People", "A", "Z");
            var people = values
                .Skip(1)
                .Select(p =>
                    {
                        var vals = new List<object>(p);
                        var cols = new List<object>(values[0]);
                        return new Person(vals, cols);
                    }
                )
                 .ToList();
            return people;

        }
        public List<Person> Search(string query)
        {
            query = query.ToLower();
            var data = GetData();
            var resultsQuery = data
                .OrderBy(p =>
                {
                    var tempName = p.name;
                    var tempKunya = p.kunya;

                    if (!query.ToLower().Contains("shaykh") && !query.ToLower().Contains("shaikh"))
                    {
                        tempName = tempName.Replace("Shaykh", "");
                        tempKunya = tempKunya?.Replace("Shaykh", "");
                    }
                    if (tempName == query) return 0;
                    if (tempKunya != null && tempKunya == query) return 0;

                    if (tempName.Contains(query)) return 0;
                    if (tempKunya != null && tempKunya.Contains(query)) return 0;

                    var nameScore = LevenshteinDistance.Compute(query, tempName);
                    var kunyaScore = int.MaxValue;

                    if (tempKunya != null) kunyaScore = LevenshteinDistance.Compute(query, p.kunya);
                    return Math.Min(nameScore, kunyaScore);
                });
            var results = resultsQuery.ToList();
            return results;
        }
    }
}