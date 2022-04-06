using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace CodeWriter.ViewBinding
{
    [Serializable, Preserve]
    public sealed class ViewEventVoid : ViewEvent<ViewEventVoid.Void, ViewEventVoid>
    {
        private List<Action> _listeners;

        public override string TypeDisplayName => "Void";

        [Preserve]
        public ViewEventVoid()
        {
        }

        public void AddListener(Action listener)
        {
            if (Context.TryGetRootEventFor<ViewEventVoid>(this, out var rootEvent))
            {
                rootEvent.AddListener(listener);
                return;
            }

            if (_listeners == null)
            {
                _listeners = new List<Action>();
            }

            if (_listeners.Contains(listener))
            {
                return;
            }

            _listeners.Add(listener);
        }

        public void RemoveListener(Action listener)
        {
            if (Context.TryGetRootEventFor<ViewEventVoid>(this, out var rootEvent))
            {
                rootEvent.RemoveListener(listener);
                return;
            }

            _listeners?.Remove(listener);
        }

        public void Invoke()
        {
            if (Context.TryGetRootEventFor<ViewEventVoid>(this, out var rootEvent))
            {
                rootEvent.Invoke();
                return;
            }

            if (_listeners == null)
            {
                return;
            }

            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].Invoke();
            }
        }

        public class Void
        {
            private Void()
            {
            }
        }
    }

    [Serializable, Preserve]
    public sealed class ViewEventBool : ViewParametrizedEvent<bool, ViewEventBool>
    {
        public override string TypeDisplayName => "Boolean";

        [Preserve]
        public ViewEventBool()
        {
        }
    }

    [Serializable, Preserve]
    public sealed class ViewEventInt : ViewParametrizedEvent<int, ViewEventInt>
    {
        public override string TypeDisplayName => "Integer";

        [Preserve]
        public ViewEventInt()
        {
        }
    }

    [Serializable, Preserve]
    public sealed class ViewEventFloat : ViewParametrizedEvent<float, ViewEventFloat>
    {
        public override string TypeDisplayName => "Float";

        [Preserve]
        public ViewEventFloat()
        {
        }
    }

    [Serializable, Preserve]
    public sealed class ViewEventString : ViewParametrizedEvent<string, ViewEventString>
    {
        public override string TypeDisplayName => "String";

        [Preserve]
        public ViewEventString()
        {
        }
    }

    [Serializable]
    public abstract class ViewParametrizedEvent<T, TEvent> : ViewEvent<T, TEvent>
        where TEvent : ViewParametrizedEvent<T, TEvent>

    {
        private List<Action<T>> _listeners;

        public void AddListener(Action<T> listener)
        {
            if (Context.TryGetRootEventFor<TEvent>(this, out var rootEvent))
            {
                rootEvent.AddListener(listener);
                return;
            }

            if (_listeners == null)
            {
                _listeners = new List<Action<T>>();
            }

            if (_listeners.Contains(listener))
            {
                return;
            }

            _listeners.Add(listener);
        }

        public void RemoveListener(Action<T> listener)
        {
            if (Context.TryGetRootEventFor<TEvent>(this, out var rootEvent))
            {
                rootEvent.RemoveListener(listener);
                return;
            }

            _listeners?.Remove(listener);
        }

        public void Invoke(T value)
        {
            if (Context.TryGetRootEventFor<TEvent>(this, out var rootEvent))
            {
                rootEvent.Invoke(value);
                return;
            }

            if (_listeners == null)
            {
                return;
            }

            for (var i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].Invoke(value);
            }
        }
    }
}