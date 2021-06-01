namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    public sealed class UnityEventIntApplicator : UnityEventApplicatorBase<int, ViewVariableInt>
    {
        protected override void Apply() => callback.Invoke(source.Value);
    }
}