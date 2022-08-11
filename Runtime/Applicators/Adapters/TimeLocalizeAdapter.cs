using System;
using UnityEngine;

namespace CodeWriter.ViewBinding.Applicators.Adapters
{
    [AddComponentMenu("View Binding/Adapters/[Binding] Time Localize Adapter")]
    public class TimeLocalizeAdapter : TimeLocalizeAdapterBase
    {
        [Space]
        [SerializeField]
        private bool showDays = false;

        [SerializeField]
        private bool showHours = false;

        [SerializeField]
        private bool showMinutes = false;

        [Space]
        [SerializeField]
        private string formatDaysHoursMinutes = "TIME_D_H_M";

        [SerializeField]
        private string formatHoursMinutesSeconds = "TIME_H_M_S";

        [SerializeField]
        private string formatDaysHours = "TIME_D_H";

        [SerializeField]
        private string formatHoursMinutes = "TIME_H_M";

        [SerializeField]
        private string formatMinutesSeconds = "TIME_M_S";

        [SerializeField]
        private string formatHours = "TIME_H";

        [SerializeField]
        private string formatDays = "TIME_D";

        [SerializeField]
        private string formatMinutes = "TIME_M";

        [SerializeField]
        private string formatSeconds = "TIME_S";

        [SerializeField]
        private string formatZero = "TIME_S";

        protected override string GetTimeStringFormat(TimeSpan span)
        {
            days.Value = (int) span.TotalDays;
            if (days.Value > 0 && showDays)
            {
                hours.Value = span.Hours;
                minutes.Value = span.Minutes;
                seconds.Value = span.Seconds;

                if (hours.Value > 0 && minutes.Value > 0)
                {
                    return formatDaysHoursMinutes;
                }

                if (hours.Value > 0)
                {
                    return formatDaysHours;
                }

                return formatDays;
            }

            hours.Value = (int) span.TotalHours;
            if (hours.Value > 0 && showHours)
            {
                minutes.Value = span.Minutes;
                seconds.Value = span.Seconds;

                if (minutes.Value > 0 && seconds.Value > 0)
                {
                    return formatHoursMinutesSeconds;
                }

                if (minutes.Value > 0)
                {
                    return formatHoursMinutes;
                }

                return formatHours;
            }

            minutes.Value = (int) span.TotalMinutes;
            if (minutes.Value > 0 && showMinutes)
            {
                seconds.Value = span.Seconds;

                if (seconds.Value > 0)
                {
                    return formatMinutesSeconds;
                }

                return formatMinutes;
            }

            seconds.Value = (int) span.TotalSeconds;
            if (seconds.Value > 0)
            {
                return formatSeconds;
            }

            return formatZero;
        }
    }
}