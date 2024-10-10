using CodingChallenge.Enums;
using System.ComponentModel.DataAnnotations;
namespace CodingChallenge.Models

{
    public class Tasks
    {
        [Key]
        public int TaskID { get;set; }
        [Required]
        public string Title {  get; set; }

        public string Description { get; set; } 
        public DateTime DueDate { get; set; }=DateTime.Now;
        public Priority Priority {  get; set; }
        public Status Status { get; set; }
    }
}
