using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Pinger
{
    class PingLogic
    {
        public string Site { get; set; }
        public bool Connected { get; private set; }
        public int SpeedShow { get; private set; }
        public string RoundTrip { get; private set; }
        private object synchronyzer;
        private PingReply pingreply;

        private System.Net.NetworkInformation.Ping pinger;

        public PingLogic(string InitialSite)
        {
            this.Site = InitialSite;
            pinger = new Ping();
        }


        public void Ping()
        {
            //IPAddress[] addresses = Dns.GetHostAddresses(Site);
            //pingreply = pinger.Send(addresses[0]);
            try
            {
                pingreply = pinger.Send(Site);
            }
            catch (Exception ex)
            {
                Connected = false;
                RoundTrip = "No Connection";
                SpeedShow = 2;
                return;
            }
            if (pingreply.Status == IPStatus.Success) //if connect succesfully finished
            {
                Connected = true;
                RoundTrip = pingreply.RoundtripTime.ToString() + " " + "ms";
                if (pingreply.RoundtripTime <= 60)
                    SpeedShow = 0;
                else if (pingreply.RoundtripTime > 60 && pingreply.RoundtripTime <= 200)
                    SpeedShow = 1;
                else if (pingreply.RoundtripTime > 200)
                    SpeedShow = 2;
            }
            else
            {
                Connected = false;
                RoundTrip = "No Connection";
                SpeedShow = 2;
            }
        }
    }
}
