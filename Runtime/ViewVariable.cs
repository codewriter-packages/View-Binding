using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    [Serializable]
    public abstract class ViewVariable<T, TVariable> : ViewVariable
        where TVariable : ViewVariable<T, TVariable>
    {
        [SerializeField]
        private T value;

        [NonSerialized]
        private TVariable _linkedSource;

        public override string TypeDisplayName => typeof(T).Name;

        public override Type Type => typeof(T);

        public T Value
        {
            get => _linkedSource != null ? _linkedSource.Value : value;
            set
            {
                if (Context == null)
                {
                    Debug.LogError("Context is null");
                    return;
                }

                if (_linkedSource == null && Context.CanLink(this, out var source))
                {
                    _linkedSource = (TVariable) source;
                }

                if (_linkedSource != null)
                {
                    _linkedSource.Value = value;
                }
                else
                {
                    if (EqualityComparer<T>.Default.Equals(this.value, value))
                    {
                        return;
                    }

                    this.value = value;

                    Context.OnVariableChanged(this);
                }
            }
        }

        public override bool IsLinkedTo(ViewVariable variable)
        {
            return _linkedSource == variable;
        }

        public override void Subscribe(IViewContextListener linkable)
        {
            if (Context == null)
            {
                Debug.LogError("Cannot link: context is null");
                return;
            }

            var newSource = (TVariable) Context.Link(this, linkable);

            if (newSource != _linkedSource)
            {
                _linkedSource = newSource;

                linkable.OnContextVariableChanged(newSource);
            }
        }
    }

    [Serializable]
    public abstract class ViewVariable
    {
        [SerializeField]
        private ViewContextBase context;

        [SerializeField]
        private string name;

        public abstract string TypeDisplayName { get; }

        public abstract Type Type { get; }

        public string Name => name;

        protected ViewContextBase Context => context;

        public abstract void Subscribe(IViewContextListener listener);

        public abstract bool IsLinkedTo(ViewVariable variable);

        public void SetName(string newName)
        {
            name = newName;
        }

        public void SetContext(ViewContextBase newContext)
        {
            context = newContext;
        }

        internal virtual string GetErrorMessage()
        {
            if (context == null)
            {
                return "Context is null";
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return "Variable is none";
            }

            if (!context.CanLink(this, out _))
            {
                return "Variable is missing";
            }

            return null;
        }
    }
}