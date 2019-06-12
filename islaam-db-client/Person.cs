using System;
using System.Collections.Generic;
using System.Linq;

namespace islaam_db_client
{
    public class Person
    {
        public Person(List<object> vals, List<object> cols)
        {
            APICaller.FixList(cols, vals);

            // ensure its size is full
            var valStrings = (
                from val in vals 
                select val?.ToString()
            ).ToList();

            // to lowercase
            var columnsInLowerCase = (from column in cols select column.ToString().ToLower()).ToList();

            /// Get column orders
            var colsInOrd = new
            {
                id = columnsInLowerCase.IndexOf("id"),
                name = columnsInLowerCase.IndexOf("name"),
                fullName = columnsInLowerCase.IndexOf("full name"),
                taqreebId = columnsInLowerCase.IndexOf("taqreeb id"),
                generation = columnsInLowerCase.IndexOf("generation"),
                birthYear = columnsInLowerCase.IndexOf("birth year"),
                deathYear = columnsInLowerCase.IndexOf("death year"),
                location = columnsInLowerCase.IndexOf("location"),
                source = columnsInLowerCase.IndexOf("source"),
            };
            name = valStrings[colsInOrd.name];
            kunya = valStrings[colsInOrd.fullName];
            location = valStrings[colsInOrd.location];
            source = valStrings[colsInOrd.source];
            generation = valStrings[colsInOrd.generation];

            // ints that need parsing
            id = int.Parse(valStrings[colsInOrd.id]);
            if (valStrings[colsInOrd.taqreebId] != null)
                taqreebNumber = int.Parse(valStrings[colsInOrd.taqreebId]);
            if (valStrings[colsInOrd.birthYear] != null)
                birthYear = int.Parse(valStrings[colsInOrd.birthYear]);
            if (valStrings[colsInOrd.deathYear] != null)
                deathYear = int.Parse(valStrings[colsInOrd.deathYear]);
        }
        /// <summary>
        /// An identifier for the person.
        /// </summary>
        public int id;
        /// <summary>
        /// The name of the person.
        /// </summary>
        public string name;
        /// <summary>
        /// The person's full name or kunya.
        /// </summary>
        public string kunya;
        /// <summary>
        /// The generation the person was part of.
        /// </summary>
        public string generation;
        /// <summary>
        /// The person's birth year.
        /// </summary>
        public int birthYear;
        /// <summary>
        /// The person's death year.
        /// </summary>
        public int deathYear;
        /// <summary>
        /// The source of some or all of this information.
        /// </summary>
        public string source;
        /// <summary>
        /// The number of the person in Ibn Hajr's book: Taqreeb at-Tahdheeb.
        /// </summary>
        public int taqreebNumber;
        /// <summary>
        /// The location the person lived in.
        /// </summary>
        public string location;
    }
}
