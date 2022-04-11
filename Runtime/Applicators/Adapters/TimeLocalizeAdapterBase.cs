using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    public abstract class TimeLocalizeAdapterBase
        : SingleResultAdapterBase<string, TimeLocalizeAdapterBase.ViewVariableTimeLocalized>
    {
        [Space]
        [SerializeField]
        private ViewVariableInt totalSeconds = default;

        protected readonly ViewVariableInlineInt days = new ViewVariableInlineInt("d");
        protected readonly ViewVariableInlineInt hours = new ViewVariableInlineInt("h");
        protected readonly ViewVariableInlineInt minutes = new ViewVariableInlineInt("m");
        protected readonly ViewVariableInlineInt seconds = new ViewVariableInlineInt("s");

        protected override string Adapt()
        {
            return "";
        }

        protected abstract string GetTimeStringFormat(TimeSpan span);

        [Serializable, Preserve]
        public class ViewVariableTimeLocalized : ViewVariable<string, ViewVariableTimeLocalized>
        {
            private TimeVariablesEnumerator _timeVariablesEnumerator;

            [Preserve]
            public ViewVariableTimeLocalized()
            {
            }

            public override void AppendValueTo(ref ValueTextBuilder builder)
            {
                var adapter = (TimeLocalizeAdapterBase) Context;

                if (_timeVariablesEnumerator == null)
                {
                    _timeVariablesEnumerator = new TimeVariablesEnumerator(adapter);
                }

                var format = adapter.GetTimeStringFormat(TimeSpan.FromSeconds(adapter.totalSeconds.Value));

                var formatTextBuilder = new ValueTextBuilder(ValueTextBuilder.DefaultCapacity);
                try
                {
                    formatTextBuilder.Append(format);
                    var localizedString = BindingsLocalization.Localize(ref formatTextBuilder);

                    builder.AppendFormat(localizedString, _timeVariablesEnumerator);
                }
                finally
                {
                    formatTextBuilder.Dispose();
                }
            }
        }

        [Serializable, Preserve]
        public class ViewVariableInlineInt : ViewVariable
        {
            public int Value { get; set; }

            public override string TypeDisplayName => "Embed";
            public override Type Type => typeof(int);

            [Preserve]
            public ViewVariableInlineInt(string name)
            {
                SetName(name);
            }

            public override void AppendValueTo(ref ValueTextBuilder builder)
            {
                builder.Append(Value);
            }

            public override bool IsRootVariableFor(ViewVariable viewVariable)
            {
                return false;
            }
        }

        private class TimeVariablesEnumerator : IVariablesEnumerator
        {
            private readonly TimeLocalizeAdapterBase _adapter;

            private int _variableIndex;

            public TimeVariablesEnumerator(TimeLocalizeAdapterBase adapter)
            {
                _adapter = adapter;
            }

            public void Reset()
            {
                _variableIndex = 0;
            }

            public bool TryGetNextVariable(out ViewVariable variable)
            {
                switch (_variableIndex)
                {
                    case 0:
                        variable = _adapter.days;
                        break;

                    case 1:
                        variable = _adapter.hours;
                        break;

                    case 2:
                        variable = _adapter.minutes;
                        break;

                    case 3:
                        variable = _adapter.seconds;
                        break;

                    default:
                        variable = default;
                        return false;
                }

                _variableIndex += 1;
                return true;
            }
        }
    }
}