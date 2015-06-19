using System;
using System.Globalization;
using System.Text.RegularExpressions;
using TMDbLib.Objects.General;

namespace TMDbLib.Utilities
{
    internal class CustomDeserialization
    {
        internal static void DeserializeAccountStatesRating(AccountState accountState, string responseContent)
        {
            const string selector = @"""rated"":{""value"":(?<value>\d+(?:\.\d{1,2}))}";
            Regex regex = new Regex(selector, RegexOptions.IgnoreCase);
            Match match = regex.Match(responseContent);
            if (match.Success)
            {
                accountState.Rating = Double.Parse(match.Groups["value"].Value,
                    CultureInfo.InvariantCulture.NumberFormat);
            }
        }
    }
}
