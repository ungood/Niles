using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Niles.Test
{
    public static class ScenarioContextExtensions
    {
        public static T GetOrCreate<T>(this ScenarioContext context, string key)
        {
            return GetOrCreate(context, key, Activator.CreateInstance<T>);
        }

        public static T GetOrCreate<T>(this ScenarioContext context, string key, Func<T> factory)
        {
            T value;
            if(!context.TryGetValue(key, out value))
            {
                value = factory();
                context.Add(key, value);
            }

            return value;
        }
    }
}
