using System.Collections.Generic;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    [AddComponentMenu("View Binding/View Context")]
    public class ViewContext : ViewContextBase
    {
        [SerializeReference]
        private List<ViewVariable> vars = new List<ViewVariable>();

        protected internal override int VariablesCount => vars.Count;

        protected internal override ViewVariable GetVariable(int index) => vars[index];

        protected void UnsafeRegisterVariable(ViewVariable variable)
        {
            vars.Add(variable);
        }

        public ViewVariable FindVariable(string variableName)
        {
            foreach (var variable in vars)
            {
                if (variable.Name == variableName)
                {
                    return variable;
                }
            }

            return null;
        }

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
    }
}