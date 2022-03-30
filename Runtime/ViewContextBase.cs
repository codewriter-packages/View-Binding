namespace CodeWriter.ViewBinding
{
    public abstract class ViewContextBase : ViewBindingBehaviour
    {
        protected internal abstract int VariablesCount { get; }
        protected internal abstract int EventCount { get; }

        protected internal abstract ViewVariable GetVariable(int index);
        protected internal abstract ViewEvent GetEvent(int index);

        internal bool TryGetRootVariableFor<TVariable>(ViewVariable variable, out TVariable rootVariable, bool selfIsOk = false)
            where TVariable : ViewVariable
        {
            for (int index = 0, count = VariablesCount; index < count; index++)
            {
                var other = GetVariable(index);

                if ((selfIsOk || other != variable) &&
                    other.Type == variable.Type &&
                    other.Name == variable.Name &&
                    other is TVariable tOther)
                {
                    rootVariable = tOther;
                    return true;
                }
            }

            rootVariable = default;
            return false;
        }

        internal bool TryGetRootEventFor<TEvent>(ViewEvent evt, out TEvent rootEvent, bool selfIsOk = false)
        {
            for (int index = 0, count = EventCount; index < count; index++)
            {
                var other = GetEvent(index);

                if ((selfIsOk || other != evt) &&
                     other.Type == evt.Type &&
                     other.Name == evt.Name &&
                     other is TEvent tOther)
                {
                    rootEvent = tOther;
                    return true;
                }
            }

            rootEvent = default;
            return false;
        }
    }
}