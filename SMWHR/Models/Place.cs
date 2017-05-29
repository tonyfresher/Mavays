using Org.BouncyCastle.Asn1;
using Org.W3c.Dom;

namespace SMWHR.Models
{
    public class Place
    {
        public string Name { get; set; }
        public string Coordinate { get; set; }
        public string Description { get; set; }

        public Place() { }

        public Place(string name, string description)
        {
            Name = name;
            Description = description;
            Coordinate = "";
        }
    }
}