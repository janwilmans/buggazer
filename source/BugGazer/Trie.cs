// -----------------------------------------------------------------------
// http://chris-miceli.blogspot.nl/2012/06/net-trie-in-c-implementing-idictionary.html
// -----------------------------------------------------------------------

namespace Trie
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Implementation of the ITrie interface, representing a collection of keys of keys and values.
    /// </summary>
    /// <typeparam name="TKey">Type of the Key to store. Note each entry in the Trie has multiple Keys represented by an IEnumerable.</typeparam>
    /// <typeparam name="TValue">Type of the Value to store. This type must have a public parameterless constructor.</typeparam>
    public class Trie<TKey, TValue> : ITrie<TKey, TValue> where TValue : new()
    {
        /// <summary>
        /// Number of KeyValuePairs in the trie.
        /// </summary>
        private int count;

        /// <summary>
        /// Keys in the trie.
        /// </summary>
        private List<IEnumerable<TKey>> keys;

        /// <summary>
        /// Values in the trie.
        /// </summary>
        private List<TValue> values;

        /// <summary>
        /// Root node of the trie.
        /// </summary>
        private Node<TKey, TValue> root;

        /// <summary>
        /// Initializes a new instance of the Trie class.
        /// </summary>
        public Trie()
        {
            this.root = new Node<TKey, TValue>();
            this.keys = new List<IEnumerable<TKey>>();
            this.values = new List<TValue>();
        }

        #region ICollectionProperties
        /// <summary>
        /// Gets the number of elements contained in the ICollection.
        /// </summary>
        public int Count
        {
            get
            {
                return this.count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the ICollection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region IDictionaryProperties

        /// <summary>
        /// Gets an ICollection containing the keys of the IDictionary.
        /// </summary>
        public ICollection<IEnumerable<TKey>> Keys
        {
            get
            {
                return this.keys;
            }
        }

        /// <summary>
        /// Gets an ICollection containing the values in the IDictionary.
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                return this.values;
            }
        }
        #endregion

        #region IDictionaryIndexers
        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <param name="keys">The key of the element to get or set.</param>
        /// <returns>The element with the specified key.</returns>
        public TValue this[IEnumerable<TKey> keys]
        {
            get
            {
                if (null == keys)
                {
                    throw new ArgumentNullException("keys cannot be null.");
                }

                Node<TKey, TValue> node = this.FindNode(this.root, keys);
                if (null != node)
                {
                    if (null != node.Value)
                    {
                        return node.Value;
                    }
                }

                throw new KeyNotFoundException();
            }

            set
            {
                if (this.IsReadOnly)
                {
                    throw new NotSupportedException();
                }
                else
                {
                    Node<TKey, TValue> node = this.FindNode(this.root, keys);
                    if (null != node)
                    {
                        node.Value = value;
                    }
                }
            }
        }
        #endregion

        #region ICollectionMethods
        /// <summary>
        /// Adds an item to the ICollection.
        /// </summary>
        /// <param name="item">The object to add to the ICollection.</param>
        public void Add(KeyValuePair<IEnumerable<TKey>, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <summary>
        /// Removes all items from the ICollection.
        /// </summary>
        public void Clear()
        {
            if (false == this.IsReadOnly)
            {
                this.keys.Clear();
                this.values.Clear();
                this.root = new Node<TKey, TValue>();
                this.count = 0;
            }
            else
            {
                throw new NotSupportedException("IDictionary is read only.");
            }
        }

        /// <summary>
        /// Determines whether the ICollection contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the ICollection.</param>
        /// <returns>true if item is found in the ICollection; otherwise, false.</returns>
        public bool Contains(KeyValuePair<IEnumerable<TKey>, TValue> item)
        {
            return this.Contains(item.Key);
        }

        /// <summary>
        /// Copies the elements of the ICollection to an Array, starting at a particular Array index. 
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination of the elements copied from ICollection. The Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(KeyValuePair<IEnumerable<TKey>, TValue>[] array, int arrayIndex)
        {
            if (null == array)
            {
                throw new ArgumentNullException("array cannot be null.");
            }
            else if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex cannot be less than 0.");
            }
            else if (arrayIndex + this.Count > array.Count())
            {
                throw new ArgumentException("The number of elements is greater than the available space from arrayIndex to the end of the destination array.");
            }

            int currentIndex = arrayIndex;
            foreach (KeyValuePair<IEnumerable<TKey>, TValue> entry in this)
            {
                array[currentIndex] = entry;
                currentIndex++;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ICollection. 
        /// </summary>
        /// <param name="item">The object to remove from the ICollection.</param>
        /// <returns>true if item was successfully removed from the ICollection; otherwise, false. This method also returns false if item is not found in the original ICollection.</returns>
        public bool Remove(KeyValuePair<IEnumerable<TKey>, TValue> item)
        {
            return this.Remove(item.Key);
        }
        #endregion

        #region IDictionaryMethods
        /// <summary>
        /// Adds an element with the provided key and value to the IDictionary.
        /// </summary>
        /// <param name="keys">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        public void Add(IEnumerable<TKey> keys, TValue value)
        {
            this.Add(this.root, keys, value);
        }
        
        /// <summary>
        /// Determines whether the IDictionary contains an element with the specified key.
        /// </summary>
        /// <param name="keys">The key to locate in the Dictionary.</param>
        /// <returns>true if the IDictionary contains an element with the key; otherwise, false.</returns>
        public bool ContainsKey(IEnumerable<TKey> keys)
        {
            Node<TKey, TValue> node = this.FindNode(keys);
            return node != null && node.Value != null;
        }

        /// <summary>
        /// Removes the element with the specified key from the IDictionary.
        /// </summary>
        /// <param name="keys">The key of the element to remove.</param>
        /// <returns>true if the element is successfully removed; otherwise, false. This method also returns false if key was not found in the original IDictionary.</returns>
        public bool Remove(IEnumerable<TKey> keys)
        {
            if (null == keys)
            {
                throw new ArgumentNullException();
            }
            else if (this.IsReadOnly)
            {
                throw new NotSupportedException();
            }

            // Find parent by removing last key
            Node<TKey, TValue> node = this.FindNode(keys);
            if (null == node)
            {
                return false;
            }

            // Make sure we are not root node
            if (null == node.Parent)
            {
                return false;
            }

            bool result = node.Parent.Remove(node.Key);

            // Get rid of any leaf nodes that don't correspond to any keys
            this.Prune();
            return result;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="keys">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>true if the object that implements IDictionary contains an element with the specified key; otherwise, false.</returns>
        public bool TryGetValue(IEnumerable<TKey> keys, out TValue value)
        {
            Node<TKey, TValue> node = this.FindNode(keys);
            if (null == node)
            {
                value = default(TValue);
                return false;
            }
            else
            {
                if (null != node.Value)
                {
                    value = node.Value;
                    return true;
                }
                else
                {
                    value = default(TValue);
                    return false;
                }
            }
        }
        #endregion

        #region ITrieMethods
        /// <summary>
        /// Find all KeyValuePairs whose keys have the prefix specified by keys
        /// </summary>
        /// <param name="keys">Keys that all results must begin with</param>
        /// <returns>Collection of KeyValuePairs whose Keys property begins with the supplied keys prefix</returns>
        public ICollection<KeyValuePair<IEnumerable<TKey>, TValue>> Suffixes(IEnumerable<TKey> keys)
        {
            if (null == keys)
            {
                throw new ArgumentNullException();
            }

            ICollection<KeyValuePair<IEnumerable<TKey>, TValue>> result = new List<KeyValuePair<IEnumerable<TKey>, TValue>>();
            if (0 == keys.Count())
            {
                return result;
            }

            Node<TKey, TValue> node = this.FindNode(this.root, keys);
            if (null == node)
            {
                return result;
            }

            TrieEnumerator enumerator = new TrieEnumerator(node);
            while (enumerator.MoveNext())
            {
                result.Add(enumerator.Current);
            }

            return result;
        }
        #endregion

        #region IEnumerableMethods
        /// <summary>
        /// Returns an enumerator that iterates through the collection. 
        /// </summary>
        /// <returns>A IEnumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<IEnumerable<TKey>, TValue>> GetEnumerator()
        {
            return new TrieEnumerator(this.root);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion

        #region PrivateMethods
        /// <summary>
        /// Determines whether a node exists in the Trie with given Keys
        /// </summary>
        /// <param name="keys">Keys identifying the node</param>
        /// <returns>True if the node was found, false otherwise.</returns>
        private bool Contains(IEnumerable<TKey> keys)
        {
            Node<TKey, TValue> node = this.FindNode(keys);
            if (null == node)
            {
                return false;
            }
            else
            {
                return null != node.Value;
            }
        }

        /// <summary>
        /// Add a new node to the Trie, beginning at the given node.
        /// </summary>
        /// <param name="node">Node to begin addtion at.</param>
        /// <param name="keys">Keys of node to be created</param>
        /// <param name="value">Value of node to be created</param>
        private void Add(Node<TKey, TValue> node, IEnumerable<TKey> keys, TValue value)
        {
            if (null == node)
            {
                throw new ArgumentNullException("node cannot be null.");
            }
            else if (null == keys)
            {
                throw new ArgumentNullException("node cannot be null.");
            }
            else if (null == value)
            {
                throw new ArgumentNullException("value cannot be null.");
            }

            TKey key = keys.FirstOrDefault();
            if (null == key)
            {
                throw new ArgumentNullException("key cannot be null.");
            }

            Node<TKey, TValue> childNode = node.Children.Where(child => child.Key.Equals(key)).FirstOrDefault();
            if (null != childNode)
            {
                this.Add(childNode, keys.Skip(1), value);
            }
            else
            {
                Node<TKey, TValue> newNode;
                int keysCount = keys.Count();
                if (1 == keysCount)
                {
                    // Insert last new node
                    newNode = new Node<TKey, TValue>(key, value);
                    node.Add(newNode);
                    this.count++;
                    this.keys.Add(keys);
                    this.values.Add(value);
                }
                else if (0 == keysCount)
                {
                    // Add to this node
                    if (null != node.Value)
                    {
                        throw new InvalidOperationException("Duplicate Key!");
                    }
                    else
                    {
                        node.Value = value;
                    }
                }
                else
                {
                    newNode = new Node<TKey, TValue>(key);
                    node.Add(newNode);
                    this.Add(newNode, keys.Skip(1), value);
                }
            }
        }

        /// <summary>
        /// Find a node, given keys
        /// </summary>
        /// <param name="keys">Keys of node to look for</param>
        /// <returns>The node with the Keys as given, null otherwise.</returns>
        private Node<TKey, TValue> FindNode(IEnumerable<TKey> keys)
        {
            return this.FindNode(this.root, keys);
        }

        /// <summary>
        /// Find a node, given keys and node to being search on.
        /// </summary>
        /// <param name="node">Node to begin the search on.</param>
        /// <param name="keys">Keys of the node to look for.</param>
        /// <returns>The found node, null otherwise.</returns>
        private Node<TKey, TValue> FindNode(Node<TKey, TValue> node, IEnumerable<TKey> keys)
        {
            if (null == node)
            {
                throw new ArgumentNullException("node may not be null.");
            }
            else if (null == keys)
            {
                throw new ArgumentNullException("keys may not be null.");
            }

            TKey key = keys.FirstOrDefault();
            if (null == key)
            {
                throw new ArgumentNullException("keys may not be empty.");
            }
            else
            {
                IEnumerable<Node<TKey, TValue>> children = node.Children.Where(child => child.Key.Equals(key));

                if (1 < children.Count())
                {
                    throw new InvalidOperationException(
                        string.Format("The trie has more than one children with a specified key {0}", key.ToString()));
                }
                else if (0 == children.Count())
                {
                    return null;
                }
                else
                {
                    // 1 == children.Count
                    Node<TKey, TValue> child = children.Single();

                    if (1 == keys.Count())
                    {
                        return child;
                    }
                    else
                    {
                        if (keys.Count() >= 2)
                        {
                            return this.FindNode(child, keys.Skip(1));
                        }
                        else
                        {
                            // This condition shouldn't be triggered
                            throw new InvalidOperationException("Not enough keys to complete lookup.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Remove nodes with no children with Value not set.
        /// </summary>
        private void Prune()
        {
            this.Prune(this.root);
        }

        /// <summary>
        /// Remove nodes with no children with Value not set beginning with the given node.
        /// </summary>
        /// <param name="node">Node to begin pruning on.</param>
        private void Prune(Node<TKey, TValue> node)
        {
            if (node.Children.Count() == 0 && null == node.Value)
            {
                node.Parent.Remove(node.Key);
            }
            else
            {
                foreach (Node<TKey, TValue> child in node.Children)
                {
                    this.Prune(child);
                }
            }
        }

        /// <summary>
        /// Iterator for the trie data structure
        /// </summary>
        private class TrieEnumerator : IEnumerator<KeyValuePair<IEnumerable<TKey>, TValue>>
        {
            /// <summary>
            /// Node to begin iteration on.
            /// </summary>
            private Node<TKey, TValue> node;

            /// <summary>
            /// Queue of all elements in the trie, used for enumeration.
            /// TODO improve enumeration to reduce memory footprint.
            /// </summary>
            private Queue<KeyValuePair<IEnumerable<TKey>, TValue>> elements;

            /// <summary>
            /// Initializes a new instance of the TrieEnumerator class.
            /// </summary>
            /// <param name="node">Node in the trie to begin enumeration on.</param>
            public TrieEnumerator(Node<TKey, TValue> node)
            {
                this.node = node;
            }

            #region IEnumeratorProperties
            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            public KeyValuePair<IEnumerable<TKey>, TValue> Current
            {
                get
                {
                    if (null == this.elements)
                    {
                        throw new InvalidOperationException();
                    }

                    return this.elements.Peek();
                }
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }
            #endregion

            #region IEnumeratorMethods
            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
                return;
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
            public bool MoveNext()
            {
                if (null == this.elements)
                {
                    this.Reset();
                }
                else if (this.elements.Count == 0)
                {
                    return false;
                }
                else
                {
                    this.elements.Dequeue();
                }

                if (this.elements.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                this.elements = new Queue<KeyValuePair<IEnumerable<TKey>, TValue>>();
                this.InorderTraversal(this.node);
            }
            #endregion

            #region PrivateMethods
            /// <summary>
            /// Recursively traverse the Trie, adding encountered nodes to a queue for enumeration later.
            /// </summary>
            /// <param name="node">Node to begin the traversal at.</param>
            private void InorderTraversal(Node<TKey, TValue> node)
            {
                if (null != node.Value)
                {
                    this.elements.Enqueue(new KeyValuePair<IEnumerable<TKey>, TValue>(node.Keys, node.Value));
                }

                foreach (Node<TKey, TValue> child in node.Children)
                {
                    this.InorderTraversal(child);
                }
            }
            #endregion
        }

        /// <summary>
        /// Internal class that build the tree. Not all nodes represent entries in the Trie as the value may not be set.
        /// </summary>
        /// <typeparam name="K">Type of the Key for this node.</typeparam>
        /// <typeparam name="V">Type of the Value for this node. This type must have a public parameterless constructor.</typeparam>
        private class Node<K, V> where V : new()
        {
            /// <summary>
            /// The child nodes in sorted order.
            /// </summary>
            private SortedList<K, Node<K, V>> children;

            /// <summary>
            /// Initializes a new instance of the Node class.
            /// Usually used for the root node of the Trie.
            /// </summary>
            public Node()
            {
                this.Key = default(K);
                this.children = new SortedList<K, Node<K, V>>();
            }

            /// <summary>
            /// Initializes a new instance of the Node class. 
            /// Constructs a node with no Value. Useful for intermediate nodes in the Trie.
            /// </summary>
            /// <param name="key">Key in the Trie</param>
            public Node(K key)
            {
                this.Key = key;
                this.children = new SortedList<K, Node<K, V>>();
            }

            /// <summary>
            /// Initializes a new instance of the Node class. 
            /// </summary>
            /// <param name="key">Key in the Trie</param>
            /// <param name="value">Value in the Trie</param>
            public Node(K key, V value)
            {
                this.Key = key;
                this.Value = value;
                this.children = new SortedList<K, Node<K, V>>();
            }

            /// <summary>
            /// Gets or sets the Key for this given node.
            /// </summary>
            public K Key
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the Value for this node.
            /// The value is null for non data storing nodes.
            /// </summary>
            public V Value
            {
                get;
                set;
            }

            /// <summary>
            /// Gets the children nodes in the Trie. All children have the prefix Keys.
            /// </summary>
            public List<Node<K, V>> Children
            {
                get
                {
                    return this.children.Select(child => child.Value).ToList();
                }
            }

            /// <summary>
            /// Gets the parent node in the Trie or null for the root node.
            /// </summary>
            public Node<K, V> Parent
            {
                get;
                private set;
            }

            /// <summary>
            /// Gets the Keys that will define this node. Note Keys may not represent an entry in the Trie as Value may be null.
            /// </summary>
            public IEnumerable<K> Keys
            {
                get
                {
                    if (null != this.Parent)
                    {
                        List<K> result = new List<K>(this.Parent.Keys);
                        result.Add(this.Key);
                        return result;
                    }
                    else
                    {
                        return new List<K>() { this.Key };
                    }
                }
            }

            /// <summary>
            /// Add a child to this node
            /// </summary>
            /// <param name="node">The Node to add as a child.</param>
            public void Add(Node<K, V> node)
            {
                this.children.Add(node.Key, node);
                node.Parent = this;
            }

            /// <summary>
            /// Remove a child from this node.
            /// </summary>
            /// <param name="key">The node to remove from the children of this node.</param>
            /// <returns>Returns whether the result was successful or not.</returns>
            public bool Remove(K key)
            {
                return this.children.Remove(key);
            }
        }
        #endregion
    }
}
