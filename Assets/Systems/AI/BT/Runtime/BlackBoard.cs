using System;
using System.Collections.Generic;

namespace BT
{
    public class BlackBoard
    {
        /// <summary>
        /// This is a wrapper for a resource which decorators can keep track of.
        /// </summary>
        public class Data
        {
            protected object currentValue;
            public object Value
            {
                get
                {
                    return currentValue;
                }
                set
                {
                    //this is used so listeners know when the value is changed
                    currentValue = value;
                    if (OnValueChanged != null)
                    {
                        OnValueChanged();
                    }
                }
            }

            public event Action OnValueChanged;
            public Data(object value)
            {
                Value = value;
            }
        }
        protected Dictionary<int, Data> data = new();
        /// <summary>
        /// Set the value of a resource given by a key. If the key cannot be found, 
        /// a new pair will be added to the internal HashMap.
        /// </summary>
        /// <typeparam name="T">The datatype of the resource.</typeparam>
        /// <param name="key">The key which will identify the resource.</param>
        /// <param name="value">The value of the resource.</param>
        public void SetData<T>(string key, T value)
        {
            int hash = key.GetHashCode();
            SetData(hash, value);
        }
        /// <summary>
        /// Set the value of a resource given by a key. If the key cannot be found, 
        /// a new pair will be added to the internal HashMap.
        /// </summary>
        /// <typeparam name="T">The datatype of the resource.</typeparam>
        /// <param name="hash">The key which will identify the resource.</param>
        /// <param name="value">The value of the resource.</param>
        public void SetData<T>(int hash, T value)
        {
            Data resource;
            if (data.TryGetValue(hash, out resource))
            {
                resource.Value = value;
            }
            else
            {
                data.Add(hash, new Data(value));
            }
        }
        /// <summary>
        /// When the value of the resource given by the key changes, the given action will fire.
        /// </summary>
        /// <param name="action">Action to execute when the value of the resource changes.</param>
        /// <param name="key">The key for the resource.</param>
        public void AddListener(Action action, string key)
        {
            AddListener(action, key.GetHashCode());
        }
        /// <summary>
        /// When the value of the resource given by the key changes, the given action will fire.
        /// </summary>
        /// <param name="action">Action to execute when the value of the resource changes.</param>
        /// <param name="hash">The hash of the resource.</param>
        public void AddListener(Action action, int hash)
        {
            Data resource;
            if (data.TryGetValue(hash, out resource))
            {
                resource.OnValueChanged += action;
            }
        }
        /// <summary>
        /// Remove an action that was tied to this resource.
        /// </summary>
        /// <param name="action">Action to disconnect from the resource.</param>
        /// <param name="key">The key of the resource.</param>
        public void RemoveListener(Action action, string key)
        {
            RemoveListener(action, key.GetHashCode());
        }
        /// <summary>
        /// Remove an action that was tied to this resource.
        /// </summary>
        /// <param name="action">Action to disconnect from the resource.</param>
        /// <param name="hash">The hash of the resource.</param>
        public void RemoveListener(Action action, int hash)
        {
            Data resource;
            if (data.TryGetValue(hash, out resource))
            {
                resource.OnValueChanged -= action;
            }
        }
        /// <summary>
        /// Get data stored at this key.
        /// </summary>
        /// <typeparam name="T">The datatype of the resource.</typeparam>
        /// <param name="key">The key which identifies the resource.</param>
        /// <returns>Returns the value of the resource if the key is found, 
        /// or the default value otherwise.</returns>
        public T GetData<T>(string key)
        {
            Data resource;
            if (data.TryGetValue(key.GetHashCode(), out resource))
            {
                return (T)resource.Value;
            }
            return default;
        }
        /// <summary>
        /// Get data stored at this key.
        /// </summary>
        /// <typeparam name="T">The datatype of the resource.</typeparam>
        /// <param name="hash">The key which identifies the resource.</param>
        /// <returns>Returns the value of the resource if the key is found, 
        /// or the default value otherwise.</returns>
        public T GetData<T>(int hash)
        {
            Data resource;
            if (data.TryGetValue(hash, out resource))
            {
                return (T)resource.Value;
            }
            return default;
        }
        public BlackBoard()
        {
            data = new();
        }
    }
}