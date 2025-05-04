using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordDocumentTableParserProject.Configuration
{
    public static class ParserConfiguration
    {
        public static IConfiguration Configuration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json",optional : false, reloadOnChange : true);

            return builder.Build();
        }
    }
}
