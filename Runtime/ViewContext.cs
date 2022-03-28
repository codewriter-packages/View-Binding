using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    [AddComponentMenu("View Binding/View Context")]
    public class ViewContext : ViewContextBase
    {
        [SerializeField]
        private List<ViewBindingBehaviour> listeners = new List<ViewBindingBehaviour>();

        [SerializeReference]
        private List<ViewVariable> vars = new List<ViewVariable>();

        [SerializeReference]
        private List<ViewEvent> evts = new List<ViewEvent>();

        protected internal override int VariablesCount => vars.Count;
        protected internal override int EventCount => evts.Count;

        protected internal override ViewVariable GetVariable(int index) => vars[index];
        protected internal override ViewEvent GetEvent(int index) => evts[index];

        protected virtual void Start()
        {
            foreach (var listener in listeners)
            {
                if (listener == null)
                {
                    Debug.LogError($"Null listener at {name}");
                    continue;
                }

                listener.OnContextStart();
            }
        }

        protected virtual void OnDestroy()
        {
            foreach (var listener in listeners)
            {
                if (listener == null)
                {
                    continue;
                }

                listener.OnContextDestroy();
            }
        }

        protected void UnsafeRegisterVariable(ViewVariable variable)
        {
            vars.Add(variable);
        }

        public ViewVariable FindVariable(string variableName) => FindVariable<ViewVariable>(variableName);

        public TViewVariable FindVariable<TViewVariable>(string variableName)
            where TViewVariable : ViewVariable
        {
            foreach (var variable in vars)
            {
                if (variable.Name == variableName && variable is TViewVariable tVariable)
                {
                    return tVariable;
                }
            }

            return null;
        }

        public ViewEvent FindEvent(string eventName) => FindEvent<ViewEvent>(eventName);

        public TViewEvent FindEvent<TViewEvent>(string eventName)
            where TViewEvent : ViewEvent
        {
            foreach (var evt in evts)
            {
                if (evt.Name == eventName && evt is TViewEvent tVariable)
                {
                    return tVariable;
                }
            }

            return null;
        }

        public void FillListeners()
        {
            listeners = Enumerable.Empty<ViewBindingBehaviour>()
                .Concat(gameObject.GetComponentsInChildren<ViewBindingBehaviour>(true))
                .Where(it => it != null)
                .Where(it => !(it is ViewContext))
                .ToList();

            listeners.RemoveAll(it => it.GetComponentInParent<ViewContext>() != this);
        }
    }
}