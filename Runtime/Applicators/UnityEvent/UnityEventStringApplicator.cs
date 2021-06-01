namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    public sealed class UnityEventStringApplicator : UnityEventApplicatorBase<string, ViewVariableString>
    {
        protected override void Apply() => callback.Invoke(source.Value);
    }
}