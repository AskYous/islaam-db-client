using System.Collections.Generic;
using System.Linq;

namespace islaam_db_client
{
    public class StudentsAPI
    {
        private APICaller Caller;
        public StudentsAPI(APICaller caller)
        {
            Caller = caller;
        }
        public List<Student> GetData()
        {
            var values = Caller.Get("Students", "A", "Z", 2);
            var students = values.Skip(1).Select(p =>
            {
                var vals = new List<object>(p);
                var cols = new List<object>(values[0]);
                return new Student(vals, cols);
            }).ToList();
            return students;

        }
    }
}