using UnityEngine;
using UnityEngine.Serialization;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    public class BoolToStringAdapter : ViewContextBase
#if UNITY_EDITOR
        , IEditorViewContextListener
#endif
    {
        [SerializeField]
        [FormerlySerializedAs("resultName")]
        private string alias = "Result";

        [Space]
        [SerializeField]
        private ViewVariableBool source;

        [SerializeField]
        private string trueString = "TRUE";

        [SerializeField]
        private string falseString = "FALSE";

        [SerializeField]
        [HideInInspector]
        private ViewVariableString result;

        public bool IsDestroyed => this == null;

        protected override int VariablesCount => 1;
        protected override int EventCount => 0;

        protected override ViewVariable GetVariable(int index) => result;
        protected override ViewEvent GetEvent(int index) => null;

        public override void OnContextStart()
        {
            base.OnContextStart();

            result.SetSource(Adapt);
        }

        public override void OnContextDestroy()
        {
            result.SetSource(null);

            base.OnContextDestroy();
        }

        private string Adapt()
        {
            return source.Value ? trueString : falseString;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (source != null && source.Context != null)
            {
                source.Context.AddEditorListener(this);
            }

            result.SetContext(this);
            result.SetName(alias);
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