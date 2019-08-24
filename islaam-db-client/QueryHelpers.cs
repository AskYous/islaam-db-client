using System;
using System.Collections.Generic;
using System.Linq;

namespace islaam_db_client
{
    public static class QueryHelpers
    {
        public static HashSet<string> GetNameVariations(string query)
        {
            return GetQueryVariations(query, null);
        }
        private static HashSet<string> GetQueryVariations(string query, HashSet<string> variations)
        {
            if (query == null) return new HashSet<string>();
            if (variations == null) variations = new HashSet<string>();
            variations.Add(query);

            var removableCharacters = new string[] { "'", "`", "-", " " };

            if (query != query.ToLower())
            {
                var lowerCaseVariations = GetQueryVariations(query.ToLower(), variations);
                variations.Concat(lowerCaseVariations);
            }

            foreach (string character in removableCharacters)
            {
                if (query.Contains(character))
                {
                    var withoutThatCharacter = query.Replace(character, "");
                    var withoutThatCharacterVariations = GetQueryVariations(withoutThatCharacter, variations);
                    variations.Concat(withoutThatCharacterVariations);
                }
            }

            return variations;
        }
    }
}
