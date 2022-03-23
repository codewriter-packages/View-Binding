using UniMob;
using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    public class BoolToStringAdapter : ViewContextBase
#if UNITY_EDITOR
        , IEditorViewContextListener
#endif
    {
        [SerializeField]
        private ViewVariableBool source;

        [SerializeField]
        private string trueString = "TRUE";

        [SerializeField]
        private string falseString = "FALSE";

        [SerializeField]
        private string resultName = "Result";

        [SerializeField]
        [HideInInspector]
        private ViewVariableString result;

        private readonly LifetimeController _lifetimeController = new LifetimeController();

        protected override int VariablesCount => 1;
        protected override int EventCount => 0;

        protected override ViewVariable GetVariable(int index) => result;
        protected override ViewEvent GetEvent(int index) => null;

        protected override void Start()
        {
            base.Start();

            result.SetSource(Atom.Computed(_lifetimeController.Lifetime, Adapt));
        }

        protected override void OnDestroy()
        {
            result.SetSource(null);
            _lifetimeController?.Dispose();

            base.OnDestroy();
        }

        private string Adapt()
        {
            return source.Value ? trueString : falseString;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            source.Context.AddEditorListener(this);

            result.SetContext(this);
            result.SetName(resultName);
        }

        public void OnEditorContextVariableChanged(ViewVariable variable)
        {
            if (variable.IsRootVariableFor(source))
            {
                result.SetValueEditorOnly(Adapt());
                NotifyEditorVariableChanged(result);
            }
        }
#endif
    }
}