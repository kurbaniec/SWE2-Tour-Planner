using System;
using System.Collections.Generic;

namespace Client.Utils.Mediators
{
    /// <summary>
    /// The multi dictionary is a dictionary that contains
    /// more than one value per key
    /// Based on https://www.codeproject.com/Articles/35277/MVVM-Mediator-Pattern by Sacha Barber.
    /// </summary>
    /// <typeparam name="T">The type of the key</typeparam>
    /// <typeparam name="TK">The type of the list contents</typeparam>
    public class MultiDictionary<T, TK>
        : Dictionary<T, List<TK>> where T: notnull
    {

        #region Private Methods

        //checks if the key is already present
        private void EnsureKey(T key)
        {
            if (!ContainsKey(key))
            {
                this[key] = new List<TK>(1);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new value in the Values collection
        /// </summary>
        /// <param name="key">The key where to place the
        /// item in the value list</param>
        /// <param name="newItem">The new item to add</param>
        public void AddValue(T key, TK newItem)
        {
            EnsureKey(key);
            this[key].Add(newItem);
        }


        /// <summary>
        /// Adds a list of values to append to the value collection
        /// </summary>
        /// <param name="key">The key where to place the item in the value list</param>
        /// <param name="newItems">The new items to add</param>
        public void AddValues(T key, IEnumerable<TK> newItems)
        {
            EnsureKey(key);
            this[key].AddRange(newItems);
        }

        /// <summary>
        /// Removes a specific element from the dict
        /// If the value list is empty the key is removed from the dict
        /// </summary>
        /// <param name="key">The key from where to remove the value</param>
        /// <param name="value">The value to remove</param>
        /// <returns>Returns false if the key was not found</returns>
        public bool RemoveValue(T key, TK value)
        {
            if (!ContainsKey(key))
                return false;

            this[key].Remove(value);

            if (this[key].Count == 0)
                this.Remove(key);

            return true;
        }

        /// <summary>
        /// Removes all items that match the predicate
        /// If the value list is empty the key is removed from the dict
        /// </summary>
        /// <param name="key">The key from where to remove the value</param>
        /// <param name="match">The predicate to match the items</param>
        /// <returns>Returns false if the key was not found</returns>
        public bool RemoveAllValue(T key, Predicate<TK> match)
        {
            if (!ContainsKey(key))
                return false;

            this[key].RemoveAll(match);

            if (this[key].Count == 0)
                this.Remove(key);

            return true;
        }

        #endregion
    }
}