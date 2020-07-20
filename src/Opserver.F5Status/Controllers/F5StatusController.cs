using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Opserver.F5Status.Data;
using Opserver.Helpers;
using Opserver.F5Status.Views;

namespace Opserver.Controllers
{
    [OnlyAllow(F5StatusRoles.Viewer)]
    public partial class F5StatusController : StatusController<F5StatusModule>
    {
        public F5StatusController(F5StatusModule module, IOptions<OpserverSettings> settings) : base(module, settings) { }

        [DefaultRoute("F5Status")]
        public ActionResult Dashboard(string group, string node, string watch = null, bool norefresh = false)
        {
            var haGroup = Module.GetGroup(group ?? node);
            var proxies = haGroup != null ? haGroup.GetProxies() : Module.GetAllProxies();
            proxies.RemoveAll(p => !p.HasServers);

            var vd = new F5StatusModel
            {
                SelectedGroup = haGroup,
                Groups = haGroup != null ? new List<F5StatusGroup> { haGroup } : Module.Groups,
                Proxies = proxies,
                View = F5StatusModel.Views.Dashboard,
                Refresh = !norefresh,
                WatchProxy = watch
            };
            return View("F5Status.Dashboard", vd);
        }
    }
}
