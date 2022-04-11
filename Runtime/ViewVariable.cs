using System;
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
        private MutableAtom<Atom<T>> _atomSourceBacking;

        private MutableAtom<Atom<T>> AtomSource
        {
            get
            {
                if (_atomSourceBacking == null)
                {
                    _atomSourceBacking = Atom.Value(default(Atom<T>), debugName: "_atomSource");
                }

                return _atomSourceBacking;
            }
        }

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
                    if (Context is AdapterBase adapter)
                    {
                        adapter.AdaptEditorOnly();
                    }
                    
                    return value;
                }
#endif

                if (AtomSource.Value == null)
                {
                    return value;
                }

                return AtomSource.Value.Value;
            }
        }

        public void SetSource(Atom<T> source)
        {
            if (Context.TryGetRootVariableFor<TVariable>(this, out var rootVariable))
            {
                rootVariable.SetSource(source);
            }
            else
            {
                AtomSource.Value = source;
            }
        }

        public void SetValue(T newValue)
        {
            if (Context.TryGetRootVariableFor<TVariable>(this, out var rootVariable))
            {
                rootVariable.SetValue(newValue);
            }
            else
            {
                using (Atom.NoWatch)
                {
                    var oldSource = AtomSource.Value;
                    if (oldSource is MutableAtom<T> oldMutableSource)
                    {
                        oldMutableSource.Value = newValue;
                    }
                    else
                    {
                        AtomSource.Value = Atom.Value(newValue);
                    }
                }
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
        public abstract void AppendValueTo(ref ValueTextBuilder builder);

        public abstract bool IsRootVariableFor(ViewVariable viewVariable);

#if UNITY_EDITOR
        public virtual void DoGUI(Rect position, GUIContent label,
            UnityEditor.SerializedProperty property, string variableName)
        {
        }

        public virtual void DoRuntimeGUI(Rect position, GUIContent label, string variableName)
        {
        }
#endif

        internal override string GetErrorMessage()
        {
            var error = base.GetErrorMessage();
            if (error != null)
            {
                return error;
            }

            if (!Context.TryGetRootVariableFor<ViewVariable>(this, out _, selfIsOk: true))
            {
                return "Variable is missing";
            }

            return null;
        }
    }
}