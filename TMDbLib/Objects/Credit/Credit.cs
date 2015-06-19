namespace TMDbLib.Objects.Credit
{
    public class Credit
    {
        public string CreditType { get; set; }
        public string Department { get; set; }
        public string Job { get; set; }
        public CreditMedia Media { get; set; }
        public string MediaType { get; set; }
        public string Id { get; set; }
        public CreditPerson Person { get; set; }
    }
}