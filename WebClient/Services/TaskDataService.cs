using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using WebClient.Abstractions;
using WebClient.Shared.Models;
using Domain.Commands;
using System.Threading.Tasks;
using Core.Extensions.ModelConversion;
using Domain.ViewModel;
using Microsoft.AspNetCore.Components;
using Domain.Queries;

namespace WebClient.Services
{
    public class TaskDataService: ITaskDataService
    {
        private readonly HttpClient httpClient;
        
        private List<TaskVm> tasks;
        public List<TaskVm> Tasks => tasks;
        public TaskVm SelectedTask { get; private set; }


        public event EventHandler TasksUpdated;
        public event EventHandler TaskSelected;
        public event EventHandler<string> UpdateTaskFailed;
        public event EventHandler<string> CreateTaskFailed;
        public TaskDataService(IHttpClientFactory clientFactory)
        {
            httpClient = clientFactory.CreateClient("FamilyTaskAPI");
            tasks = new List<TaskVm>();
        }

        public async Task ToggleTask(Guid id)
        {
            foreach (var taskModel in tasks)
            {
                if (taskModel.Id == id)
                {
                    taskModel.IsComplete = !taskModel.IsComplete;
                    await UpdateTask(taskModel);
                }
            }

            TasksUpdated?.Invoke(this, null);
        }

        public async void AddTask(TaskVm model)
        {
            var result = await Create(ModelConversionExtensions.ToCreateTaskCommand(model));
            if (result != null) {
                TasksUpdated?.Invoke(this, null);
            }
        }

        public async Task UpdateTask(TaskVm model) 
        {
            var result = await Update(ModelConversionExtensions.ToUpdateTaskCommand(model));
            if (result != null)
            {
                TasksUpdated?.Invoke(this, null);
            }
        }

        public async Task<CreateTaskCommandResult> Create(CreateTaskCommand command) {
            try
            {
                return await httpClient.PostJsonAsync<CreateTaskCommandResult>("Task/Create", command);
            }
            catch (Exception e) {
                CreateTaskFailed?.Invoke(this, "Unable to create Task . Exception Occured ");
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<UpdateTaskCommandResult> Update(UpdateTaskCommand command)
        {
            try
            {
                var result =  await httpClient.PutJsonAsync<UpdateTaskCommandResult>("Task/UpdateTask/"+command.Id, command);
                return result;
            }
            catch (Exception e)
            {
                UpdateTaskFailed?.Invoke(this, "Unable to update Task. Exception Occured");
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<GetAllTasksByMemberQueryResult> GetAllTaskByMember(MemberVm memberVm)
        {
             var result  = await httpClient.GetJsonAsync<GetAllTasksByMemberQueryResult>("Task/GetAllTask/memberId="+ memberVm.Id);
            tasks = result.Payload;
            return result;
        }

        public async Task<GetAllTasksQueryResult> GetAllTask()
        {
            var result = await httpClient.GetJsonAsync<GetAllTasksQueryResult>("Task/GetAllTask");
            tasks = result.Payload;
            return result;
        }

        public void SelectTask(Guid taskId) {
            if (Tasks.All(taskVm => taskVm.Id != taskId)) return;
            {
                SelectedTask = Tasks.SingleOrDefault(taskVm => taskVm.Id == taskId);
            }
        }

        public async Task AssignTaskToMember(Guid memberId)
        {
            SelectedTask.AssignedToId = memberId;
            await UpdateTask(SelectedTask);
            SelectedTask = null;
        }
    }
}