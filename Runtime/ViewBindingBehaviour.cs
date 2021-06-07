using UnityEngine;

namespace CodeWriter.ViewBinding
{
    public abstract class ViewBindingBehaviour : MonoBehaviour
    {
        protected virtual void Start()
        {
        }

        protected virtual void OnDestroy()
        {
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
        }
#endif
    }
}