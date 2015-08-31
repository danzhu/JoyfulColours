using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JoyfulColours.Library
{
    public static class Extensions
    {
        /// <summary>
        /// Gets the value associated with the specified key. If the key is not found in the
        /// <see cref="IDictionary{TKey, TValue}"/>, the default value is returned.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary</typeparam>
        /// <param name="dict">
        /// A <see cref="Dictionary{TKey, TValue}"/> to get value from.
        /// </param>
        /// <param name="key">The key whose value to get.</param>
        /// <returns></returns>
        public static TValue GetOrDefault<TKey, TValue>
            (this IDictionary<TKey, TValue> dict, TKey key)
        {
            TValue value;
            if (dict.TryGetValue(key, out value))
                return value;
            else
                return default(TValue);
        }

        /// <summary>
        /// Gets the value associated with the specified key. If the key is not found in the
        /// <see cref="IDictionary{TKey, TValue}"/>, a new value is created and added to the
        /// collection.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary</typeparam>
        /// <param name="dict">
        /// A <see cref="Dictionary{TKey, TValue}"/> to get value from.
        /// </param>
        /// <param name="key">The key whose value to get.</param>
        /// <returns></returns>
        public static TValue GetOrNew<TKey, TValue>
            (this IDictionary<TKey, TValue> dict, TKey key)
            where TValue : new()
        {
            TValue value;
            if (dict.TryGetValue(key, out value))
                return value;
            else
                return dict[key] = new TValue();
        }
    }
}
