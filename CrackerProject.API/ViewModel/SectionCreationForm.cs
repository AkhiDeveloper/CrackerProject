using CrackerProject.API.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace CrackerProject.API.ViewModel
{
    public class SectionCreationForm
    {
        public int Sn { get; set; } = 1;

        public string Name { get; set; } = "Write name of Book Section";

        public string Description { get; set; } = "Write Description";
    }
}
