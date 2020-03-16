using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Scorpio.Api.Validation
{
    public class SensorDataValidatorsFactory
    {
        public static IEnumerable<ISensorDataValidator> GetValidators(string sensorKey)
        {
            var ret = new List<ISensorDataValidator>();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            var validators = types.Where(x => !x.IsAbstract && !x.IsInterface && typeof(ISensorDataValidator).IsAssignableFrom(x)).ToArray();

            foreach (var validator in validators)
            {
                if (!(Activator.CreateInstance(validator) is ISensorDataValidator concreteValidator)) continue;

                // By starts with we can re-use same validator, i.e.
                // If validator is for 'gps' key, and the entity has 'gps' or 'gps-markers' both will be matched
                if (sensorKey.StartsWith(concreteValidator.SensorKey, StringComparison.InvariantCultureIgnoreCase))
                    ret.Add(concreteValidator);
            }

            return ret;
        }
    }
}