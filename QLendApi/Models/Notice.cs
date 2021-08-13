using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace QLendApi.Models
{
    [Table("Notice")]
    public class Notice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public string? Link { get; set; }
        public DateTime? CreateTime { get; set; }
        public int ForeignWorkerId { get; set; }
        // public virtual ForeignWorker ForeignWorkerIdNavigation { get; set; }
    }
}
