using System;
using System.Collections.Generic;

namespace EventBus
{
    public static class EventBus<T> where T : IEvent
    {
        static readonly Dictionary<int, EventBinding<T>> bindings = new();
        static void Clear()
        {
            bindings.Clear();
        }
        /// <summary>
        /// Raise binding 0.
        /// </summary>
        /// <param name="event">The value of the IEvent parameter.</param>
        /// <returns>True if the binding was found and raised.</returns>
        public static bool Raise(T @event)
        {
            return Raise(0, @event);
        }
        /// <summary>
        /// Raise a binding.
        /// </summary>
        /// <param name="bindingId">The id of the binding.</param>
        /// <param name="event">The value of the IEvent parameter.</param>
        /// <returns>True if the binding was found and raised.</returns>
        public static bool Raise(int bindingId, T @event)
        {
            if (bindings.TryGetValue(bindingId, out var binding))
            {
                binding.Invoke(@event);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add a new binding.
        /// </summary>
        /// <param name="id">The id of the new binding. Defaults to 0.</param>
        /// <returns>True if the binding was successfully added.</returns>
        public static bool AddBinding(int id = 0)
        {
            return bindings.TryAdd(id, new());
        }
        /// <summary>
        /// Clear and then remove an existing binding.
        /// </summary>
        /// <param name="id">The id of the binding to remove.</param>
        /// <returns>True if the binding was successfully removed.</returns>
        public static bool RemoveBinding(int id = 0)
        {
            if (bindings.TryGetValue(id, out var binding))
            {
                binding.Clear();
                bindings.Remove(id);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Clears an existing binding.
        /// </summary>
        /// <param name="id">The id of the binding to clear. Defaults to 0.</param>
        /// <returns>True if the binding was found.</returns>
        public static bool ClearBinding(int id = 0)
        {
            if (bindings.TryGetValue(id, out var binding))
            {
                binding.Clear();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add Actions to binding 0. By default will attempt to add a new binding if none is found, and add the Actions to it.
        /// </summary>
        /// <param name="action">Parametrized action.</param>
        /// <param name="actionNoArgs">Non-parametrized action.</param>
        /// <param name="addBinding">If true, will attempt to add a new binding if none is found, and add the Actions to it. Defaults to true.</param>
        /// <returns>True if the binding was found, or if it was successfully added.</returns>
        public static bool AddActions(Action<T> action,
            Action actionNoArgs = null, bool addBinding = true)
        {
            return AddActions(0, action, actionNoArgs, addBinding);
        }
        /// <summary>
        /// Add Actions to a binding. By default will attempt to add a new binding if none is found, and add the Actions to it.
        /// </summary>
        /// <param name="bindingId">The id of the binding.</param>
        /// <param name="action">Parametrized action.</param>
        /// <param name="actionNoArgs">Non-parametrized action.</param>
        /// <param name="addBinding">If true, will attempt to add a new binding if none is found, and add the Actions to it. Defaults to true.</param>
        /// <returns>True if the binding was found, or if it was successfully added.</returns>
        public static bool AddActions(int bindingId, Action<T> action,
            Action actionNoArgs = null, bool addBinding = true)
        {
            if (bindings.TryGetValue(bindingId, out var binding))
            {
                binding.Add(action);
                binding.Add(actionNoArgs);
                return true;
            }
            if (addBinding && bindings.TryAdd(bindingId, new()))
            {
                binding = bindings[bindingId];
                binding.Add(action);
                binding.Add(actionNoArgs);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Removes Actions from binding 0.
        /// </summary>
        /// <param name="action">Parametrized action.</param>
        /// <param name="actionNoArgs">Non-parametrized action.</param>
        /// <returns></returns>
        public static bool RemoveActions(Action<T> action,
            Action actionNoArgs = null)
        {
            return RemoveActions(0, action, actionNoArgs);
        }
        /// <summary>
        /// Removes Actions from a binding.
        /// </summary>
        /// <param name="bindingId">The id of the binding.</param>
        /// <param name="action">Parametrized action.</param>
        /// <param name="actionNoArgs">Non-parametrized action.</param>
        /// <returns></returns>
        public static bool RemoveActions(int bindingId, Action<T> action,
            Action actionNoArgs = null)
        {
            if (bindings.TryGetValue(bindingId, out var binding))
            {
                binding.Remove(action);
                binding.Remove(actionNoArgs);
                return true;
            }
            return false;
        }
    }
}