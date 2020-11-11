using System;

namespace WebClient.Shared.Models
{
    public class TaskModel
    {
        public Guid Id { get; set; } 
        public Guid Member { get; set; }
        public string Subject { get; set; }
        public bool IsComplete { get; set; }
    }
}
