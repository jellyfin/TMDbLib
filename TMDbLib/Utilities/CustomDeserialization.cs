using System;
using System.Globalization;
using System.Text.RegularExpressions;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

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

        internal static void DeserializeAccountStatesRating(ResultContainer<TvEpisodeAccountState> accountState, string responseContent)
        {
            // Match both below, capture either "false" or "3.0" (numbers)
            // "rated":{"value":3.0}
            // "rated":false
            const string rgx = "\"rated\":(?:(?<value>false)|\\{\"value\":(?<value>\\d+(?:\\.\\d{1,2}))\\})";

            Regex regex = new Regex(rgx, RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(responseContent);

            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];

                string value = match.Groups["value"].Value;

                if (value == "false")
                    accountState.Results[i].Rating = null;
                else
                    accountState.Results[i].Rating = Double.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            }
        }

        internal static void DeserializeAccountStatesRating(TvEpisodeAccountState accountState, string responseContent)
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
