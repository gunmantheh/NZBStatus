using System;
using System.ComponentModel;

namespace NZBStatus.DTOs
{
    public class Slot
    {
        public string status { get; set; } // <status>Queued</status>
        public int index { get; set; } // <index>0</index>
        public string eta { get; set; } // <eta>unknown</eta>
        public int missing { get; set; } // <missing>0</missing>
        public string avg_age { get; set; } // <avg_age>2h</avg_age>
        public string script { get; set; } // <script>sabToAniDB.py</script>
        public int? msgid { get; set; } // <msgid/> TODO verify possible value of the variable
        public string verbosity { get; set; } // <verbosity/> TODO verify possible value of the variable
        public decimal mb { get; set; } // <mb>246.92</mb>
        public string sizeleft { get; set; } // <sizeleft>192 MB</sizeleft>
        [Description("Filename")]
        public string filename { get; set; } // <filename>HorribleSubs Blood Lad - 10 720p</filename>
        public string priority { get; set; } // <priority>Normal</priority> TODO convert to enum
        public string cat { get; set; } // <cat>anime</cat>
        public decimal mbleft { get; set; } // <mbleft>192.00</mbleft>
        public string timeleft { get; set; } // <timeleft>0:00:00</timeleft>
        public int percentage { get; set; } // <percentage>22</percentage>
        public string nzo_id { get; set; } // <nzo_id>SABnzbd_nzo_uiuq_v</nzo_id>
        public string unpackopts { get; set; } // <unpackopts>3</unpackopts>
        public string size { get; set; } // <size>247 MB</size> 
    }
}