using UnityEngine;

namespace CodeWriter.ViewBinding
{
    public abstract class ApplicatorBase : MonoBehaviour, IViewContextListener
    {
        protected virtual void Start()
        {
            ReSubscribe();
        }

        protected virtual void OnValidate()
        {
            ReSubscribe();
        }

        protected abstract void ReSubscribe();

        public abstract void OnContextVariableChanged(ViewVariable variable);
    }
}