using System.Collections.Generic;
using System.Linq;

namespace islaam_db_client
{
    public class Student
    {
        /// <summary>
        /// The person ID of the student
        /// </summary>
        public int studentId;
        /// <summary>
        /// The name of the student
        /// </summary>
        public string studentName;
        /// <summary>
        /// The person ID of the teacher
        /// </summary>
        public int teacherId;
        /// <summary>
        /// The name of the teacher
        /// </summary>
        public string teacherName;
        /// <summary>
        /// The term used when stating the person was a student of someone
        /// </summary>
        public string relationshipTerm;
        /// <summary>
        /// The source for this information.
        /// </summary>
        public string source;

        public Student(List<object> vals, List<object> cols)
        {
            APICaller.FixList(cols, vals);

            // get all string values
            var valStrings = (from val in vals select val?.ToString()).ToList();

            // to lowercase
            var columnsInLowerCase = (from column in cols select column.ToString().ToLower()).ToList();

            /// Get column orders
            var colsInOrd = new
            {
                studentId = columnsInLowerCase.IndexOf("student id"),
                studentName = columnsInLowerCase.IndexOf("student"),
                teacherId = columnsInLowerCase.IndexOf("teacher id"),
                teacherName = columnsInLowerCase.IndexOf("teacher"),
                relationshipTerm = columnsInLowerCase.IndexOf("relationship term"),
                source = columnsInLowerCase.IndexOf("source"),
            };

            // get string values
            source = valStrings[colsInOrd.source];
            relationshipTerm = valStrings[colsInOrd.relationshipTerm];
            studentName = valStrings[colsInOrd.studentName];
            teacherName = valStrings[colsInOrd.teacherName];
            // get int values
            studentId = int.Parse(valStrings[colsInOrd.studentId]);
            teacherId = int.Parse(valStrings[colsInOrd.teacherId]);
        }
    }
}