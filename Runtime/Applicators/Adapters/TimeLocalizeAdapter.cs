using System;
using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    [AddComponentMenu("View Binding/Adapters/Time Localize")]
    public class TimeLocalizeAdapter : TimeLocalizeAdapterBase
    {
        [Space]
        [SerializeField]
        private string formatDays = "TIME_D_H";

        [SerializeField]
        private string formatHours = "TIME_H_M";

        [SerializeField]
        private string formatMinutes = "TIME_M_S";

        [SerializeField]
        private string formatSeconds = "TIME_S";

        [SerializeField]
        private string formatZero = "TIME_S";

        protected override string GetTimeStringFormat(TimeSpan span)
        {
            days.Value = (int) span.TotalDays;
            if (days.Value > 0)
            {
                hours.Value = span.Hours;
                minutes.Value = span.Minutes;
                seconds.Value = span.Seconds;

                if (!string.IsNullOrEmpty(formatDays))
                {
                    return formatDays;
                }
            }

            hours.Value = (int) span.TotalHours;
            if (hours.Value > 0)
            {
                minutes.Value = span.Minutes;
                seconds.Value = span.Seconds;

                if (!string.IsNullOrEmpty(formatHours))
                {
                    return formatHours;
                }
            }

            minutes.Value = (int) span.TotalMinutes;
            if (minutes.Value > 0)
            {
                seconds.Value = span.Seconds;

                if (!string.IsNullOrEmpty(formatMinutes))
                {
                    return formatMinutes;
                }
            }

            seconds.Value = (int) span.TotalSeconds;
            if (seconds.Value > 0)
            {
                if (!string.IsNullOrEmpty(formatSeconds))
                {
                    return formatSeconds;
                }
            }

            return formatZero;
        }
    }
}