using CulturalCenterWpf.Models;
using Stimulsoft.Blockly.StiBlocks.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CulturalCenterWpf.Services
{
    internal class ApiServices
    {
        private HttpClient _httpClient;

        public ApiServices()
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(App.Settings.UrlApi)
            };
        }

        public HttpClient GetHttpClient()
        {
            return _httpClient;
        }
    }
}
