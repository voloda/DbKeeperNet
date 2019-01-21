using System.IO;
using DbKeeperNet.Engine;

namespace DbKeeperNet.Extensions.CloudSpanner
{
    public class CloudSpannerDatabaseServiceInstaller : IDatabaseServiceInstaller
    {
        public Stream GetInstallerScript()
        {
            return typeof(CloudSpannerDatabaseServiceInstaller).Assembly.GetManifestResourceStream(
                "DbKeeperNet.Extensions.CloudSpanner.CloudSpannerDatabaseServiceInstall.xml");
        }
    }
}