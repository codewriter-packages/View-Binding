using System;

namespace CodeWriter.ViewBinding
{
    [Serializable]
    public abstract class ViewEvent<T, TEvent> : ViewEvent
        where TEvent : ViewEvent<T, TEvent>
    {
        public override string TypeDisplayName => typeof(T).Name;

        public override Type Type => typeof(T);

        public override bool IsRootEventFor(ViewEvent viewEvent)
        {
            return Context.TryGetRootEventFor<TEvent>(viewEvent, out var rootVariable) &&
                   rootVariable == this;
        }
    }

    [Serializable]
    public abstract class ViewEvent : ViewEntry
    {
        public abstract bool IsRootEventFor(ViewEvent viewEvent);

        internal override string GetErrorMessage()
        {
            var error = base.GetErrorMessage();
            if (error != null)
            {
                return error;
            }

            if (!Context.TryGetRootEventFor<ViewEvent>(this, out _, selfIsOk: true))
            {
                return "Event is missing";
            }

            return null;
        }
    }
}