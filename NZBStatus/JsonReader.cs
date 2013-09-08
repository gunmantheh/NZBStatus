using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;

namespace NZBStatus
{
    public class JsonReader
    {
        private readonly string _url;
        private readonly string _apiKey;
        private JToken _jsonString;
        private JArray _slots;
        private readonly WebClient _webClient;

        public decimal TotalMB
        {
            get
            {
                return GetRootValue<decimal>("mb");
            }
        }

        public decimal AlreadyDownloadedMB
        {
            get
            {
                return GetRootValue<decimal>("mb") - GetRootValue<decimal>("mbleft");
            }
        }

        private TClassType GetRootValue<TClassType>(string key)
        {
            var token = _jsonString.SelectToken(key);
            var value = token != null ? token.Value<TClassType>() : default(TClassType);
            return value;
        }

        public JsonReader(string url, string apiKey)
        {
            _url = url;
            _apiKey = apiKey;
            _webClient = new WebClient();
            InitializeData();
        }

        private void InitializeData()
        {
            _jsonString = JObject.Parse(GetData()).GetValue("queue");
            _slots = (JArray)_jsonString["slots"];
        }

        private string GetData()
        {
            return _webClient.DownloadString(string.Format("{0}&apikey={1}", _url, _apiKey));
        }

    }
}