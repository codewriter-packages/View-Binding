using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    public abstract class ViewContextBase : ViewBindingBehaviour
    {
#if UNITY_EDITOR
        [NonSerialized]
        private readonly List<IEditorViewContextListener> _editorListeners = new List<IEditorViewContextListener>();

        [NonSerialized]
        private bool _executingNotify;
#endif

        protected internal abstract int VariablesCount { get; }
        protected internal abstract int EventCount { get; }

        protected internal abstract ViewVariable GetVariable(int index);
        protected internal abstract ViewEvent GetEvent(int index);

        internal bool TryGetRootVariableFor<TVariable>(ViewVariable variable, out TVariable rootVariable, bool selfIsOk = false)
            where TVariable : ViewVariable
        {
            for (int index = 0, count = VariablesCount; index < count; index++)
            {
                var other = GetVariable(index);

                if ((selfIsOk || other != variable) &&
                    other.Type == variable.Type &&
                    other.Name == variable.Name &&
                    other is TVariable tOther)
                {
                    rootVariable = tOther;
                    return true;
                }
            }

            rootVariable = default;
            return false;
        }

        internal bool TryGetRootEventFor<TEvent>(ViewEvent evt, out TEvent rootEvent, bool selfIsOk = false)
        {
            for (int index = 0, count = EventCount; index < count; index++)
            {
                var other = GetEvent(index);

                if ((selfIsOk || other != evt) &&
                     other.Type == evt.Type &&
                     other.Name == evt.Name &&
                     other is TEvent tOther)
                {
                    rootEvent = tOther;
                    return true;
                }
            }

            rootEvent = default;
            return false;
        }

#if UNITY_EDITOR
        internal void AddEditorListener(IEditorViewContextListener listener)
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (!_editorListeners.Contains(listener))
            {
                _editorListeners.Add(listener);
            }
        }

        protected internal void NotifyEditorVariableChanged(ViewVariable variable)
        {
            if (Application.isPlaying)
            {
                return;
            }

            if (_executingNotify)
            {
                Debug.LogError($"Cyclic variable dependency of {this}");
                return;
            }

            _editorListeners.RemoveAll(it => it.IsDestroyed);

            _executingNotify = true;

            foreach (var listener in _editorListeners)
            {
                listener.OnEditorContextVariableChanged(variable);
            }

            _executingNotify = false;
        }
#endif
    }

#if UNITY_EDITOR
    public interface IEditorViewContextListener
    {
        bool IsDestroyed { get; }

        void OnEditorContextVariableChanged(ViewVariable variable);
    }

    public static class EditorViewContextListenerExtension
    {
        public static void EditorTrackModificationsOf(this IEditorViewContextListener self, ViewVariable variable)
        {
            if (self == null || variable == null || variable.Context == null)
            {
                return;
            }

            variable.Context.AddEditorListener(self);
        }

        public static void EditorTrackModificationsOf(this IEditorViewContextListener self, ViewContextBase context)
        {
            if (self == null || context == null)
            {
                return;
            }

            context.AddEditorListener(self);
        }

        public static void EditorTrackModificationsOf(this IEditorViewContextListener self, ViewContextBase[] contexts)
        {
            if (self == null || contexts == null)
            {
                return;
            }

            foreach (var context in contexts)
            {
                if (context == null)
                {
                    continue;
                }

                context.AddEditorListener(self);
            }
        }
    }
#endif
}