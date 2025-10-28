namespace BookStoreApp.API.Options
{
    public class StorageOptions
    {

        public const string Storage = "Storage";
        public string BasePath { get; set; }
        public string BookCoversPath { get; set; }
        public string BookCoversPublicUrl { get; set; }
    }
}
