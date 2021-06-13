using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace CodeWriter.ViewBinding.Editor
{
    [CustomPropertyDrawer(typeof(ViewEvent), true)]
    internal class ViewEventDrawer : ViewEntryDrawerBase<ViewEvent>
    {
        protected override IEnumerable<ViewEvent> EnumerateEntries(ViewContextBase context)
        {
            return Enumerable.Range(0, context.EventCount).Select(context.GetEvent);
        }
    }
}