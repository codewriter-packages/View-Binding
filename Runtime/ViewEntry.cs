using System;
using UnityEngine;

namespace CodeWriter.ViewBinding
{
    [Serializable]
    public abstract class ViewEntry
    {
        [SerializeField]
        private ViewContextBase context;

        [SerializeField]
        private string name;

        public abstract string TypeDisplayName { get; }

        public abstract Type Type { get; }

        public string Name => name;

        public ViewContextBase Context => context;

        public void SetName(string newName)
        {
            name = newName;
        }

        public void SetContext(ViewContextBase newContext)
        {
            context = newContext;
        }

        internal virtual string GetErrorMessage()
        {
            if (context == null)
            {
                return "Context is missing";
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return "Name is required";
            }

            return null;
        }
    }
}