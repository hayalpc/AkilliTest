namespace AkilliTest.Helpers
{
    public class WebHostSettingHelper
    {
        public string Port { get; set; }
        public string Urls { get; set; }
        public int MaxConcurrentConnections { get; set; } = 500;
    }
}
