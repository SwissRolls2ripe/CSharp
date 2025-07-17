using System.Configuration;

namespace DbConn
{
    public static class DBConn
    {
        public static string ConnectionString { get; private set; }

        static DBConn()
        {
            // 从配置文件获取连接字符串
            // 从指定的配置文件获取连接字符串
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = @"Db.config";
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            ConnectionString = config.ConnectionStrings.ConnectionStrings["SQLServerConn"].ConnectionString;
        }

        //Db.config 文件内容示例
        // <?xml version="1.0" encoding="utf-8" ?>
        // <configuration>
        //   <connectionStrings>
        //     <add name="SQLServerConn" connectionString="Server=XXXX;Initial Catalog=WF45GettingStartedTutorial;User ID=XXXX;Password=XXXX;" />
        //   </connectionStrings>
        // </configuration>
    }
}
