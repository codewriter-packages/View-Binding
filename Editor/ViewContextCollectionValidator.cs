using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeWriter.ViewBinding.Editor;
using TriInspector;
using UnityEngine;

[assembly: RegisterTriAttributeValidator(typeof(ViewContextCollectionValidator))]

namespace CodeWriter.ViewBinding.Editor
{
    public class ViewContextCollectionValidator : TriAttributeValidator<ViewContextCollectionAttribute>
    {
        private static readonly List<ViewContextBase> TempContexts = new List<ViewContextBase>();
        private static readonly List<ViewContextBase> TempSelfValues = new List<ViewContextBase>();

        public override TriExtensionInitializationResult Initialize(TriPropertyDefinition propertyDefinition)
        {
            if (propertyDefinition.FieldType != typeof(ViewContextBase[]) &&
                propertyDefinition.FieldType != typeof(List<ViewContextBase>))
            {
                return "[ExtraViewContextCollection] can be used only on List<ViewContextBase>";
            }

            return base.Initialize(propertyDefinition);
        }

        public override TriValidationResult Validate(TriProperty property)
        {
            var mb = property.PropertyTree.RootProperty.Value as MonoBehaviour;

            if (mb == null)
            {
                return TriValidationResult.Warning("TargetObject is not MonoBehaviour");
            }

            FillContext(mb, TempContexts);

            if (TempContexts.SequenceEqual(property.Value as IList<ViewContextBase> ?? Array.Empty<ViewContextBase>()))
            {
                return TriValidationResult.Valid;
            }

            return CreateFix(mb, property);
        }

        private TriValidationResult CreateFix(MonoBehaviour mb, TriProperty property)
        {
            return TriValidationResult.Info("Contexts is out of sync").WithFix(() =>
            {
                FillContext(mb, TempContexts);

                var convertedContexts = property.FieldType == typeof(ViewContextBase[])
                    ? (object) TempContexts.ToArray()
                    : TempContexts.ToList();

                if (!property.TryGetMemberInfo(out var mi))
                {
                    return;
                }

                switch (mi)
                {
                    case FieldInfo fi:
                        fi.SetValue(property.Parent.Value, convertedContexts);
                        break;

                    case PropertyInfo pi:
                        pi.SetValue(property.Parent.Value, convertedContexts);
                        break;
                }
            }, "Fill Contexts");
        }

        private static void FillContext(MonoBehaviour mb, List<ViewContextBase> contexts)
        {
            contexts.Clear();
            mb.GetComponentsInParent<ViewContextBase>(true, contexts);
            contexts.RemoveAll(it => it == null || it == mb);

            if (mb is ViewContextBase targetViewContext)
            {
                mb.GetComponents<ViewContextBase>(TempSelfValues);

                contexts.RemoveAll(it =>
                {
                    var selfIndex = TempSelfValues.IndexOf(targetViewContext);
                    var otherIndex = TempSelfValues.IndexOf(it);
                    return selfIndex < otherIndex;
                });
            }
        }
    }
}