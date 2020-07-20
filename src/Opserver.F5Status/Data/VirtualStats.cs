using System;
using System.Collections.Generic;
using System.Text;

namespace Opserver.F5Status.Data
{

    public class Nestedstats
    {
        public string kind { get; set; }
        public string selfLink { get; set; }
        public VirtualStats entries { get; set; }
    }

    public class VirtualStats
    {
        public long clientsidebitsIn { get; set; }
        public long clientsidebitsOut { get; set; }
        public long clientsidecurConns { get; set; }
        public long clientsideevictedConns { get; set; }
        public long clientsidemaxConns { get; set; }
        public long clientsidepktsIn { get; set; }
        public long clientsidepktsOut { get; set; }
        public long clientsideslowKilled { get; set; }
        public long clientsidetotConns { get; set; }
        public string cmpEnableMode { get; set; }
        public string cmpEnabled { get; set; }
        public long csMaxConnDur { get; set; }
        public long csMeanConnDur { get; set; }
        public long csMinConnDur { get; set; }
        public string destination { get; set; }
        public long ephemeralbitsIn { get; set; }
        public long ephemeralbitsOut { get; set; }
        public long ephemeralcurConns { get; set; }
        public long ephemeralevictedConns { get; set; }
        public long ephemeralmaxConns { get; set; }
        public long ephemeralpktsIn { get; set; }
        public long ephemeralpktsOut { get; set; }
        public long ephemeralslowKilled { get; set; }
        public long ephemeraltotConns { get; set; }
        public long fiveMinAvgUsageRatio { get; set; }
        public long fiveSecAvgUsageRatio { get; set; }
        public string tmName { get; set; }
        public long oneMinAvgUsageRatio { get; set; }
        public string statusavailabilityState { get; set; }
        public string statusenabledState { get; set; }
        public string statusstatusReason { get; set; }
        public string syncookieStatus { get; set; }
        public long syncookieaccepts { get; set; }
        public long syncookiehwAccepts { get; set; }
        public long syncookiehwSyncookies { get; set; }
        public long syncookiehwsyncookieInstance { get; set; }
        public long syncookierejects { get; set; }
        public long syncookieswsyncookieInstance { get; set; }
        public long syncookiesyncacheCurr { get; set; }
        public long syncookiesyncacheOver { get; set; }
        public long syncookiesyncookies { get; set; }
        public long totRequests { get; set; }
    }
}
