using System;
using System.ComponentModel;

namespace TMDbLib.Objects.Movies
{
    [Flags]
    public enum MovieMethods
    {
        [Description("Undefined")]
        Undefined = 0,
        [Description("alternative_titles")]
        AlternativeTitles = 1,
        [Description("credits")]
        Credits = 2,
        [Description("images")]
        Images = 4,
        [Description("keywords")]
        Keywords = 8,
        [Description("releases")]
        Releases = 16,
        [Description("videos")]
        Videos = 32,
        [Description("translations")]
        Translations = 64,
        [Description("similar")]
        Similar = 128,
        [Description("reviews")]
        Reviews = 256,
        [Description("lists")]
        Lists = 512,
        [Description("changes")]
        Changes = 1024,
        /// <summary>
        /// Requires a valid user session to be set on the client object
        /// </summary>
        [Description("account_states")]
        AccountStates = 2048
    }
}