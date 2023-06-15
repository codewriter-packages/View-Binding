using TriInspector;
using UniMob;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    [HideMonoScript]
    public abstract class ViewBindingBehaviour : MonoBehaviour
    {
        public static void Setup(ViewBindingBehaviour viewBindingBehaviour, Lifetime lifetime)
        {
            viewBindingBehaviour.Setup(lifetime);
        }

        public static void LinkToRender(ViewBindingBehaviour viewBindingBehaviour)
        {
            viewBindingBehaviour.LinkToRender();
        }
        
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