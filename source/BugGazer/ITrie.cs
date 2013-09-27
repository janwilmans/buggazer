// -----------------------------------------------------------------------
// <copyright file="ITrie.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Trie
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Interface for a data structure that implements a Trie.
    /// </summary>
    /// <typeparam name="TKey">Type of the Key to store. Note each entry in the Trie has multiple Keys represented by an IEnumerable.</typeparam>
    /// <typeparam name="TValue">Type of the Value to store. This type must have a public parameterless constructor.</typeparam>
    public interface ITrie<TKey, TValue> : IDictionary<IEnumerable<TKey>, TValue>
    {
        /// <summary>
        /// Find all KeyValuePairs whose keys have the prefix specified by keys
        /// </summary>
        /// <param name="keys">Keys that all results must begin with</param>
        /// <returns>Collection of KeyValuePairs whose Keys property begins with the supplied keys prefix</returns>
        ICollection<KeyValuePair<IEnumerable<TKey>, TValue>> Suffixes(IEnumerable<TKey> keys);
    }
}
