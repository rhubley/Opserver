using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Opserver.F5Status.Data;
using Opserver.Helpers;

namespace Opserver.Controllers
{
    public partial class F5StatusController
    {
        [Route("f5status/admin/action"), HttpPost, OnlyAllow(F5StatusRoles.Admin)]
        public async Task<ActionResult> F5StatusAdminProxy(string group, string proxy, string server, Action act)
        {
            // Entire server
            if (proxy.IsNullOrEmpty() && group.IsNullOrEmpty() && server.HasValue())
                return Json(await Module.Admin.PerformServerActionAsync(server, act));
            // Entire group
            if (proxy.IsNullOrEmpty() && server.IsNullOrEmpty() && group.HasValue())
                return Json(await Module.Admin.PerformGroupActionAsync(group, act));

            var haGroup = Module.GetGroup(group);
            var proxies = (haGroup != null ? haGroup.GetProxies() : Module.GetAllProxies()).Where(pr => pr.Name == proxy);

            return Json(await Module.Admin.PerformProxyActionAsync(proxies, server, act));
        }
    }
}
