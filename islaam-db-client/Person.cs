﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace islaam_db_client
{
    public class Person
    {
        /// <summary>
        /// The gender of the person.
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

        public string BioIntro(IslaamDBClient idb)
        {
            var people = idb.PersonAPI.GetDataFromSheet().ToList();
            var pronoun = useMasculinePronoun ? "He" : "She";
            var possesivePronoun = useMasculinePronoun ? "His" : "Her";
            var bioIntro = new List<string> { $"{pronoun} is " };
            var praises = idb.PraisesAPI.GetData().Where(pr => pr.recommendeeId == id).ToList();
            var titles = praises.Where(p => p.title != null).Select(p => p.title).Distinct().ToList();
            var praisers = getPraisers(people, praises);

            // booleans
            var hasPraises = praises.Count > 0;
            var hasTitles = titles.Count > 0;
            var hasLocation = location != null;
            var hasKunya = kunya != null;
            var hasDeathYear = deathYear != null;
            var hasBirthYear = birthYear != null;

            // praises
            bioIntro[0] += (hasKunya ? kunya : name) + ".";
            if (hasTitles)
            {
                bioIntro.Add($"{possesivePronoun} titles include: {String.Join(", ", titles)}.");
            }

            // location
            if (hasLocation)
            {
                bioIntro.Add($"{pronoun} is from {location}.");
            }

            // birth and death year
            if (hasBirthYear && hasDeathYear)
            {
                bioIntro.Add(
                    $"{pronoun} was born in the year {birthYear} and died {deathYear} AH."
                );
            }
            else if (hasBirthYear)
            {
                bioIntro.Add($"{pronoun} was born in the year {birthYear} AH.");
            }
            else if (hasDeathYear)
            {
                bioIntro.Add($"{pronoun} died in the year {deathYear} AH.");
            }

            if (hasPraises)
            {
                bioIntro.Add($"He was priased by: {praisers}.");
            }

            bioIntro.Add("(Please note that the research is not yet complete.)");

            // join sentences together
            return String.Join(" ", bioIntro);
        }
        private string getPraisers(List<Person> people, List<Praise> praises)
        {
            return String.Join(", ", praises
                .Select(p => p.recommenderId)
                .Distinct()
                .Select(pId => people.First(p => p.id == pId))
                .Select(person => person.name)
            );
        }
    }
}
