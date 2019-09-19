namespace ScopedHubMonitor.Scoped
{
    public class ScopedConfig
    {
        public ScopedConfig()
        {
            HubUri = "http://localhost:5000/ScopedHub";
            ScopeGroupId = "Scope-001";
            ClientId = "Win-001";
        }

        public string HubUri { get; set; }
        public string ClientId { get; set; }
        public string ScopeGroupId { get; set; }
    }
}
