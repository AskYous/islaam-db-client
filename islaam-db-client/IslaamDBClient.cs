using Google.Apis.Sheets.v4.Data;

namespace islaam_db_client
{
    public class IslaamDBClient
    {
        private readonly APICaller ApiCaller;
        public readonly PersonAPI PersonAPI;
        public readonly PraisesAPI PraisesAPI;
        public readonly StudentsAPI StudentsAPI;
        public IslaamDBClient(string APIKey)
        {
            ApiCaller = new APICaller(APIKey);
            PersonAPI = new PersonAPI(ApiCaller);
            PraisesAPI = new PraisesAPI(ApiCaller);
            StudentsAPI = new StudentsAPI(ApiCaller);
        }
    }
}
