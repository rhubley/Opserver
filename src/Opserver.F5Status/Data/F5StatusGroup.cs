using System.Collections.Generic;
using System.Linq;
using Opserver.Data;

namespace Opserver.F5Status.Data
{
    public partial class F5StatusGroup : IMonitedService, ISearchableNode
    {
        private F5StatusModule Module { get; }

        public string DisplayName => Name;
        public string CategoryName => "F5Status";

        public F5StatusSettings.Group Settings { get; }

        public string Name => Settings.Name;
        public string Description => Settings.Description;
        public List<F5StatusInstance> Instances { get; }

        public MonitorStatus MonitorStatus => Instances.GetWorstStatus();

        public string MonitorStatusReason => Instances.GetReasonSummary();

        public F5StatusGroup(F5StatusModule module, F5StatusSettings.Group group)
        {
            Module = module;
            Settings = group;
            Instances = group.Instances.Select(i => new F5StatusInstance(module, i, group) { Group = this }).ToList();
            Instances.ForEach(i => i.TryAddToGlobalPollers());
        }

        /// <summary>
        /// Creates a single instance group for consistent management at a higher level.
        /// </summary>
        /// <param name="instance">The <see cref="F5StatusInstance"/> to create a single-item group for.</param>
        public F5StatusGroup(F5StatusModule module, F5StatusSettings.Instance instance)
        {
            Module = module;
            Settings = new F5StatusSettings.Group
                {
                    Name = instance.Name,
                    Description = instance.Description
                };
            Instances = new List<F5StatusInstance>
            {
                new F5StatusInstance(module, instance)
                {
                    Group = this
                }
            };
            Instances.ForEach(i => i.TryAddToGlobalPollers());
        }

        public override string ToString()
        {
            return string.Concat(Name, " - ", Instances != null ? Instances.Count.ToString() + " instances" : "");
        }

        /// <summary>
        /// Gets the list of proxies for this group
        /// </summary>
        public List<Proxy> GetProxies() => F5StatusModule.GetProxies(Instances);
    }
}
