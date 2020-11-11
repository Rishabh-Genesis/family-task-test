using AutoMapper;
using Core.Abstractions.Repositories;
using Core.Abstractions.Services;
using Domain.Commands;
using Domain.Queries;
using Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        public TaskService(IMapper mapper, ITaskRepository taskRepository)
        {
            _mapper = mapper;
            _taskRepository = taskRepository;
        }
        public async Task<CreateTaskCommandResult> CreateTaskCommandHandler(CreateTaskCommand command)
        {
           var task = _mapper.Map<Domain.DataModels.Task>(command);
           var createdTask = await _taskRepository.CreateRecordAsync(task);
           var vm = _mapper.Map<TaskVm>(createdTask);

            return new CreateTaskCommandResult()
            {
                Payload = vm
            };
        }

        public async Task<GetAllTasksQueryResult> GetAllTasksQueryHandler(Guid memberId)
        {
            List<TaskVm> vm = new List<TaskVm>();

            var tasks = await _taskRepository.Reset().ToListAsync();

            if (tasks != null && tasks.Any())
                vm = _mapper.Map<List<TaskVm>>(tasks.Where(t=>t.AssignedToId == memberId));

            return new GetAllTasksQueryResult()
            {
                Payload = vm
            };
        }
    }
}
