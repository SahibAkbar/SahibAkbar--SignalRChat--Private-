using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(500)]
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        public CustomUser Sender { get; set; }
        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
        public CustomUser Receiver { get; set; }
    }
}
