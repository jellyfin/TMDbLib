using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMDbLib.Objects.Reviews
{
    public class Review
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }

        public int MediaId { get; set; }
        public string MediaTitle { get; set; }
        public string MediaType { get; set; }

        public string Url { get; set; }
    }
}
