using System;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Net.Http;
using System.Text.Json;
using CefSharp.Wpf;
using System.IO;

namespace Lab5
{
    public class ViewModel
    {
        private readonly HttpClient httpClient = new HttpClient();

        private readonly string appId;
        private readonly string secretKey;


        public ChromiumWebBrowser WebBrowser { get; set; }

        public ViewModel()
        {
            string rootDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            string filePath = Path.Combine(rootDirectory, "appconfig.ini");
            IniFile cfg = new IniFile(filePath);

            this.appId = cfg.Read("APPID", "AIS");
            this.secretKey = cfg.Read("SECRET", "AIS");
        }

        public Command AuthCommand
        {
            get => new Command(obj =>
            {
                if (WebBrowser is null) return;

                var uriStr = $"https://oauth.vk.com/authorize?client_id={appId}&scope=docs&redirect_uri=https://oauth.vk.com/blank.html&display=page&v=5.6&response_type=code";
                WebBrowser.AddressChanged += BrowserOnNavigated;
                WebBrowser.Load(uriStr);
            });
        }

        private void BrowserOnNavigated(object sender, DependencyPropertyChangedEventArgs e)
        {
            var uri = new Uri((string)e.NewValue);
            if (uri.AbsoluteUri.Contains(@"oauth.vk.com/blank.html#"))
            {
                string code = HttpUtility.ParseQueryString(uri.Fragment.Trim('#')).Get("code");
                var uriStr = $"https://oauth.vk.com/access_token?client_id={appId}&client_secret={secretKey}&redirect_uri=https://oauth.vk.com/blank.html&code={code}";
                RESTRequest(uriStr);
            }
        }

        private async void RESTRequest(string url)
        {
            string resString = "";
            var res = await GET(url);
            if (res.TryGetProperty("access_token", out _))
            {
                string access_token = res.GetProperty("access_token").ToString();

                string getResponseURI = $"https://api.vk.com/method/{{0}}?{{1}}&access_token={access_token}&v=5.154";

                res = await GET(string.Format(getResponseURI, "account.getInfo", "&fields=country,lang,2fa_required,community_comments,no_wall_replies,vk_pay_app_id"));
                resString = JsonSerializer.Serialize(res, new JsonSerializerOptions { WriteIndented = true });
                MessageBox.Show(resString);

                res = await GET(string.Format(getResponseURI, "docs.get", "&count=3"));
                resString = JsonSerializer.Serialize(res, new JsonSerializerOptions { WriteIndented = true });
                MessageBox.Show(resString);
            }
        }

        private async Task<JsonElement> GET(string url)
        {
            var json = await httpClient.GetStringAsync(url);
            return JsonDocument.Parse(json).RootElement;
        }
    }
}
