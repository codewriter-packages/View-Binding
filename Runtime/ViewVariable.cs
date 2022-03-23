using System;
using System.Text;
using UniMob;
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
        private MutableAtom<Func<T>> _atomSource = Atom.Value(default(Func<T>));

        public override string TypeDisplayName => typeof(T).Name;

        public override Type Type => typeof(T);

        public T Value
        {
            get
            {
                if (Context.TryGetRootVariableFor<TVariable>(this, out var rootVariable))
                {
                    return rootVariable.Value;
                }

#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    return value;
                }
#endif

                if (_atomSource.Value == null)
                {
                    return value;
                }

                return _atomSource.Value.Invoke();
            }
        }

        public void SetSource(Func<T> source)
        {
            if (Context.TryGetRootVariableFor<TVariable>(this, out var rootVariable))
            {
                rootVariable.SetSource(source);
            }
            else
            {
                _atomSource.Value = source;
            }
        }

        public override bool IsRootVariableFor(ViewVariable viewVariable)
        {
            return Context.TryGetRootVariableFor<TVariable>(viewVariable, out var rootVariable) &&
                   rootVariable == this;
        }

#if UNITY_EDITOR
        public void SetValueEditorOnly(T newValue)
        {
            value = newValue;
        }
#endif
    }

    [Serializable]
    public abstract class ViewVariable : ViewEntry
    {
        public abstract void AppendValueTo(ref StringBuilder builder);

        public abstract bool IsRootVariableFor(ViewVariable viewVariable);

        internal override string GetErrorMessage()
        {
            var error = base.GetErrorMessage();
            if (error != null)
            {
                return error;
            }

            if (!Context.TryGetRootVariableFor<ViewVariable>(this, out _))
            {
                return "Variable is missing";
            }

            return null;
        }
    }
}