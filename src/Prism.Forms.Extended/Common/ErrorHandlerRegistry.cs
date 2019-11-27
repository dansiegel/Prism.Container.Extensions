using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Prism.Forms.Extended.Common
{
    public class ErrorHandlerRegistry : IDictionary<Type, Action<Exception>>
    {
        private readonly Dictionary<Type, Action<Exception>> _handlers = new Dictionary<Type, Action<Exception>>();

        public Action<Exception> this[Type key]
        {
            get => _handlers[key];
            set => _handlers[key] = value;
        }

        public Action<Exception> CatchAll { get; set; }
        public ICollection<Type> Keys => _handlers.Keys;
        public ICollection<Action<Exception>> Values => _handlers.Values;
        public int Count => _handlers.Count;
        public bool IsReadOnly => false;

        public void Add(Type key, Action<Exception> value) =>
            _handlers.Add(key, value);

        public void Add(KeyValuePair<Type, Action<Exception>> item) =>
            Add(item.Key, item.Value);

        public void Clear() => _handlers.Clear();

        public bool Contains(KeyValuePair<Type, Action<Exception>> item) =>
            _handlers.Any(x => x.Equals(item));

        public bool ContainsKey(Type key) =>
            _handlers.ContainsKey(key);

        public void CopyTo(KeyValuePair<Type, Action<Exception>>[] array, int arrayIndex)
        {
            var dict = (IDictionary)_handlers;
            dict.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<Type, Action<Exception>>> GetEnumerator() =>
            _handlers.GetEnumerator();

        public bool HandledException(Exception ex)
        {
            if (ex is null) return true;

            var type = ex.GetType();
            foreach(var key in Keys)
            {
                if(type.IsAssignableFrom(key))
                {
                    var handler = this[key];
                    handler?.Invoke(ex);
                    return true;
                }
            }

            if(ex is AggregateException ae)
            {
                var results = ae.InnerExceptions.Select(e => HandledException(e));
                return results.Where(x => x == false).Count() == 0;
            }

            if (CatchAll is null)
            {
                return false;
            }

            CatchAll.Invoke(ex);
            return true;
        }

        public bool Remove(Type key) =>
            _handlers.Remove(key);

        public bool Remove(KeyValuePair<Type, Action<Exception>> item) =>
            _handlers.Remove(item.Key);

        public bool TryGetValue(Type key, out Action<Exception> value) =>
            _handlers.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() =>
            _handlers.GetEnumerator();
    }
}
