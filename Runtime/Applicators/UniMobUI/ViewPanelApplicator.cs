#if UNIMOB_UI

using UnityEngine;
using UniMob.UI.Widgets;

namespace CodeWriter.ViewBinding.Applicators.UniMobUI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ViewPanel))]
    [AddComponentMenu("View Binding/UI/ViewPanel Applicator")]
    public sealed class ViewPanelApplicator : ComponentApplicatorBase<ViewPanel, ViewVariableState>
    {
        [SerializeField] private bool link = true;

        protected override void Apply(ViewPanel target, ViewVariableState source)
        {
            var state = source.Value;
            var view = (UniMob.UI.IView) target;

            if (state != null)
            {
                view.SetSource(state.InnerViewState, link);
            }
            else
            {
                view.ResetSource();
            }
        }
    }
}

#endif