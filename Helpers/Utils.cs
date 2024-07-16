using MudBlazor;
using System.Globalization;

namespace PostmanBlazor.Helpers
{
    /// <summary>
    /// Utilities
    /// </summary>
    public class Utils
    {
        public const int DecimalPlaces = 2;

        /// <summary>
        /// Returns the percentage representation for the specified <paramref name="value"/>
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns></returns>
        public static string GetPercentageRepresentation(double value)
        {
            var percentage = GetPercentage(value);

            var roundedPercentage = percentage.ToString("F", CultureInfo.InvariantCulture);

            return $"{roundedPercentage}%";
        }

        /// <summary>
        /// Returns the Double percentage
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double GetPercentage(double value)
        {
            var percentage = value * 100;

            return Math.Round(percentage, DecimalPlaces);
        }

        /// <summary>
        /// Returns the <see cref="Color"/> based on the <paramref name="percentage"/>
        /// </summary>
        /// <param name="percentage">The percentage</param>
        /// <returns></returns>
        public static Color GetPercentageColor(double percentage)
        {
            if (percentage <= 0.3)
                return Color.Error;

            if (percentage <= 0.6)
                return Color.Warning;

            if (percentage <= 0.95)
                return Color.Info;

            return Color.Success;
        }
    }
}
