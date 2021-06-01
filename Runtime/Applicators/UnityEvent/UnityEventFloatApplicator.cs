namespace CodeWriter.ViewBinding.Applicators.UnityEvent
{
    public sealed class UnityEventFloatApplicator : UnityEventApplicatorBase<float, ViewVariableFloat>
    {
        protected override void Apply() => callback.Invoke(source.Value);
    }
}