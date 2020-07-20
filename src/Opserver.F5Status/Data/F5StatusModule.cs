using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Opserver.Data;
using StackExchange.Profiling;

namespace Opserver.F5Status.Data
{
    public class F5StatusModule : StatusModule<F5StatusSettings>
    {
        public override string Name => "F5Status";
        public override bool Enabled => Groups.Count > 0;

        public List<F5StatusGroup> Groups { get; }
        public F5StatusAdmin Admin { get; }

        public F5StatusModule(IConfiguration config, PollingService poller) : base(config, poller)
        {
            var snapshot = Settings;
            Groups = snapshot.Groups.Select(g => new F5StatusGroup(this, g))
                .Union(snapshot.Instances.Select(c => new F5StatusGroup(this, c)))
                .ToList();
            Admin = new F5StatusAdmin(this);
        }
        public override MonitorStatus MonitorStatus => Groups.GetWorstStatus();
        public override bool IsMember(string node)
        {
            //TODO: Get/Store Host IPs from config, compare to instance passed in
            // Or based on data provider metrics, e.g. in Bosun with identifiers, hmmmm
            return false;
        }

        /// <summary>
        /// Gets the F5Status instance with the given name, null if it doesn't exist
        /// </summary>
        /// <param name="name">The name of the <see cref="F5StatusGroup"/> to fetch.</param>
        public F5StatusGroup GetGroup(string name)
        {
            return Groups.Find(e => string.Equals(e.Name, name, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Gets the list of proxies from F5Status
        /// </summary>
        public List<Proxy> GetAllProxies()
        {
            if (!Enabled) return new List<Proxy>();
            var instances = Groups.SelectMany(g => g.Instances).ToList();
            return GetProxies(instances);
        }

        internal static List<Proxy> GetProxies(List<F5StatusInstance> instances)
        {
            using (MiniProfiler.Current.Step("F5Status - GetProxies()"))
            {
                return instances.AsParallel().SelectMany(i => i.Proxies.Data ?? Enumerable.Empty<Proxy>())
                    .Where(p => p != null)
                    .OrderBy(p => instances.IndexOf(p.Instance))
                    .ToList();
            }
        }
    }
}
