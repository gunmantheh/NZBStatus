using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NZBStatus.DTOs;
using Newtonsoft.Json;
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
            get { return GetRootValue<decimal>("mb"); }
        }

        public decimal Speed
        {
            get { return GetRootValue<decimal>("kbpersec"); }
        }

        public decimal AlreadyDownloadedMB
        {
            get { return GetRootValue<decimal>("mb") - GetRootValue<decimal>("mbleft"); }
        }

        public bool IsDownloading
        {
            get { return Status == "Downloading"; }
        }

        public string Status
        {
            get { return GetRootValue<string>("status"); }
        }
        public string StatusIcon
        {
            get 
            {
                switch (Status)
                {
                    case "Downloading":
                        return "|>";
                    case "Idle":
                        return "█";
                    case "Paused":
                        return "||";
                    default:
                        return "??";
                }
            }
        }
        
        public Slot GetCurrentSlot()
        {
            if (_slots.Count > 0)
            {
                return JsonConvert.DeserializeObject<Slot>(_slots[0].ToString());
            }
            return new Slot();
        }

        public HashSet<Slot> GetAllSlots()
        {
            var result = new HashSet<Slot>();
            foreach (JToken slot in _slots)
            {
                result.Add(JsonConvert.DeserializeObject<Slot>(slot.ToString()));
            }
            return result;
        }

        private TClassType GetRootValue<TClassType>(string key)
        {
            var token = _jsonString.SelectToken(key);
            var value = token != null ? token.Value<TClassType>() : default(TClassType);
            return value;
        }

        private TClassType GetCurrentSlotValue<TClassType>(string key)
        {
            if (_slots.Count > 0)
            {
                var token = _slots[0].SelectToken(key);
                var value = token != null ? token.Value<TClassType>() : default(TClassType);
                return value;
            }
            return default(TClassType);
        }

        public JsonReader(string url, string apiKey, bool dontParse = false, string mode = "queue")
        {
            _url = string.Format("{0}/sabnzbd/api?mode={1}&output=json", url, mode);
            _apiKey = apiKey;
            _webClient = new WebClient();
            if (!dontParse)
            {
                InitializeData();
            }
            else
            {
                GetData();
            }
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

        public void RefreshData()
        {
            InitializeData();
        }
    }
}