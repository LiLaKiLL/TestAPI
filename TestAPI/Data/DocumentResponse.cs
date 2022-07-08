using TestAPI.Models;

namespace TestAPI.Data
{
    public class DocumentResponse
    {
        public List<Document> Documents { get; set; } = new List<Document>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }

    }
}
