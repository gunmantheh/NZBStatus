using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace NZBStatus
{
    static class Program
    {
        static void Main(string[] args)
        {
            var jsonReader = new JsonReader("http://hydra:8080/sabnzbd/api?mode=queue&output=json",
                                            "0d40686633a53a316e0e9d46020a98c5");
            Console.Clear();
            Console.WriteLine(jsonReader.TotalMB);
            Console.Title = String.Format("SabNZBd watcher: {0}/{1}", jsonReader.AlreadyDownloadedMB, jsonReader.TotalMB);
        }
    }
}
