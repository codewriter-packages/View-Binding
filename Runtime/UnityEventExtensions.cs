using UniMob;
using UnityEngine.Events;

// ReSharper disable IdentifierTypo

namespace CodeWriter.ViewBinding
{
    internal static class UnityEventExtensions
    {
        public static void AddLifetimedListener(this UnityEvent evt, Lifetime lifetime, UnityAction call)
        {
            if (lifetime.IsDisposed)
            {
                return;
            }

            evt.AddListener(call);
            lifetime.Register(() => evt.RemoveListener(call));
        }

        public static void AddLifetimedListener<T>(this UnityEvent<T> evt, Lifetime lifetime, UnityAction<T> call)
        {
            if (lifetime.IsDisposed)
            {
                return;
            }

            evt.AddListener(call);
            lifetime.Register(() => evt.RemoveListener(call));
        }
    }
}