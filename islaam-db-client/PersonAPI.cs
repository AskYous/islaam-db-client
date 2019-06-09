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
            return (from value in values.Skip(1)
                    select new Person(
                        new List<object>(value),
                        new List<object>(values[0])
                     )).ToList();

        }
        public List<Person> Search(string query)
        {
            query = query.ToLower();
            var results = GetData()
                .Where(p =>
                    (p.name != null && p.name.ToLower().Contains(query))
                    || (p.kunya != null && p.kunya.ToLower().Contains(query))
                ).ToList();
            return results;
        }
    }
}