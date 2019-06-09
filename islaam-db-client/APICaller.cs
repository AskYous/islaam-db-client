using Google.Apis.Sheets.v4;

namespace islaam_db_client
{
    public class APICaller
    {
        /// <summary>
        /// The sheet ID.
        /// </summary>
        private readonly string SHEET_ID = "1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms";
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
        /// <summary>
        /// Returns a GET request to the sheet.
        /// </summary>
        /// <returns>The GET request.</returns>
        /// <param name="range">The rango to get from the sheet.</param>
        public SpreadsheetsResource.ValuesResource.GetRequest Get(string range)
        {
            return service.Spreadsheets.Values.Get(SHEET_ID, range);
        }
    }
}