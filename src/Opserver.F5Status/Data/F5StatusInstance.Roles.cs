﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Opserver.Data;

namespace Opserver.F5Status.Data
{
    public partial class F5StatusInstance
    {
        public IEnumerable<NodeRole> GetRoles(string node)
        {
            var data = Proxies.Data;
            if (data == null) yield break;

            foreach (var p in data)
            {
                if (p.Servers == null) continue;
                foreach (var s in p.Servers)
                {
                    if (node.IsNullOrEmpty() || string.Equals(s.Name, node, System.StringComparison.OrdinalIgnoreCase))
                    {
                        yield return new NodeRole
                        {
                            Service = "F5Status",
                            Description = $"{Name} - {Group?.Name ?? "(No Group)"} - {p.NiceName}",
                            Active = s.MonitorStatus == MonitorStatus.Good,
                            Node = node.HasValue() ? null : s.Name,
                            SiblingsActive = p.Servers.Count(ps => ps != s && ps.MonitorStatus == MonitorStatus.Good),
                            SiblingsInactive = p.Servers.Count(ps => ps != s && ps.MonitorStatus != MonitorStatus.Good)
                        };
                    }
                }
            }
        }

        public Task<bool> EnableAsync(string node) => Module.Admin.PerformServerActionAsync(node, Action.Ready);
        public Task<bool> DisableAsync(string node) => Module.Admin.PerformServerActionAsync(node, Action.Drain);
    }
}
