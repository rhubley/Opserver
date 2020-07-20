﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Opserver.Controllers;

namespace Opserver.Helpers
{
    public class NavTab
    {
        public StatusModule Module { get; }
        public string Name => Module.Name;
        public string Route { get; }
        public int Order { get; set; } = 0;

        public int? BadgeCount => (Module as IOverallStatusCount)?.Count;
        public string Tooltip => (Module as IOverallStatusCount)?.Tooltip;

        /// <summary>
        /// Enabled if the module is active and it either has no security, or the current user can see it.
        /// </summary>
        public bool IsEnabled => Module.Enabled && (Module.SecuritySettings == null || Current.User.HasAccess(Module));

        public NavTab(StatusModule module, string route)
        {
            Module = module;
            Route = route;
        }

        public static List<NavTab> AllTabs { get; private set; }
        private static Dictionary<Type, NavTab> _controllerMappings = new Dictionary<Type, NavTab>();

        public static NavTab Get(StatusController c) => _controllerMappings.TryGetValue(c.GetType(), out var tab) ? tab : null;
        public static NavTab GetByName(string tabName) => AllTabs.Find(t => t.Name == tabName);

        /// <summary>
        /// https://www.youtube.com/watch?v=JnbfuAcCqpY
        /// </summary>
        public static void ConfigureAll(IEnumerable<StatusModule> modules)
        {
            var allTabs = new List<NavTab>();
            var mappings = new Dictionary<Type, NavTab>();
            var moduleControllerTypes = Assembly.GetEntryAssembly().GetTypes()
                .Where(t => t.BaseType?.IsGenericType ?? false);

            foreach (var controllerType in moduleControllerTypes)
            {
                // Get the module type for this controller
                var controllerModuleType = controllerType.BaseType.GetGenericArguments()[0];
                // Get the active module this controller goes with and make a NavTab based on it
                var module = modules.FirstOrDefault(m => m.GetType() == controllerModuleType);

                // Was the type a StatusModule?
                if (module != null && typeof(StatusModule).IsAssignableFrom(controllerModuleType))
                {
                    // Get all the potential actions
                    foreach (var method in controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public))
                    {
                        // Check for [DefaultRoute]
                        var defaultRoute = method.GetCustomAttribute<DefaultRoute>();
                        if (defaultRoute != null)
                        {
                            // Do you believe in maaaaaaaaagic?
                            var tab = new NavTab(module, "~/" + defaultRoute.Template);
                            allTabs.Add(tab);
                            mappings.Add(controllerType, tab);
                        }
                    }
                }
            }
            _controllerMappings = mappings;
            allTabs.Sort((a, b) => a.Order.CompareTo(b.Order));

            AllTabs = allTabs;
        }
    }

    //public static class NavTabExtensions
    //{
    //    public static IServiceCollection AddNavTabs(this IServiceCollection services, IConfiguration _configuration)
    //    {
    //        // TODO: Discovery instead

    //        services.Configure<HAProxySettings>(_configuration.GetSection("HAProxy"))
    //                .AddSingleton<Data.HAProxy.HAProxyModule>();

    //        return services;
    //    }
    //}
}
