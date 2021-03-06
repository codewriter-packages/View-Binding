using UnityEngine;
using UnityEngine.Events;

namespace CodeWriter.ViewBinding.Applicators
{
    public abstract class UnityEventApplicatorBase<TValue, TVariable> : ApplicatorBase
        where TVariable : ViewVariable
    {
        [SerializeField]
        protected internal TVariable source;

        [SerializeField]
        protected internal UnityEvent<TValue> callback;
    }
}