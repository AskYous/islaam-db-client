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
        /// The name of the recommendee
        /// </summary>
        public string recommendeeName;
        /// <summary>
        /// The name of the recommender
        /// </summary>
        public string recommenderName;
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
                recommendee = columnsInLowerCase.IndexOf("recommendee"),
                recommender = columnsInLowerCase.IndexOf("recommender"),
                title = columnsInLowerCase.IndexOf("title"),
                source = columnsInLowerCase.IndexOf("source"),
            };

            var recommendee = valStrings[colsInOrd.recommendee];
            var recommender = valStrings[colsInOrd.recommender];

            // get string values
            source = valStrings[colsInOrd.source];
            title = valStrings[colsInOrd.title];
            recommendeeName = recommendee.Split(". ")[1];
            recommenderName = recommender.Split(". ")[1];
            // get int values
            recommendeeId = int.Parse(recommendee.Split(". ")[0]);
            recommenderId = int.Parse(recommender.Split(". ")[0]);
        }
    }
}