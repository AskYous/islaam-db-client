using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public IList<Person> GetDataFromSheet()
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
        public Person GetPersonById(int id)
        {
            return GetDataFromSheet().FirstOrDefault(p => p.id == id);
        }
        public List<PersonSearchResult> Search(string query)
        {
            var data = GetDataFromSheet();
            var resultsQuery = data.Select(p => new PersonSearchResult {
                    person = p,
                    lavDistance = GetLavDistanceForPerson(query, p),
                })
                .OrderBy(x => x.lavDistance)
                .ToList();
            return resultsQuery;
        }

        public int GetLavDistanceForPerson(string query, Person p)
        {
            var queryVariations = QueryHelpers.GetNameVariations(query);
            var lowestScore = int.MaxValue;
            var nameVariations = QueryHelpers
                .GetNameVariations(p.name)
                .Concat(QueryHelpers.GetNameVariations(p.kunya));

            //if (p.id == 106) Debugger.Break();

            foreach (string variation in queryVariations)
            {
                foreach (string nameVariation in nameVariations)
                {
                    // check if exact match
                    if (nameVariation == variation) return 0;

                    var score = LevenshteinDistance.Compute(variation, nameVariation);
                    lowestScore = Math.Min(lowestScore, score);
                }
            }

            return lowestScore;
        }
    }
    public class PersonSearchResult
    {
        public int lavDistance;
        public Person person;
    }
}