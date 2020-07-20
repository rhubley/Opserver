﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Opserver.F5Status.Data
{
    public class StatProperty
    {
        /// <summary>
        /// The F5Status Stat name
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// The position F5Status places this attribute in the CSV (parsing order)
        /// </summary>
        public int Position { get; internal set; }
        /// <summary>
        /// The property info for this stat's property, for populating in parsing
        /// </summary>
        public PropertyInfo PropertyInfo { get; internal set; }

        /// <summary>
        /// Creates a StatProperty from a property's PropertyInfo
        /// </summary>
        /// <param name="p">The propertyInfo decorated with a StatAttribute</param>
        public StatProperty(PropertyInfo p)
        {
            var sa = p.GetCustomAttributes(typeof(StatAttribute), false)[0] as StatAttribute;
            Name = sa.Name;
            Position = sa.Position;
            PropertyInfo = p;
        }

        //Load properties to parse initially on load
        public static readonly List<StatProperty> AllOrdered = GetAll();

        private static List<StatProperty> GetAll()
        {
            return typeof(Item).GetProperties()
                   .Where(p => p.IsDefined(typeof(StatAttribute), false))
                   .Select(p => new StatProperty(p))
                   .OrderBy(s => s.Position)
                   .ToList();
        }
    }
}
