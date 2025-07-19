namespace TodoApi.Configuration
{
    public class DataStoreOptions
    {
        public const string SectionName = "DataStore";
        
        public bool InitializeSampleData { get; set; } = true;
    }
} 