using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestAPI.Models
{
    [Table("Document")]
    public class Document
    {
        [Key()]
        public Guid Id { get; set; }
        public StatusEnum Status { get; set; }
        public string? Data { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime? Modified_Date { get; set; }
    }
    public enum StatusEnum
    {
        [Description("Черновик")]
        draft,
        [Description("Опубликован")]
        published
    }
}
