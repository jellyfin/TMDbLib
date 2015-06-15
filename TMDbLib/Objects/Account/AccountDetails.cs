namespace TMDbLib.Objects.Account
{
    public class AccountDetails
    {
        public int Id { get; set; }

        public Avatar Avatar { get; set; }

        public bool IncludeAdult { get; set; }

        /// <summary>
        /// The country iso code specified by the user. Ex. US
        /// </summary>
        public string Iso_3166_1 { get; set; }

        /// <summary>
        /// The Language iso code specified by the user. Ex en
        /// </summary>
        public string Iso_639_1 { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }
    }
}
