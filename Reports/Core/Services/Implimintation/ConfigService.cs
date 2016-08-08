using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using System.IO;
using Newtonsoft.Json;

namespace Core.Services.Implimintation
{
    public class ConfigService: IConfigService
    {
        //public static List<BalancCardNumber> BalanceDictionary { get; set; }
        public Config GetConfig()
        {
            var config = new Config();
            if (File.Exists("Config.json"))
            {
                var json = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.json"));
                var configThis = JsonConvert.DeserializeObject<Config>(json);
                config.BalanceDictionary = configThis.BalanceDictionary;
                config.Version = configThis.Version;
            }
            else
            {
                var json = JsonConvert.SerializeObject(new Config()
                {
                    BalanceDictionary = config.BalanceDictionary,
                    Version = config.Version,
                   
                }, Formatting.Indented);

                File.WriteAllText("Config.json", json);
            }
            return config;
        }
    }
}
