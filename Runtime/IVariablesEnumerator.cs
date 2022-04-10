namespace CodeWriter.ViewBinding
{
    public interface IVariablesEnumerator
    {
        void Reset();

        bool TryGetNextVariable(out ViewVariable variable);
    }

    public struct ViewContextVariablesEnumerator : IVariablesEnumerator
    {
        private readonly ViewContextBase _context;
        private readonly int _variablesCount;

        private int _variableIndex;

        public ViewContextVariablesEnumerator(ViewContextBase context)
        {
            _context = context;
            _variablesCount = context != null ? context.VariablesCount : 0;
            _variableIndex = 0;
        }

        public void Reset()
        {
            _variableIndex = 0;
        }

        public bool TryGetNextVariable(out ViewVariable variable)
        {
            if (_variableIndex >= _variablesCount)
            {
                variable = default;
                return false;
            }

            variable = _context.GetVariable(_variableIndex);

            _variableIndex += 1;

            return true;
        }
    }

    public struct ViewContextPlusArrayVariablesEnumerator : IVariablesEnumerator
    {
        private readonly ViewContextBase _context;
        private readonly int _variablesCount;

        private int _variableIndex;
        private ViewContextArrayVariablesEnumerator _enumerator;

        public ViewContextPlusArrayVariablesEnumerator(ViewContextBase context, ViewContextBase[] contexts)
        {
            _context = context;
            _variablesCount = context != null ? context.VariablesCount : 0;
            _variableIndex = 0;
            _enumerator = new ViewContextArrayVariablesEnumerator(contexts);
        }

        public void Reset()
        {
            _variableIndex = 0;
        }

        public bool TryGetNextVariable(out ViewVariable variable)
        {
            if (_variableIndex >= _variablesCount)
            {
                return _enumerator.TryGetNextVariable(out variable);
            }

            variable = _context.GetVariable(_variableIndex);

            _variableIndex += 1;

            return true;
        }
    }

    public struct ViewContextArrayVariablesEnumerator : IVariablesEnumerator
    {
        private readonly ViewContextBase[] _contexts;
        private int _contextIndex;
        private int _variableIndex;
        private int _variablesCount;

        public ViewContextArrayVariablesEnumerator(ViewContextBase[] contexts)
        {
            _contexts = contexts;
            _contextIndex = -1;

            _variableIndex = 0;
            _variablesCount = 0;
        }

        public void Reset()
        {
            _contextIndex = -1;

            _variableIndex = 0;
            _variablesCount = 0;
        }

        public bool TryGetNextVariable(out ViewVariable variable)
        {
            while (_variableIndex >= _variablesCount)
            {
                if (_contextIndex + 1 >= _contexts.Length)
                {
                    variable = default;
                    return false;
                }

                _contextIndex += 1;

                if (_contexts[_contextIndex] == null)
                {
                    continue;
                }

                _variableIndex = 0;
                _variablesCount = _contexts[_contextIndex].VariablesCount;
            }

            variable = _contexts[_contextIndex].GetVariable(_variableIndex);

            _variableIndex += 1;

            return true;
        }
    }
}