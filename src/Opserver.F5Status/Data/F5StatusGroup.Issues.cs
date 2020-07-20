using System.Collections.Generic;
using System.Linq;
using Opserver.Data;

namespace Opserver.F5Status.Data
{
    public partial class F5StatusGroup : IIssuesProvider
    {
        string IIssuesProvider.Name => "F5Status";

        public IEnumerable<Issue> GetIssues()
        {
            if (MonitorStatus != MonitorStatus.Good && Instances.Any(i => i.LastPoll.HasValue))
            {
                yield return new Issue<F5StatusGroup>(this, "F5Status", Name);
            }
        }
    }
}
