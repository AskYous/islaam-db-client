using System.Collections.Generic;

namespace islaam_db_client
{
    internal class Praise
    {
        private List<object> vals;
        private List<object> cols;

        public Praise(List<object> vals, List<object> cols)
        {
            this.vals = vals;
            this.cols = cols;
        }
    }
}