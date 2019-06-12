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
        private IList<Person> GetData()
        {
            var values = Caller.Get("People", "A", "Z").Execute().Values;
            var valsWithIDAndName = values
                .Skip(1)
                .Where(v => v.Count >= 2)
                .Where(value => !string.IsNullOrWhiteSpace(value[0].ToString())
                    && !string.IsNullOrWhiteSpace(value[1].ToString())
                )
                .Select(p =>
                    {
                        var vals = new List<object>(p);
                        var cols = new List<object>(values[0]);
                        return new Person(vals, cols);
                    }
                )
                 .ToList();
            return valsWithIDAndName;

        }
        public List<Person> Search(string query)
        {
            query = query.ToLower();
            var data = GetData();
            var results = data
                .OrderBy(p =>
                {
                    if (p.name == query) return 0;
                    if (p.kunya != null && p.kunya == query) return 0;

                    if (p.name.Contains(query)) return 0;
                    if (p.kunya != null && p.kunya.Contains(query)) return 0;

                    var nameScore = LevenshteinDistance.Compute(query, p.name);
                    var kunyaScore = int.MaxValue;

                    if (p.kunya != null) kunyaScore = LevenshteinDistance.Compute(query, p.kunya);
                    return Math.Min(nameScore, kunyaScore);
                });
            return results.ToList();
        }
    }
}