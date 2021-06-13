using System.Collections.Generic;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    [AddComponentMenu("View Binding/View Context")]
    public class ViewContext : ViewContextBase
    {
        [SerializeReference]
        private List<ViewVariable> vars = new List<ViewVariable>();

        [SerializeReference]
        private List<ViewEvent> evts = new List<ViewEvent>();

        protected internal override int VariablesCount => vars.Count;
        protected internal override int EventCount => evts.Count;

        protected internal override ViewVariable GetVariable(int index) => vars[index];
        protected internal override ViewEvent GetEvent(int index) => evts[index];

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
    }
}