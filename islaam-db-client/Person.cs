using System;

namespace islaam_db_client
{
    public class Person
    {
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
        public int generationId;
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
