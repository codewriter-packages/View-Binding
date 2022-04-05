using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators
{
    [RequireComponent(typeof(Transform))]
    [AddComponentMenu("View Binding/GameObject Activity")]
    public sealed class GameObjectActivityApplicator : ComponentApplicatorBase<Transform, ViewVariableBool>
    {
        [SerializeField] private bool inverse;

        protected override void Apply(Transform target, ViewVariableBool source)
        {
            target.gameObject.SetActive(source.Value != inverse);

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(target);
            }
#endif
        }
    }
}