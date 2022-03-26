using UnityEngine;

namespace CodeWriter.ViewBinding
{
    public abstract class ViewBindingBehaviour : MonoBehaviour
    {
        public virtual void OnContextStart()
        {
        }

        public virtual void OnContextDestroy()
        {
        }

#if UNITY_EDITOR
        protected virtual void Reset()
        {
        }

        protected virtual void OnValidate()
        {
        }
#endif
    }
}