using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators
{
    [RequireComponent(typeof(Transform))]
    [AddComponentMenu("View Binding/GameObject Activity Applicator")]
    public sealed class GameObjectActivityApplicator : ComponentApplicatorBase<Transform, ViewVariableBool>
    {
        [SerializeField] private bool inverse;

        protected override void Apply(Transform target, ViewVariableBool source)
        {
            target.gameObject.SetActive(source.Value != inverse);
        }
    }
}