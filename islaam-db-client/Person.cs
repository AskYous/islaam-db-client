using System;
using System.Collections.Generic;
using System.Linq;

namespace islaam_db_client
{
    public class Person
    {
        /// <summary>
        /// Whether to use 'his' or 'her.'
        /// Better than 'gender' since Allaah has no gender.
        /// </summary>
        public bool useMasculinePronoun = true;
        /// <summary>
        /// An identifier for the person.
        /// </summary>
        public int? id;
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
        public int? birthYear;
        /// <summary>
        /// The person's birth year.
        /// </summary>
        public string birthPlace;
        /// <summary>
        /// The person's death year.
        /// </summary>
        public int? deathYear;
        /// <summary>
        /// The source of some or all of this information.
        /// </summary>
        public string source;
        /// <summary>
        /// The number of the person in Ibn Hajr's book: Taqreeb at-Tahdheeb.
        /// </summary>
        public int? taqreebNumber;
        /// <summary>
        /// The location the person lived in.
        /// </summary>
        public string location;
        public Person(List<object> vals, List<object> cols)
        {
            APICaller.FixList(cols, vals);

            // get all string values
            var valStrings = (from val in vals select val?.ToString()).ToList();

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
                gender = columnsInLowerCase.IndexOf("gender"),
            };

            // get string values
            name = valStrings[colsInOrd.name];
            source = valStrings[colsInOrd.source];
            kunya = valStrings[colsInOrd.fullName];
            location = valStrings[colsInOrd.location];
            generation = valStrings[colsInOrd.generation];

            if (generation != null && generation.ToLower() == "later generations")
                generation = generation.ToLower();

            // get int values
            id = int.Parse(valStrings[colsInOrd.id]);
            if (valStrings[colsInOrd.taqreebId] != null)
                taqreebNumber = int.Parse(valStrings[colsInOrd.taqreebId]);
            if (valStrings[colsInOrd.birthYear] != null)
                birthYear = int.Parse(valStrings[colsInOrd.birthYear]);
            if (valStrings[colsInOrd.deathYear] != null)
                deathYear = int.Parse(valStrings[colsInOrd.deathYear]);

            // This handles the case for Allaah since Allaah is referred to as "He"
            useMasculinePronoun = valStrings[colsInOrd.gender]?.ToLower() != "female";
        }

        public BioInfo GetBio(IslaamDBClient idb)
        {
            var people = idb.PersonAPI.GetDataFromSheet().ToList();
            var pronoun = useMasculinePronoun ? "He" : "She";
            var possesivePronoun = useMasculinePronoun ? "His" : "Her";

            // start
            var biography = $"{pronoun} is ";
            var praises = GetUniquePraises(idb);
            var teachersAndStudents = GetTeachersAndStudents(idb);
            var teachers = GetUniqueTeachers(teachersAndStudents);
            var students = GetUniqueStudents(teachersAndStudents);
            var titles = GetUniqueTitles(praises);
            var praiserNames = FriendlyJoin(praises.Select(x => x.recommenderName).ToList());

            // booleans
            var hasPraises = praises.Count > 0;
            var hasTitles = titles.Count > 0;
            var hasLocation = location != null;
            var hasKunya = kunya != null;
            var hasDeathYear = deathYear != null;
            var hasBirthYear = birthYear != null;
            var hasBirthPlace = birthPlace != null;
            var hasTeachers = teachers.Count > 0;
            var hasStudents = students.Count > 0;
            var hasGeneration = generation != null;

            /** The amount of information in this bio **/
            var amountOfInfo = GetAmountOfInfo();

            // titles
            if (hasTitles)
                biography += $"the {String.Join("the ,", titles)} ";

            // name or kunya
            if (hasKunya)
                biography += kunya;
            else
                biography += name;

            if (hasGeneration)
                biography += $", from the {generation}";
            biography += ". ";

            // birth
            if (hasBirthYear)
                if (hasBirthPlace)
                    biography += $"{pronoun} was born in {birthPlace} in the year {birthYear} AH. ";
                else
                    biography += $"{pronoun} was born in the year {birthYear} AH. ";
            else
                if (hasBirthPlace)
                biography += $"{pronoun} was born in {birthPlace}. (I don't have {possesivePronoun} birth year yet.) ";

            // location
            if (hasLocation)
                biography += $"{pronoun} lived in {location}. ";

            // praises
            if (hasPraises)
                biography += $"{pronoun} was praised by {praiserNames}. ";

            // teachers
            if (hasTeachers)
                biography += $"{pronoun} took knowledge from {FriendlyJoin(teachers)}. ";

            // students
            if (hasStudents)
                biography += $"{pronoun} taught {FriendlyJoin(students)}. ";

            // books
            // not yet supported

            // death year
            if (hasDeathYear)
                biography += $"{pronoun} died in the year {deathYear} AH.";

            biography = biography.Trim();

            // join sentences together
            return new BioInfo
            {
                info = biography,
                amountOfInfo = amountOfInfo,
            };

            int GetAmountOfInfo()
            {
                return new bool[] {
                    hasPraises,
                    hasTitles,
                    hasLocation,
                    hasKunya,
                    hasDeathYear,
                    hasBirthYear,
                    hasBirthPlace,
                    hasTeachers,
                    hasStudents,
                    hasGeneration
                }.Where(x => x).Count();
            }
        }

        private static List<string> GetUniqueTitles(List<Praise> praises)
        {
            return praises
                .Where(p => p.title != null)
                .Select(p => p.title)
                .Distinct()
                .ToList();
        }

        private List<string> GetUniqueStudents(IEnumerable<Student> teachersAndStudents)
        {
            return teachersAndStudents
                .Where(x => x.teacherId == id)
                .GroupBy(x => x.studentId)
                .Select(x => x.First())
                .Select(x => x.studentName)
                .ToList();
        }

        private List<string> GetUniqueTeachers(IEnumerable<Student> teachersAndStudents)
        {
            var teachers = teachersAndStudents
                .Where(x => x.studentId == id)
                .GroupBy(x => x.teacherId)
                .Select(x => x.First())
                .Select(x => x.teacherName)
                .ToList();
            return teachers;
        }

        private IEnumerable<Student> GetTeachersAndStudents(IslaamDBClient idb)
        {
            return idb.StudentsAPI
                .GetData()
                .Where(x => x.studentId == id || x.teacherId == id);
        }

        private List<Praise> GetUniquePraises(IslaamDBClient idb)
        {
            var praises = idb.PraisesAPI
                .GetData()
                .Where(pr => pr.recommendeeId == id)
                .GroupBy(x => x.recommenderId)
                .Select(x => x.First()) // make unique
                .ToList();
            return praises;
        }

        private string FriendlyJoin(List<string> list)
        {
            if (list.Count == 0) return null;
            if (list.Count == 1) return list[0];
            if (list.Count == 2) return $"{list[0]} and {list[1]}";

            return $"{String.Join(", ", list.SkipLast(1))}, and {list.Last()}";
        }
    }
    public class BioInfo
    {
        public int amountOfInfo;
        public string info;
    }
}
