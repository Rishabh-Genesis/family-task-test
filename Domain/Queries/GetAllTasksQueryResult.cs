using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Queries
{
    public class GetAllTasksQueryResult
    {
        public List<TaskVm> Payload { get; set; }
    }
}
