using TMDbLib.Objects.General;

namespace TMDbLib.Objects.Companies
{
    public class Company
    {
        public string Description { get; set; }
        public string Headquarters { get; set; }
        public string Homepage { get; set; }
        public int Id { get; set; }
        public string LogoPath { get; set; }
        public string Name { get; set; }
        public ParentCompany ParentCompany { get; set; }
        public MovieResultContainer Movies { get; set; }
    }
}