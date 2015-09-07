using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Implementation.Repositories.Implementation
{
    public class MongoConfigService
    {
        private const string ConfiSectionName = "MongoConnectionSettings";
        private const string ServerKey = "server";
        private const string PortKey = "port";
        private const string SourceDatabaseNameKey = "sourceDatabaseName";

        private readonly NameValueCollection MongoConfigSection;

        public MongoConfigService()
        {
            MongoConfigSection = ConfigurationManager.GetSection(ConfiSectionName) as NameValueCollection;
            if (MongoConfigSection == null)
            {
                throw new ArgumentNullException("MongoConfigSection", "Mongo confuguration sections not found in App.config file");
            }
        }

        public string GetSourceDatabaseName()
        {
            return MongoConfigSection[SourceDatabaseNameKey];
        }

        public string GetConnectionUrl()
        {
            string serverName = MongoConfigSection[ServerKey];
            string portNumber = MongoConfigSection[PortKey];
            return string.Format("mongodb://{0}:{1}", serverName, portNumber); // "mongodb://localhost:27017"
        }
    }
}