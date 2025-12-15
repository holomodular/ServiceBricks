using System.Reflection;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace ServiceBricks
{
    public static class JsonPatchHelper
    {
        /// <summary>
        /// Helper to create a json patch given two objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static JsonPatchDocument<T> CreatePatch<T>(T source, T destination)
            where T : class
        {
            if (source == null || destination == null)
                return null;

            var patchDoc = new JsonPatchDocument<T>();

            var type = typeof(T);
            var props = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.CanRead && p.CanWrite);

            foreach (var prop in props)
            {
                if (string.Compare(prop.Name, "StorageKey", true) == 0 ||
                    string.Compare(prop.Name, "CreateDate", true) == 0 ||
                    string.Compare(prop.Name, "UpdateDate", true) == 0)
                    continue;
                var sourceValue = prop.GetValue(source);
                var destValue = prop.GetValue(destination);

                if (!ValuesEqual(sourceValue, destValue))
                {
                    var path = "/" + prop.Name;
                    patchDoc.Operations.Add(new Operation<T>(
                        op: "replace",
                        path: path,
                        from: null,
                        value: destValue
                    ));
                }
            }

            return patchDoc;
        }

        private static bool ValuesEqual(object a, object b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a == null || b == null) return false;
            return a.Equals(b);
        }
    }
}