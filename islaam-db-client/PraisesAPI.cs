using System;
using System.Collections.Generic;
using System.Linq;

namespace islaam_db_client
{
    public class PraisesAPI
    {
        private APICaller Caller;
        public PraisesAPI(APICaller caller)
        {
            Caller = caller;
        }
        public List<Praise> GetData()
        {
            var values = Caller.Get("Praises", "A", "Z", 2);
            var praises = values.Skip(1).Select(p =>
            {
                var vals = new List<object>(p);
                var cols = new List<object>(values[0]);
                return new Praise(vals, cols);
            }).ToList();
            return praises;

        }
    }
}