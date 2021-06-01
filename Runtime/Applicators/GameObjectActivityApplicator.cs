using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators
{
    public sealed class GameObjectActivityApplicator : Applicator<Transform, ViewVariableBool>
    {
        [SerializeField] private bool inverse;

        protected override void Apply(Transform target, ViewVariableBool source)
        {
            target.gameObject.SetActive(source.Value != inverse);
        }
    }
}