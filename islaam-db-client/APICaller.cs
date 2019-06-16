using Google.Apis.Sheets.v4;
using System.Collections.Generic;
using System.Linq;

namespace islaam_db_client
{
    public class APICaller
    {
        /// <summary>
        /// The sheet ID.
        /// </summary>
        private readonly string SHEET_ID = "1oEhVbC85KnVYpjOnqX18plTSyjyH6F4dxNQ4SjjkBAs";
        /// <summary>
        /// The service that calls the API.
        /// </summary>
        private SheetsService service;
        public APICaller(string ApiKey)
        {
            service = new SheetsService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                ApiKey = ApiKey
            });
        }
        public IList<IList<object>> Get(
            string sheetName = "People",
            string fromRange = "A",
            string toRange = "Z",
            int minNonNullCols = 1
        )
        {
            var range = $"{sheetName}!{fromRange}:{toRange}";
            var values = service.Spreadsheets.Values
                .Get(SHEET_ID, range)
                .Execute()
                .Values
                .Where(v => v.Count >= minNonNullCols)
                .Where(value =>
                {
                    for (var i = 0; i < minNonNullCols; i++)
                    {
                        if (string.IsNullOrWhiteSpace(value[i].ToString()))
                        {
                            return false;
                        }
                    }
                    return true;
                });
            return values.ToList();
        }

        /// <summary>
        /// Makes the row size == column size by appending nulls. Also converts empty strings to nulls.
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="values"></param>
        public static void FixList(List<object> columns, List<object> values)
        {
            for (var i = 0; i < columns.Count; i++)
            {
                if (values.Count < columns.Count)
                {
                    values.Add(null);
                }
                if (values[i] != null && string.IsNullOrWhiteSpace(values[i].ToString()))
                {
                    values[i] = null;
                }
            }
        }
    }
}