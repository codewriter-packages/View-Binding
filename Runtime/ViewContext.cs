using System.Collections.Generic;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    public class ViewContext : ViewContextBase
    {
        [SerializeReference]
        private List<ViewVariable> variables = new List<ViewVariable>();

        internal override IEnumerable<ViewVariable> EnumerateVariables() => variables;
    }
}