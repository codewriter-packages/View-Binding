#if UNIMOB_UI

using System;
using JetBrains.Annotations;
using UniMob;
using UniMob.UI;

namespace CodeWriter.ViewBinding
{
    public static class ViewVariableExtensions
    {
        [PublicAPI]
        public static void Bind<T, TVariable, TState>(this ViewVariable<T, TVariable> variable,
            View<TState> it, Func<T> f)
            where TState : class, IViewState
            where TVariable : ViewVariable<T, TVariable>
        {
            var atom = Atom.Computed(it.ViewLifetime, f);
            var activation = new Action(() =>
            {
                atom.Invalidate();
                variable.SetSource(atom);
            });
            var deactivation = new Action(() => variable.SetSource(null));

            it.AddActivationCallback(activation);
            it.AddDeactivationCallback(deactivation);

            if (it.HasState)
            {
                using (Atom.NoWatch)
                {
                    activation.Invoke();
                }
            }
        }

        [PublicAPI]
        public static void Bind<T, TEvent, TState>(this ViewParametrizedEvent<T, TEvent> evt,
            View<TState> it, Action<T> f)
            where TState : class, IViewState
            where TEvent : ViewParametrizedEvent<T, TEvent>
        {
            var activation = new Action(() => evt.AddListener(f));
            var deactivation = new Action(() => evt.RemoveListener(f));

            it.AddActivationCallback(activation);
            it.AddDeactivationCallback(deactivation);

            if (it.HasState)
            {
                using (Atom.NoWatch)
                {
                    activation.Invoke();
                }
            }
        }

        [PublicAPI]
        public static void Bind<TState>(this ViewEventVoid evt,
            View<TState> it, Action f)
            where TState : class, IViewState
        {
            var activation = new Action(() => evt.AddListener(f));
            var deactivation = new Action(() => evt.RemoveListener(f));

            it.AddActivationCallback(activation);
            it.AddDeactivationCallback(deactivation);

            if (it.HasState)
            {
                using (Atom.NoWatch)
                {
                    activation.Invoke();
                }
            }
        }
    }
}

#endif