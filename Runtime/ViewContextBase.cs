using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    public abstract class ViewContextBase : MonoBehaviour
    {
        [NonSerialized]
        private readonly List<IViewContextListener> _listeners = new List<IViewContextListener>();

        internal abstract IEnumerable<ViewVariable> EnumerateVariables();

        internal bool CanLink(ViewVariable variable, out ViewVariable variableToLink)
        {
            foreach (var other in EnumerateVariables())
            {
                if (other != variable && other.Type == variable.Type && other.Name == variable.Name)
                {
                    variableToLink = other;
                    return true;
                }
            }

            variableToLink = default;
            return false;
        }

        internal ViewVariable Link(ViewVariable variable, IViewContextListener listener)
        {
            if (!CanLink(variable, out var variableToLink))
            {
                return null;
            }

            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
            }

            return variableToLink;
        }

        protected internal void OnVariableChanged(ViewVariable variable)
        {
            foreach (var listener in _listeners)
            {
                listener.OnContextVariableChanged(variable);
            }
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnValidate()
        {
        }
    }

    public interface IViewContextListener
    {
        void OnContextVariableChanged(ViewVariable variable);
    }
}