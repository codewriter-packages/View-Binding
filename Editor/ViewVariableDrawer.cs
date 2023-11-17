using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace CodeWriter.ViewBinding.Editor
{
    [CustomPropertyDrawer(typeof(ViewVariable), true)]
    internal class ViewVariableDrawer : ViewEntryDrawerBase<ViewVariable>
    {
        protected override IEnumerable<ViewVariable> EnumerateEntries(ViewContextBase context)
        {
            return Enumerable.Range(0, context.VariablesCount).Select(context.GetVariable);
        }

        protected override string GetNoEntriesMessage()
        {
            var type = ViewEntryUtils.GetValueTypeFromEntryType(fieldInfo.FieldType);
            return $"No variables of type '{type.Name}' found in parents";
        }
    }
}