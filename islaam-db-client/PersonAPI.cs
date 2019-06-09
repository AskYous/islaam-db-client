using System;
namespace islaam_db_client
{
    public class PersonAPI
    {
        private APICaller Caller;
        public PersonAPI(APICaller caller)
        {
            this.Caller = caller;
        }
        public Person Search(string query)
        {
            var person = Caller.Get("Class Data!A2:E");
            var response = person.Execute();
            return new Person()
            {
                name = "As-Sa'dee",
                kunya = "Aboo ‘Abdillaah, 'Abdur-Rahmaan ibn Naasir ibn ‘Abdillaah ibn Naasir as-Sa'dee",
                birthYear = 1307,
                deathYear = 1375,
                source = "http://www.bakkah.net/en/biography-shaykh-abdur-rahmaan-naasir-as-sadee.htm",
            };
        }
    }
}