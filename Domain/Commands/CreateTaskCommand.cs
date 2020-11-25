using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Commands
{
    public class CreateTaskCommand
    {
        public Guid? AssignedToId { get; set; }
        public string Subject { get; set; }
        public bool IsComplete { get; set; }
    }
}
