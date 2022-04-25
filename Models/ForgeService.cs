namespace MyForgeApp.Models
{
    public partial class ForgeService
    {
        //partial class: provides a special ability to implement the functionality of a single class into multiple files
        //and all these files are combined into a single class file when the application is compiled
        //we will be implementing all the Forge-specific logic that will be used in different areas of our server application

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _bucket;

        public ForgeService(string clientId, string clientSecret, string bucket = null)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _bucket = string.IsNullOrEmpty(bucket) ? string.Format("{0}-basic-app", _clientId.ToLower()) : bucket;
        }
    }
}
