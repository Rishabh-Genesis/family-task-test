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
        public TaskDataService(IHttpClientFactory clientFactory)
        {
            httpClient = clientFactory.CreateClient("FamilyTaskAPI");
            tasks = new List<TaskVm>();
        }



        private List<TaskVm> tasks;
        public List<TaskVm> Tasks => tasks;
        public TaskModel SelectedTask { get; private set; }


        public event EventHandler TasksUpdated;
        public event EventHandler TaskSelected;

        public void SelectTask(Guid id)
        {
            //SelectedTask = Tasks.SingleOrDefault(t => t.Id == id);
            TasksUpdated?.Invoke(this, null);
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
            //Tasks.Add(model);
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
                return await httpClient.PostJsonAsync<CreateTaskCommandResult>("https://localhost:5001/api/Task/Create", command);//Todo : put the url at appropriate place 
            }
            catch (Exception e) {
                return null;
            }
        }
        public async Task<UpdateTaskCommandResult> Update(UpdateTaskCommand command)
        {
            try
            {
                var result =  await httpClient.PutJsonAsync<UpdateTaskCommandResult>("https://localhost:5001/api/Task/UpdateTask/"+command.Id, command);//Todo : put the url at appropriate place 
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<GetAllTasksByMemberQueryResult> GetAllTaskByMember(MemberVm memberVm)
        {
             var result  = await httpClient.GetJsonAsync<GetAllTasksByMemberQueryResult>("https://localhost:5001/api/Task/GetAllTask/memberId="+ memberVm.Id);
            tasks = result.Payload;
            return result;
        }

        public async Task<GetAllTasksQueryResult> GetAllTask()
        {
            var result = await httpClient.GetJsonAsync<GetAllTasksQueryResult>("https://localhost:5001/api/Task/GetAllTask");
            tasks = result.Payload;
            return result;
        }
    }
}