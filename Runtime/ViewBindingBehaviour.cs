using UniMob;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    public abstract class ViewBindingBehaviour : MonoBehaviour
    {
        protected internal virtual void Setup(Lifetime lifetime)
        {
        }

        protected internal virtual void LinkToRender()
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