namespace CodeWriter.ViewBinding
{
    public abstract class AdapterBase : ViewContextBase, IViewContextListener
    {
        protected override void Start()
        {
            base.Start();

            ReSubscribe();
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            ReSubscribe();
        }

        protected abstract void ReSubscribe();

        public abstract void OnContextVariableChanged(ViewVariable variable);
    }
}