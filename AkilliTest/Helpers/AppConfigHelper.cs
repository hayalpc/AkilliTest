using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AkilliTest.Helpers
{
    public class AppConfigHelper
    {
        public static string AppName { get; set; }
        public static string RealUrl { get; set; }
        public static string ApiUrl { get; set; }
        public static string JwtSecurityKey { get; set; }
        public static string JwtIssuer { get; set; }
        public static string GoogleKey { get; set; }
        public static string GoogleSecretKey { get; set; }
        public static string JwtAudience { get; set; }
        public static AzureBlob AzureBlob { get; set; }
    }
    public class AzureBlob
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
        public string BlobUrl { get; set; }
    }
}
