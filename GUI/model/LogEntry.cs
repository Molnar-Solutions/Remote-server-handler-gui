using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogCommon
{
    [Table("LogEntries")]
    public class LogEntry
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string CorrelationId { get; set; }
        [Required]
        public int thread { get; set; }
        [Required]
        public DateTime DateUtc { get; set; }
        [Required]
        public EErrorLevel level { get; set; }
        [Required]
        public string Logger { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Exception { get; set; }
    }
}
