using System.Collections.Generic;

namespace Opserver.F5Status.Data
{
    /// <summary>
    /// Represents an F5Status backend for a proxy
    /// </summary>
    public class Backend : Item
    {
        public List<Server> Servers { get; internal set; }
    }
}
