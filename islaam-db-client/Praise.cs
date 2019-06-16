using System.Collections.Generic;
using System.Linq;

namespace islaam_db_client
{
    public class Praise
    {
        /// <summary>
        /// The person ID of the recommendee
        /// </summary>
        public int recommendeeId;
        /// <summary>
        /// The person ID of the recommender
        /// </summary>
        public int recommenderId;
        /// <summary>
        /// The title he was given
        /// </summary>
        public string title;
        /// <summary>
        /// The source for this information.
        /// </summary>
        public string source;

        public Praise(List<object> vals, List<object> cols)
        {
            APICaller.FixList(cols, vals);

            // get all string values
            var valStrings = (from val in vals select val?.ToString()).ToList();

            // to lowercase
            var columnsInLowerCase = (from column in cols select column.ToString().ToLower()).ToList();

            /// Get column orders
            var colsInOrd = new
            {
                recommendeeId = columnsInLowerCase.IndexOf("recommendee id"),
                recommenderId = columnsInLowerCase.IndexOf("recommender id"),
                title = columnsInLowerCase.IndexOf("title"),
                source = columnsInLowerCase.IndexOf("source"),
            };

            // get string values
            source = valStrings[colsInOrd.source];
            title = valStrings[colsInOrd.title];
            // get int values
            recommendeeId = int.Parse(valStrings[colsInOrd.recommendeeId]);
            recommenderId = int.Parse(valStrings[colsInOrd.recommenderId]);
        }
    }
}